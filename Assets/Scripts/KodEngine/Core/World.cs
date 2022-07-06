using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.Core;
using KodEngine.KodEBase;

using Newtonsoft.Json.Linq;

namespace KodEngine.Core
{
	public delegate void Focus();

	public class World
	{
		public Unity.Netcode.NetworkVariable<UnityEngine.Vector3> Position = new Unity.Netcode.NetworkVariable<UnityEngine.Vector3>();
		public static Slot root;
		public string worldID;
		public static User hostUser;
		public static List<User> users;
		public Dictionary<RefID, IWorldElement> RefIDDictionary = new Dictionary<RefID, IWorldElement>();

		public void OnDisconnected(ulong uid)
		{
			foreach (User user in users)
			{
				if (user.unityNetworkID == uid)
				{
					user.userRoot.Destroy();
					users.Remove(user);
					break;
				}
			}
		}

		public void Close()
		{

		}

		public World(string worldID) : this(WorldType.Default, worldID)
		{
			this.worldID = worldID;
		}

		// Constructor
		public World(WorldType type, string worldID)
		{
			this.worldID = worldID;
			// Create the world root
			root = CreateSlot("Root");

			// Create user list
			users = new List<User>();

			UnityEngine.GameObject gameObject = new UnityEngine.GameObject(type.ToString());
			gameObject.transform.parent = WorldManager.worldRoot.transform;
			root.SetParent(gameObject);

			// Create the world
			switch (type)
			{
				case WorldType.Localhome:

					Slot cube = new Slot("Cube");
					cube.SetParent(root);

					cube.position = new Float3(1, 0, -.75f);

					Texture2D tex = cube.AttachComponent<Texture2D>();
					tex.uri = new System.Uri("C:\\Users\\Jack Duvall\\Downloads\\hug.png");

					PBS_Metallic material = cube.AttachComponent<PBS_Metallic>();
					material.texture = tex;

					UnityEngine.Debug.Log(material.componentName);

					ProceduralBoxMesh boxMesh = cube.AttachComponent<ProceduralBoxMesh>();
					ProceduralSphereMesh sphereMesh = cube.AttachComponent<ProceduralSphereMesh>();
					MeshRenderer renderer = cube.AttachComponent<MeshRenderer>();
					renderer.material = material;

					renderer.mesh = sphereMesh;

					material.albedo = new Color(1, 1, 1, 1);

					MeshCollider collider = cube.AttachComponent<MeshCollider>();
					collider.mesh = boxMesh;



					Slot cube2 = new Slot("Cube2");
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

					root.AttachPlane(new UnityEngine.Color(1, 1, 0, 1));
					root.AttachComponent<CharacterController>();
					break;
				case WorldType.Default:
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
		}

		public void Destroy()
		{
			root.Destroy();
		}

		public Slot CreateSlot(string name)
		{
			return new Slot(name);
		}
		
		//world.AddUser("Username", "UserID", "MachineID", owningWorld, userRoot, networkInstance);
		public User BuildUser(User user)
		{
			user.userName = "Client";
			Slot userRoot = root.CreateChild();
			user.userRoot = userRoot;
			userRoot.name = user.userName;
			users.Add(user);

			User.userInitialized += ConstructVisual;



			return user;
		}

		public User BuildHostUser(User user)
		{
			user.userName = "Host";
			Slot userRoot = root.CreateChild();
			userRoot.name = user.userName;
			user.userRoot = userRoot;
			
			users.Add(user);
			return user;
		}

		public void ConstructVisual(User user)
		{
			user.userRoot.AttachComponent<PlayerVisual>();
		}
	
		public static void SerializeWorld()
		{
			string fileName = "Test.json";
			string json = Newtonsoft.Json.JsonConvert.SerializeObject(root, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings()
			{
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
				TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			});
			System.IO.File.WriteAllText(fileName, json);
			byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
		}
	}
}