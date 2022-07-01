using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	public Slot root;
	
    public void Close()
	{
		
	}

	// Constructor
	public World(WorldTypes type)
	{
		// Create the root object
		root = new Slot("Root");

		GameObject gameObject = new GameObject(type.ToString());
		gameObject.transform.parent = WorldManager.worldRoot.transform;
		root.SetParent(gameObject);

		// Create the world
		switch (type)
		{
			case WorldTypes.Default:
				Slot slot = new Slot("I am a slot in the Default world!");
				Slot subSlot = new Slot("I am a child of the slot above!");
				subSlot.SetParent(slot);
				slot.SetParent(root);
				break;
			case WorldTypes.Space:
				break;
			case WorldTypes.Gridspace:
				break;
			case WorldTypes.Custom:
				break;
		}
	}

	public void Focus()
	{
		root.gameObject.SetActive(true);
	}

	public void Unfocus()
	{
		root.gameObject.SetActive(false);
	}
}
