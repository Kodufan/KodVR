using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.Core;

namespace KodEngine
{
	public delegate void Update();

	public class Engine : MonoBehaviour
	{
		private UnityInputHandler _unityInputActions;
		public static InputHandler _inputHandler;
		public static Core.BuiltInMaterial builtInMaterial;

		public static event Update OnCommonUpdate;

		void Awake()
		{
			_unityInputActions = new UnityInputHandler();
			builtInMaterial = new Core.BuiltInMaterial();
		}

		// Start is called before the first frame update
		void Start()
		{
			_inputHandler = new InputHandler(_unityInputActions);
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
}
