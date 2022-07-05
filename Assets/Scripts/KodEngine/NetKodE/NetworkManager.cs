using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KodEngine
{
	public delegate void Action(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response);

	public class NetworkManager : MonoBehaviour
	{
		public static event Action OnPlayerConnection;

		public static void OnConnection(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response)
		{
			OnPlayerConnection?.Invoke(request, response);
		}

		private void Start()
		{
			Engine._inputHandler.PrimaryInteractAction += Change;
		}


		public void Change()
		{
			
		}
		
		void OnGUI()
		{
			GUILayout.BeginArea(new Rect(10, 10, 300, 300));
			if (!Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsServer)
			{
				StartButtons();
			}
			else
			{
				StatusLabels();

				SubmitNewPosition();
			}

			GUILayout.EndArea();
		}

		static void StartButtons()
		{
			if (GUILayout.Button("Host"))
			{
				Unity.Netcode.NetworkManager.Singleton.ConnectionApprovalCallback += OnConnection;
				Unity.Netcode.NetworkManager.Singleton.StartHost();
			}
			if (GUILayout.Button("Client"))
			{
				Unity.Netcode.NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("1");
				Unity.Netcode.NetworkManager.Singleton.StartClient();
			}
			if (GUILayout.Button("Server")) Unity.Netcode.NetworkManager.Singleton.StartServer();
		}

		static void StatusLabels()
		{
			var mode = Unity.Netcode.NetworkManager.Singleton.IsHost ?
				"Host" : Unity.Netcode.NetworkManager.Singleton.IsServer ? "Server" : "Client";

			GUILayout.Label("Transport: " +
				Unity.Netcode.NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
			GUILayout.Label("Mode: " + mode);
		}

		static void SubmitNewPosition()
		{
			if (GUILayout.Button(Unity.Netcode.NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
			{
				if (Unity.Netcode.NetworkManager.Singleton.IsServer && !Unity.Netcode.NetworkManager.Singleton.IsClient)
				{
					foreach (ulong uid in Unity.Netcode.NetworkManager.Singleton.ConnectedClientsIds)
						Unity.Netcode.NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerNetworkInstance>();
				}
				else
				{
					var playerObject = Unity.Netcode.NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
					var player = playerObject.GetComponent<PlayerNetworkInstance>();
					//player.Move();
				}
			}
		}
	}
}