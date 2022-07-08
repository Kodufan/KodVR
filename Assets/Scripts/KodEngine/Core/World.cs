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
		
		public static RefID root;
		public string worldID;
		public static User hostUser;
		public static List<User> users;
		public static UnityEngine.GameObject worldObject;

		public void OnDisconnected(ulong uid)
		{
			foreach (User user in users)
			{
				if (user.unityNetworkID == uid)
				{
					((Slot)user.userRoot.Resolve()).Destroy();
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
			Slot rootSlot = new Slot("Root");

			// Preferably make a way to create dedicated reference IDs
			rootSlot.refID.id = 1;
			root = rootSlot.refID;

			// Create user list
			users = new List<User>();

			worldObject = new UnityEngine.GameObject(type.ToString());
			worldObject.transform.parent = WorldManager.worldRoot.transform;
			rootSlot.SetParent(worldObject);

			// Create the world
			switch (type)
			{
				case WorldType.Localhome:

					Slot cube = new Slot("Cube");
					cube.SetParent(rootSlot.refID);

					cube.SetPosition(new Float3(1, 0, -.75f));

					Texture2D tex = cube.AttachComponent<Texture2D>();
					tex.uri = new System.Uri(@"C:\Users\koduf\Downloads\unknown.png");

					PBS_Metallic material = cube.AttachComponent<PBS_Metallic>();
					material.SetTexture(tex.refID);

					ProceduralBoxMesh boxMesh = cube.AttachComponent<ProceduralBoxMesh>();
					ProceduralSphereMesh sphereMesh = cube.AttachComponent<ProceduralSphereMesh>();
					MeshRenderer renderer = cube.AttachComponent<MeshRenderer>();
					renderer.SetMaterial(material.refID);
					renderer.SetMesh(boxMesh.refID);

					material.SetColor(new Color(1, 1, 1, 1));

					MeshCollider collider = cube.AttachComponent<MeshCollider>();
					collider.SetMesh(boxMesh.refID);



					Slot cube2 = new Slot("Cube2");
					cube2.SetParent(rootSlot.refID);
					
					cube2.SetPosition(new Float3(1, 0, .75f));

					Texture2D tex2 = cube2.AttachComponent<Texture2D>();
					tex2.uri = new System.Uri(@"C:\Users\koduf\Downloads\kindpng_92984.png");

					PBS_Metallic material2 = cube2.AttachComponent<PBS_Metallic>();
					material2.SetTexture(tex2.refID);

					ProceduralBoxMesh boxMesh2 = cube2.AttachComponent<ProceduralBoxMesh>();
					MeshRenderer renderer2 = cube2.AttachComponent<MeshRenderer>();
					renderer2.SetMaterial(material2.refID);
					renderer2.SetMesh(boxMesh2.refID);

					material2.SetColor(new Color(1, 1, 1, 1));

					MeshCollider collider2 = cube2.AttachComponent<MeshCollider>();
					collider2.SetMesh(boxMesh2.refID);

					rootSlot.AttachPlane(new UnityEngine.Color(1, 1, 0, 1));
					break;
				case WorldType.Default:
					rootSlot.AttachPlane(UnityEngine.Color.grey);
					break;
				case WorldType.Space:
					worldID = "1";
					rootSlot.AttachPlane(UnityEngine.Color.black);
					break;
				case WorldType.Gridspace:
					worldID = "2";
					rootSlot.AttachPlane(UnityEngine.Color.white);
					break;
				case WorldType.Debug:
					worldID = "3";
					rootSlot.AttachPlane(UnityEngine.Color.blue);
					break;
				case WorldType.Custom:
					worldID = "4";
					break;
			}
		}

		public World(Slot rootSlot)
		{
			this.worldID = "Loaded world";

			// Preferably make a way to create dedicated reference IDs
			root = rootSlot.refID;
			
			// Create user list
			users = new List<User>();

			UnityEngine.GameObject gameObject = new UnityEngine.GameObject(worldID);
			gameObject.transform.parent = WorldManager.worldRoot.transform;
			rootSlot.SetParent(gameObject);
		}

		public static void Destroy()
		{
			UnityEngine.GameObject.Destroy(worldObject);
			((Slot)root.Resolve()).Destroy();
		}
		
		//world.AddUser("Username", "UserID", "MachineID", owningWorld, userRoot, networkInstance);
		public User BuildUser(User user)
		{
			user.userName = "Client";
			Slot userRoot = GetRoot().CreateChild();
			user.userRoot = userRoot.refID;
			userRoot.name = user.userName;
			users.Add(user);

			User.userInitialized += ConstructVisual;
			return user;
		}

		public User BuildHostUser(User user)
		{
			user.userName = "Host";
			Slot userRoot = GetRoot().CreateChild();
			userRoot.name = user.userName;
			user.userRoot = userRoot.refID;

			userRoot.AttachComponent<CharacterController>();
			users.Add(user);
			return user;
		}

		public void ConstructVisual(User user)
		{
			((Slot)user.userRoot.Resolve()).AttachComponent<PlayerVisual>();
		}
	
		public static void SerializeWorld()
		{
			string fileName = "Test.json";

			//List<WorldElement> worldElements = new List<WorldElement>(RefTable.RefIDDictionary.Values);

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(RefTable.RefIDDictionary, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings()
			{
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
				TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			});
			System.IO.File.WriteAllText(fileName, json);
			byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
		}

		public static Slot GetRoot()
		{
			return (Slot)root.Resolve();
		}
	}
}