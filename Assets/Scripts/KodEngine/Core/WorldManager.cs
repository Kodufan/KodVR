using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public delegate void OnWorldLoaded();

	public class WorldManager
	{
		// List of worlds
		public static GameObject worldRoot;
		public static World currentWorld;
		InputHandler _input = new InputHandler(new UnityInputHandler());
		public static int worldID;

		public static event OnWorldLoaded onWorldLoaded;

		public WorldManager(GameObject gameObject)
		{
			worldRoot = new GameObject("World root");
			worldRoot.transform.parent = gameObject.transform;
		}

		public static void CreateLocalHome()
		{
			CreateWorld(WorldType.Localhome, "localhome");
		}

		public static void CreateWorld(WorldType worldType, string worldID)
		{
			World world = null;
			if (worldID == "localhome")
			{
				world = new World(worldType, "localhome");

			} else
			{
				world = new World(worldType, worldType.ToString());
			}
			
			currentWorld = world;
		}

		public static void LoadWorld(string filePath)
		{
			World.Destroy();
			Engine.refTable.Clear();

			if (Engine.refTable.RefIDDictionary.Count != 0)
			{
				UnityEngine.Debug.LogError("World was not fully cleaned up!");
			}
			
			string json = System.IO.File.ReadAllText(filePath);

			RefTable refTable = Newtonsoft.Json.JsonConvert.DeserializeObject<RefTable>(json, new Newtonsoft.Json.JsonSerializerSettings() {
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
				TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			});

			Engine.refTable = refTable;

			string filename = "test2.json";
			json = Newtonsoft.Json.JsonConvert.SerializeObject(Engine.refTable, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings()
			{
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
				TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			});
			System.IO.File.WriteAllText(filename, json);

			byte[] file1 = System.IO.File.ReadAllBytes(@"C:\Users\koduf\Documents\GitHub\KodVR\Test.json");
			byte[] file2 = System.IO.File.ReadAllBytes(@"C:\Users\koduf\Documents\GitHub\KodVR\test2.json");
			if (file1.Length == file2.Length)
			{
				for (int i = 0; i < file1.Length; i++)
				{
					if (file1[i] != file2[i])
					{
						UnityEngine.Debug.LogError("JSON does not match!");
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogError("JSON does not match!");
			}

			RefID rootID = new RefID(1);

			Slot slot = (Slot)rootID.Resolve();
			currentWorld = new World(slot);

			UnityEngine.Debug.Log("Initializing WorldObjects...");

			onWorldLoaded?.Invoke();

			slot.RebalanceHeirarchy();

			//RefTable.RefIDDictionary.TryGetValue(new RefID(1, true), out WorldElement rootSlot);
			//World.root = rootSlot.refID;

			
		}

		// Will eventually need to duplicate a Unity Netcode manager and set it as a client
		public static void ConnectToWorld(string worldID, User user)
		{			
			if (currentWorld != null)
			{
				CreateWorld(WorldType.Default, worldID);
			}
		}
	}
}