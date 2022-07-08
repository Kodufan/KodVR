using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public class WorldManager
	{
		// List of worlds
		public static GameObject worldRoot;
		public static World currentWorld;
		InputHandler _input = new InputHandler(new UnityInputHandler());
		public static int worldID;

		public WorldManager(GameObject gameObject)
		{
			worldRoot = new GameObject("World root");
			worldRoot.transform.parent = gameObject.transform;
			//_input.ToggleSessionEvent += ToggleSession;
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
			KodEBase.RefID.ResetID();
			
			foreach (KeyValuePair<RefID, WorldElement> e in RefTable.RefIDDictionary)
			{
				UnityEngine.Debug.Log(e.Key + ": " + e.Value);
			}

			string json = System.IO.File.ReadAllText(filePath);
			

			Dictionary<RefID, WorldElement> refTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<RefID, WorldElement>>(json, new Newtonsoft.Json.JsonSerializerSettings() {
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			});
			//World.root = deserializedProduct.refID;

			//string fileName = "Test2.json";
			//json = Newtonsoft.Json.JsonConvert.SerializeObject(World.root, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings()
			//{
			//TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
			//	TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple
			//});
			//System.IO.File.WriteAllText(fileName, json);
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