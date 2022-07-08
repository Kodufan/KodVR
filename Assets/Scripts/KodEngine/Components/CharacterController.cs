using UnityEngine;
//using UnityEngine.InputSystem;
using KodEngine.Core;

namespace KodEngine.Component
{
	public class CharacterController : Core.Component
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		[Newtonsoft.Json.JsonIgnore]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;


		private UnityEngine.CharacterController _controller;
		private InputHandler _input;
		private GameObject _controllerObject;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		public CharacterController(KodEBase.RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public CharacterController(KodEBase.RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
		}

		public override string helpText
		{
			get
			{
				return "The CharacterController component currently serves multiple purposes: providing a viewpoint, moving the player, and handling collisions. " +
					"This functionality will later be seperated into their own components, but for testing purposes, it is currently all in one component.";
			}
			set
			{
			}
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(_controllerObject.gameObject.transform.position.x, _controllerObject.gameObject.transform.position.y - GroundedOffset, _controllerObject.gameObject.transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.ReadLookDirection().sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = 1.0f;

				_cinemachineTargetPitch += -_input.ReadLookDirection().y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.ReadLookDirection().x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				_controllerObject.transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.ReadMoveDirection() == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.ReadMoveDirection().magnitude;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.ReadMoveDirection().x, 0.0f, _input.ReadMoveDirection().y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.ReadMoveDirection() != Vector2.zero)
			{
				// move
				inputDirection = _controllerObject.transform.right * _input.ReadMoveDirection().x + _controllerObject.transform.forward * _input.ReadMoveDirection().y;

				
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void Jump()
		{
			_verticalVelocity = (Grounded) ? Mathf.Sqrt(JumpHeight * -2f * Gravity) : _verticalVelocity;
		}

		private void UpdateGravity()
		{
			if (Grounded)
			{

				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				//_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(_controllerObject.transform.position.x, _controllerObject.transform.position.y - GroundedOffset, _controllerObject.transform.position.z), GroundedRadius);
		}

		public override void OnAttach()
		{
			// Create player visual and collider
			_controllerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			_controllerObject.name = "Character controller";

			// Parent player object to the slot
			Slot ownerSlot = (Slot)owner.Resolve();
			_controllerObject.transform.SetParent(ownerSlot.gameObject.transform);

			// Create camera object and add camera component
			_mainCamera = new GameObject("Camera");
			_mainCamera.AddComponent<Camera>();

			// Parents camera to player object
			_mainCamera.transform.SetParent(_controllerObject.transform);

			// Adds Unity CharacterController to player object and sets it to collide with Default objects
			_controllerObject.AddComponent<UnityEngine.CharacterController>();
			GroundLayers = LayerMask.GetMask("Default");

			// Sets the camera target
			CinemachineCameraTarget = _mainCamera;

			// Gets the CharacterController component from the player object and initializes the Input Handler
			_controller = _controllerObject.gameObject.GetComponent<UnityEngine.CharacterController>();
			_input = Engine._inputHandler;

			//Cursor.lockState = CursorLockMode.Locked;

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			Engine.OnCommonUpdate += OnUpdate;
			_input.JumpEvent += Jump;
		}

		public override void OnUpdate()
		{
			GroundedCheck();
			Move();
			UpdateGravity();
			CameraRotation();
		}

		public override void OnDestroy()
		{
			Object.Destroy(CinemachineCameraTarget);
			Object.Destroy(_mainCamera);
			Object.Destroy(_controllerObject);
			_input.JumpEvent -= Jump;
			Engine.OnCommonUpdate -= OnUpdate;
		}

		public override void OnChange()
		{
		}
	}
}