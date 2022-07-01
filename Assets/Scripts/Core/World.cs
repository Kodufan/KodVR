using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	public Slot root;
	
    public void Close()
	{
		
	}

	public World() : this(WorldType.Default)
	{
	}

	// Constructor
	public World(WorldType type)
	{
		// Create the root object
		root = CreateSlot("Root");

		GameObject gameObject = new GameObject(type.ToString());
		gameObject.transform.parent = WorldManager.worldRoot.transform;
		root.SetParent(gameObject);

		// Create the world
		switch (type)
		{
			case WorldType.Default:
				Slot slot = CreateSlot("I am a slot in the default world!");
				Slot subSlot = CreateSlot("I am a child of the slot above!");
				Slot subSubSlot = CreateSlot("C");
				subSlot.SetParent(slot);
				slot.SetParent(root);
				subSubSlot.SetParent(subSlot);
				Slot subCousinSlot = CreateSlot("D");
				subCousinSlot.SetParent(subSlot);
				break;
			case WorldType.Space:
				break;
			case WorldType.Gridspace:
				break;
			case WorldType.Custom:
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

	public Slot CreateSlot(string name)
	{
		return new Slot(name, this);
	}
}
