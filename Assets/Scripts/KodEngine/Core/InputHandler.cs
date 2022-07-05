using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void Action();

public class InputHandler
{
	// Declared Unity Input Actions
	private InputAction _lookAction;
	private InputAction _moveAction;
	private InputAction _jumpAction;
	private InputAction _toggleSessionAction;
	private InputAction _primaryInteractAction;

	// KodEngine Input Actions
	public event Action JumpEvent;
	public event Action ToggleSessionEvent;
	public event Action PrimaryInteractAction;

	public InputHandler(UnityInputHandler unityInputHandler)
	{
		_lookAction = unityInputHandler.Player.Look;
		_lookAction.Enable();
		
		_moveAction = unityInputHandler.Player.Move;
		_moveAction.Enable();
		
		_jumpAction = unityInputHandler.Player.Jump;
		unityInputHandler.Player.Jump.performed += OnJump;
		_jumpAction.Enable();
		
		_toggleSessionAction = unityInputHandler.Player.ToggleFocusedSession;
		unityInputHandler.Player.ToggleFocusedSession.performed += OnToggleSession;
		_toggleSessionAction.Enable();

		_primaryInteractAction = unityInputHandler.Player.PrimaryInteract;
		unityInputHandler.Player.PrimaryInteract.performed += OnPrimaryInteract;
		_primaryInteractAction.Enable();
		
	}

	void OnJump(InputAction.CallbackContext context)
	{
		JumpEvent?.Invoke();
	}

	void OnToggleSession(InputAction.CallbackContext context)
	{
		ToggleSessionEvent?.Invoke();
	}

	void OnPrimaryInteract(InputAction.CallbackContext context)
	{
		PrimaryInteractAction?.Invoke();
	}

	public Vector2 ReadMoveDirection()
	{
		return _moveAction.ReadValue<Vector2>();
	}

	public Vector2 ReadLookDirection()
	{
		return _lookAction.ReadValue<Vector2>();
	}
}
