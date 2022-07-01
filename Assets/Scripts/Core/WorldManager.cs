using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
	// List of worlds
	public static List<World> worlds = new List<World>();
	public static World focusedWorld;
	public static int focusedWorldIndex;
	public static GameObject worldRoot;

	public WorldManager(GameObject gameObject)
	{
		worldRoot = new GameObject("World root");
		focusedWorldIndex = -1;
		worldRoot.transform.parent = gameObject.transform;
	}

	public World LoadDefaultWorld()
	{
		return LoadWorld(WorldType.Default);
	}

	public World LoadWorld(WorldType worldType)
	{
		World world = new World(worldType);
		worlds.Add(world);
		FocusWorld(world);
		return world;
	}

	public static void FocusWorld(int index)
	{
		if (index == focusedWorldIndex)
		{
			Debug.LogError("World already focused!");
			return;
		}
		if (index < 0 || index >= worlds.Count)
		{
			Debug.LogError("Invalid world index: " + index);
			return;
		}
		
		if (focusedWorldIndex != -1)
		{
			worlds[focusedWorldIndex].Unfocus();
		}
		focusedWorldIndex = index;
		focusedWorld = worlds[focusedWorldIndex];
		focusedWorld.Focus();
	}

	public void FocusWorld(World world)
	{
		if (worlds.Contains(world))
		{
			FocusWorld(worlds.IndexOf(world));
		}
		else
		{
			Debug.LogError("World not found: " + world);
		}
	}

    public void ExitWorld(World world)
	{
		world.Close();
		worlds.Remove(world);
	}
}
