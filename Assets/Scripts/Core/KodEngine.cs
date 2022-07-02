using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Update();

public class KodEngine : MonoBehaviour
{
	private UnityInputHandler _unityInputActions;
	private InputHandler _inputHandler;

	public static event Update OnCommonUpdate;

	void Awake()
	{
		_unityInputActions = new UnityInputHandler();
	}

	// Start is called before the first frame update
	void Start()
    {
		InputHandler _inputHandler = new InputHandler(_unityInputActions);
		WorldManager worldManager = new WorldManager(gameObject);

		//CharacterController characterController = new CharacterController(_inputHandler);

		worldManager.LoadDefaultWorld();
		worldManager.LoadWorld(WorldType.Gridspace);
		worldManager.LoadWorld(WorldType.Debug);
		WorldManager.FocusWorld(0);

	}

    // Update is called once per frame
    void Update()
    {
		
	}

	void FixedUpdate()
	{
		EngineUpdate();
	}

	public void EngineUpdate()
	{
		OnCommonUpdate?.Invoke();
	}
}
