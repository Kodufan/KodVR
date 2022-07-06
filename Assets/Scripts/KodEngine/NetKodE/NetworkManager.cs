using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KodEngine
{
	public delegate void Action(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response);

	public class NetworkManager : MonoBehaviour
	{
		public static event Action OnPlayerConnection;

		// Janky solution to making sure the host binds connection events as well
		public static ulong hostID;

		public static void OnConnection(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response)
		{

			Core.User user = new Core.User("Username", "UserID", "MachineID", request.ClientNetworkId);

			response.PlayerPrefabHash = null;
			response.CreatePlayerObject = true;

			if (System.Text.ASCIIEncoding.Default.GetString(request.Payload) == "init")
			{
				response.Approved = true;
				KodEngine.Core.WorldManager.currentWorld.BuildHostUser(user);
				return;
			}
			
			// Eventually perform authentication check with API, verifying user information. Skip this if hosted locally or is host.
			bool isUserAuthenticated = true;

			if (isUserAuthenticated)
			{
				response.Approved = true;
				Debug.Log("Building client!");
				KodEngine.Core.WorldManager.currentWorld.BuildUser(user);
			}
		}

		private void Start()
		{
			Engine.OnEngineInit += OnInit;
			
			Debug.Log("Network Engine initializing");
			Unity.Netcode.NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("init");
			Unity.Netcode.NetworkManager.Singleton.ConnectionApprovalCallback += OnConnection;
		}

		public void OnInit()
		{
			
		}


		public static void Change()
		{
			
		}
		
		void OnGUI()
		{
			GUILayout.BeginArea(new Rect(10, 10, 300, 300));
				StartButtons();
				StatusLabels();


			GUILayout.EndArea();
		}

		static void StartButtons()
		{
			if (GUILayout.Button("Client"))
			{
				Unity.Netcode.NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("1");
				Unity.Netcode.NetworkManager.Singleton.StartClient();
				Debug.Log("Client started!");
			}
			if (GUILayout.Button("Host")) 
			{ 
				Unity.Netcode.NetworkManager.Singleton.StartHost();
				Debug.Log("Host started!");
				hostID = Unity.Netcode.NetworkManager.Singleton.LocalClient.ClientId;
				Engine._inputHandler.PrimaryInteractAction += NetworkManager.Change;
				Unity.Netcode.NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("");
			}
		}

		static void StatusLabels()
		{
			var mode = Unity.Netcode.NetworkManager.Singleton.IsHost ?
				"Host" : Unity.Netcode.NetworkManager.Singleton.IsServer ? "Server" : "Client";

			GUILayout.Label("Transport: " +
				Unity.Netcode.NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
			GUILayout.Label("Mode: " + mode);
		}
	}
}