using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.Core;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public delegate void Focus();

	public class World
	{
		public Unity.Netcode.NetworkVariable<UnityEngine.Vector3> Position = new Unity.Netcode.NetworkVariable<UnityEngine.Vector3>();
		public Slot root;
		public string worldID;
		public List<User> users;
		public event Focus OnFocusGained;
		public event Focus OnFocusLost;

		public void OnConnection(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response)
		{
			//UnityEngine.Debug.Log(System.Text.Encoding.ASCII.GetString(request.Payload));
			
			if (System.Text.Encoding.ASCII.GetString(request.Payload) == worldID || Unity.Netcode.NetworkManager.Singleton.IsHost)
			{
				response.Approved = true;
				UnityEngine.Debug.Log("Building Player...");
				Slot playerSlot = root.CreateChild();
				User user = new User("UserName", "UserID", "MachineID", WorldManager.GetWorldFromID(worldID));
				user.userRoot = playerSlot;
				users.Add(user);

				playerSlot.name = "Player";
				

				playerSlot.AttachComponent<PlayerVisual>();
				
			} else
			{
				//UnityEngine.Debug.Log("Player rejected");
			}
		}

		public void Close()
		{

		}

		public World(string worldID) : this(WorldType.Default, worldID)
		{
		}

		// Constructor
		public World(WorldType type, string worldID)
		{
			this.worldID = worldID;
			// Create the world root
			root = CreateSlot("Root");

			// Create user list
			users = new List<User>();

			// Create the world network listener object
			KodEngine.NetworkManager.OnPlayerConnection += OnConnection;

			UnityEngine.GameObject gameObject = new UnityEngine.GameObject(type.ToString());
			gameObject.transform.parent = WorldManager.worldRoot.transform;
			root.SetParent(gameObject);

			// Create the world
			switch (type)
			{
				case WorldType.Default:
					worldID = "0";
					Slot cube = new Slot("Cube", this);
					cube.SetParent(root);

					cube.position = new Float3(1, 0, -.75f);

					Texture2D tex = cube.AttachComponent<Texture2D>();
					tex.uri = new System.Uri("C:\\Users\\Jack Duvall\\Downloads\\hug.png");

					PBS_Metallic material = cube.AttachComponent<PBS_Metallic>();
					material.texture = tex;

					ProceduralBoxMesh boxMesh = cube.AttachComponent<ProceduralBoxMesh>();
					ProceduralSphereMesh sphereMesh = cube.AttachComponent<ProceduralSphereMesh>();
					MeshRenderer renderer = cube.AttachComponent<MeshRenderer>();
					renderer.material = material;

					renderer.mesh = sphereMesh;

					material.albedo = new Color(1, 1, 1, 1);

					MeshCollider collider = cube.AttachComponent<MeshCollider>();
					collider.mesh = boxMesh;


					
					Slot cube2 = new Slot("Cube2", this);
					cube2.SetParent(root);

					cube2.position = new Float3(1, 0, .75f);

					Texture2D tex2 = cube2.AttachComponent<Texture2D>();
					tex2.uri = new System.Uri("C:\\Users\\Jack Duvall\\Downloads\\drag.png");

					PBS_Metallic material2 = cube2.AttachComponent<PBS_Metallic>();
					material2.texture = tex2;

					ProceduralBoxMesh boxMesh2 = cube2.AttachComponent<ProceduralBoxMesh>();
					MeshRenderer renderer2 = cube2.AttachComponent<MeshRenderer>();
					renderer2.material = material2;

					renderer2.mesh = boxMesh2;

					material2.albedo = new Color(1, 1, 1, 1);

					MeshCollider collider2 = cube2.AttachComponent<MeshCollider>();
					collider2.mesh = boxMesh2;



					root.AttachPlane(UnityEngine.Color.grey);
					break;
				case WorldType.Space:
					worldID = "1";
					root.AttachPlane(UnityEngine.Color.black);
					break;
				case WorldType.Gridspace:
					worldID = "2";
					root.AttachPlane(UnityEngine.Color.white);
					break;
				case WorldType.Debug:
					worldID = "3";
					root.AttachPlane(UnityEngine.Color.blue);
					break;
				case WorldType.Custom:
					worldID = "4";
					root.AttachPlane(UnityEngine.Color.red);
					break;
			}
			root.AttachComponent<CharacterController>();
		}

		public void Focus()
		{
			root.gameObject.SetActive(true);
			OnFocusGained?.Invoke();
		}

		public void Unfocus()
		{
			root.gameObject.SetActive(false);
			OnFocusLost?.Invoke();
		}

		public Slot CreateSlot(string name)
		{
			return new Slot(name, this);
		}
		
		//world.AddUser("Username", "UserID", "MachineID", owningWorld, userRoot, networkInstance);
		public User AddUser(string userName, string userID, string machineID, PlayerNetworkInstance networkInstance)
		{
			User user = new User(userName, userID, machineID, this);
			users.Add(user);
			return user;
		}
	}

}