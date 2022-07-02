using System.Collections;
using System.Collections.Generic;
using System;
using KodEngine.Core;

namespace KodEngine.Core
{
	public class Slot : IComparable
	{
		public string name;
		public string tag;
		private int orderOffset;
		public UnityEngine.Vector3 position;
		public UnityEngine.Quaternion rotation;
		public UnityEngine.Vector3 scale;
		public World owningWorld;
		public Slot parent;
		public List<Slot> children;
		public List<Component> components;
		public bool isActive;
		public UnityEngine.GameObject gameObject;

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public Slot(World owningWorld) : this("", "", UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, UnityEngine.Vector3.one, null, owningWorld, true)
		{
		}

		public Slot(string name, World owningWorld) : this(name, "", UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, UnityEngine.Vector3.one, null, owningWorld, true)
		{
		}

		public Slot(string name, string tag, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, UnityEngine.Vector3 scale, Slot parent, World owningWorld, bool isActive)
		{
			this.name = name;
			this.tag = tag;
			this.orderOffset = 0;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			this.owningWorld = owningWorld;
			this.parent = parent;
			this.children = new List<Slot>();
			this.components = new List<Component>();
			this.isActive = true;
			this.gameObject = new UnityEngine.GameObject(name);
		}

		public void SetParent(Slot parent)
		{
			if (parent != null && this.owningWorld == parent.owningWorld)
			{
				if (this.parent != null)
				{
					List<Slot> children = this.parent.children;
					children.RemoveAt(children.IndexOf(this));
				}

				this.parent = parent;
				parent.children.Add(this);
				parent.children.Sort();
				this.gameObject.transform.SetParent(parent.gameObject.transform);
			}
		}

		public void SetParent(UnityEngine.GameObject parent)
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
			if (slot.parent != null)
			{
				slot.parent.children.RemoveAt(slot.parent.children.IndexOf(slot));
			}

			slot.parent = null;
			UnityEngine.Object.Destroy(slot.gameObject);
			UnityEngine.Debug.Log("Destroying slot: " + slot);

		}

		public Slot CreateChild(Slot slot)
		{
			return new Slot(slot.name + " - Child", "", UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, UnityEngine.Vector3.zero, slot, this.owningWorld, true); ;
		}

		public Slot GetChild(int index)
		{
			return children[index];
		}

		public int CompareTo(object obj)
		{
			if (obj.GetType() == typeof(Slot))
			{
				Slot comparer = (Slot)obj;
				return this.orderOffset - comparer.orderOffset;
			}

			throw new ArgumentException();
		}

		public int GetOrderOffset()
		{
			return orderOffset;
		}

		// Setter for orderOffset
		// Sort could use binary tactics for efficiency down the line
		public void SetOrderOffset(int orderOffset)
		{
			this.orderOffset = orderOffset;
			if (parent != null)
			{
				parent.children.Sort();
			}
		}

		override
		public string ToString()
		{
			return name;
		}

		public void AttachPlane(UnityEngine.Color albedo)
		{
			UnityEngine.GameObject plane = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane);
			UnityEngine.Material material = new UnityEngine.Material(UnityEngine.Shader.Find("Specular"));
			UnityEditor.Presets.Preset preset = new UnityEditor.Presets.Preset(UnityEngine.Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic"));
			preset.ApplyTo(material);

			UnityEngine.MeshRenderer renderer = plane.GetComponent<UnityEngine.MeshRenderer>();
			plane.transform.position = this.position;
			plane.transform.rotation = this.rotation;
			plane.transform.localScale = this.scale;
			plane.transform.SetParent(this.gameObject.transform);
			material.color = albedo;
			renderer.material = material;
		}

		public T AttachComponent<T>() where T : Component, new()
		{
			// This is gross. Component should take a slot as a constructor and then call OnAttach when completed.
			T component = new T();
			component.owner = this;
			component.OnAttach();
			return component;
		}
	}
}