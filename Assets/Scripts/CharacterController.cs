using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
	private InputHandler handler;
	private Rigidbody _rigidbody;
	private Camera _camera;
	private float _speed = 8f;
	private float _sensitivity = 10f;

	public CharacterController(InputHandler handler)
	{
		
	}

	public void OnJump()
	{
		Debug.Log("Jujujujampu");
		
	}

	private void Awake()
	{
		_rigidbody = GetComponentInChildren<Rigidbody>();
		handler = new InputHandler(new UnityInputHandler());
		handler.JumpEvent += OnJump;

		_camera = GetComponentInChildren<Camera>();
	}

	void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	void FixedUpdate()
	{
		Vector2 moveDir = handler.ReadMoveDirection();
		//Debug.Log($"Moving, direction = {moveDir}");
		Vector3 vel = _rigidbody.velocity;
		vel.x = _speed * moveDir.x;
		vel.z = _speed * moveDir.y;
		_rigidbody.velocity.Set(moveDir.x * _speed, 0, moveDir.y * _speed);

		Vector2 lookDir = handler.ReadLookDirection();
		//Debug.Log($"Looking, direction = {lookDir}");
		Vector3 look = _camera.transform.localRotation.eulerAngles;
		look.x = _sensitivity * lookDir.x + look.x;
		look.y = _sensitivity * lookDir.y + look.y;
		_camera.gameObject.transform.Rotate(look);
	}
}
