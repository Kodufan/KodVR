using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KodEngine : MonoBehaviour
{
	private UnityInputHandler _unityInputActions;
	private InputHandler _inputHandler;

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
		//if (Input.GetKeyDown(KeyCode.Alpha1))
		//{
		//	Debug.Log("Focusing world 1...");
		//	WorldManager.FocusWorld(0);
		//}
		//else if (Input.GetKeyDown(KeyCode.Alpha2))
		//{
		//	Debug.Log("Focusing on world 2...");
		//	WorldManager.FocusWorld(1);
		//}
		//else if (Input.GetKeyDown(KeyCode.Delete))
		//{
		//	Debug.Log("Deleting slots in world 1...");
		//	WorldManager.FocusWorld(0);
		//	WorldManager.focusedWorld.root.GetChild(0).destroy();
		//} else if (Input.GetKeyDown(KeyCode.C))
		//{
		//	Debug.Log("Creating new slot in world 1...");
		//	WorldManager.FocusWorld(0);
		//	WorldManager.focusedWorld.root.GetChild(0).CreateChild(WorldManager.focusedWorld.root);
		//}
	}
}
