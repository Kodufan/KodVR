using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.Core;
using KodEngine.KodEBase;

namespace KodEngine
{
	public delegate void Update();

	public class Engine : MonoBehaviour
	{
		private UnityInputHandler _unityInputActions;
		public static InputHandler _inputHandler;
		public static Core.BuiltInMaterial builtInMaterial;

		public static event Update OnCommonUpdate;
		public static event Update OnEngineInit;

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

			_inputHandler.PrimaryInteractAction += Debug;


			WorldManager.CreateLocalHome();


			// This marks that the engine has completely initialized and will begin to call Unity methods to finish setup.
			EngineInit();

			KodEngine.NetKodE.NetworkManager.StartHost();
		}

		// Update is called once per frame
		void Update()
		{

		}

		void FixedUpdate()
		{
			EngineUpdate();
		}

		public void EngineInit()
		{
			OnEngineInit?.Invoke();
		}

		public void EngineUpdate()
		{
			OnCommonUpdate?.Invoke();
		}

		public void Debug2()
		{
			UnityEngine.Debug.Log("Loading world...");
			WorldManager.LoadWorld(@"C:\Users\koduf\Documents\GitHub\KodVR\Test.json");
		}
		
		public void Debug()
		{
			UnityEngine.Debug.Log("Serializing world...");
			World.SerializeWorld();
			Debug2();
		}
	}
}
