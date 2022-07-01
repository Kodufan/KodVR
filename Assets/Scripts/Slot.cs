using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Slot : IComparable
{
	public string name;
	public string tag;
	public int orderOffset;
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public Slot parent;
	public List<Slot> children;
	public bool isActive;
	public GameObject gameObject;

	// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
	public Slot() : this("", "", Vector3.zero, Quaternion.identity, Vector3.one, null, true)
	{
	}

	public Slot(string name) : this(name, "", Vector3.zero, Quaternion.identity, Vector3.one, WorldManager.focusedWorld.root, true)
	{
	}

	public Slot(string name, string tag, Vector3 position, Quaternion rotation, Vector3 scale, Slot parent, bool isActive)
	{
		this.name = name;
		this.tag = tag;
		this.orderOffset = 0;
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
		this.parent = parent;
		this.children = new List<Slot>();
		this.isActive = true;
		this.gameObject = new GameObject(name);
		Debug.Log("Initialized!");
	}

	public void SetParent(Slot parent)
	{
		if (parent != null)
		{
			if (this.parent != null)
			{
				List<Slot> children = this.parent.children;
				children.RemoveAt(children.IndexOf(this));
			}
			
			this.parent = parent;
			parent.children.Add(this);
			this.gameObject.transform.SetParent(parent.gameObject.transform);
		}
	}

	public void SetParent(GameObject parent)
	{
		if (parent != null)
		{
			this.parent = null;
			this.gameObject.transform.SetParent(parent.gameObject.transform);
		}
	}

	public void destroy()
	{
		destroy(this);
	}

	private void destroy(Slot slot)
	{
		Debug.Log("Destroying " + name);
		if (children.Count == 0)
		{
			this.parent = null;
			UnityEngine.Object.Destroy(gameObject);
			Debug.Log("Destroy was called");
		}
		foreach (Slot child in slot.children) 
		{
			destroy(child);
		}
	}

	public Slot GetChild(int index)
	{
		Debug.Log("Get child was ran with index: " + index);
		return children[index];
	}

	public int CompareTo(object obj)
	{
		if (obj.GetType() == typeof(object))
		{
			Slot comparer = (Slot)obj;
			return this.orderOffset - comparer.orderOffset;
		}
		
		throw new NotImplementedException();
	}
}
