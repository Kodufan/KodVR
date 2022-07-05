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

			_inputHandler.PrimaryInteractAction += Debug;
	
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

		public void Debug()
		{
			PlayerNetworkInstance instance = null;
			UnityEngine.Debug.Log("Left mouse button pressed");
			if (Unity.Netcode.NetworkManager.Singleton.IsHost)
			{
				foreach (ulong uid in Unity.Netcode.NetworkManager.Singleton.ConnectedClientsIds)
				{
					if (Unity.Netcode.NetworkManager.Singleton.LocalClient.ClientId == uid)
					{
						instance = Unity.Netcode.NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerNetworkInstance>();
						instance.SubmitPositionRequestServerRpc();
					}
				}
			}
		}
	}
}
