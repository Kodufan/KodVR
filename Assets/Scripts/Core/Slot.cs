using System.Collections;
using System.Collections.Generic;
using System;
using KodEngine.Core;
using KodEngine.Component;

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

			if (this.parent != null)
			{
				this.gameObject.transform.SetParent(this.parent.gameObject.transform);
			}
		}

		public void SetParent(Slot parent)
		{
			if (parent != null && this.owningWorld == parent.owningWorld && this.parent != parent)
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

		public Slot CreateChild()
		{
			Slot newSlot =  new Slot(this.name + " - Child", "", UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, UnityEngine.Vector3.zero, this, this.owningWorld, true);
			return newSlot;
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
			Slot cube3 = new Slot("Cube3", owningWorld);
			cube3.SetParent(owningWorld.root);

			cube3.gameObject.transform.localPosition = new UnityEngine.Vector3(0, -1, 0);

			// When a cube is scaled to 0, the renderer will fail and it will turn black
			cube3.gameObject.transform.localScale = new UnityEngine.Vector3(10, 0.00001f, 10);

			Texture2D tex3 = cube3.AttachComponent<Texture2D>();
			tex3.uri = new System.Uri("C:\\Users\\koduf\\Desktop\\Memes\\718c6523d13d52ea0d5decf15988d119d2d24305a72b1e680f5acb24e943295d_1.png");

			PBS_Metallic material3 = cube3.AttachComponent<PBS_Metallic>();
			//material3.texture = tex3;

			ProceduralSphereMesh sphereMesh3 = cube3.AttachComponent<ProceduralSphereMesh>();
			ProceduralBoxMesh boxMesh3 = cube3.AttachComponent<ProceduralBoxMesh>();
			MeshRenderer renderer3 = cube3.AttachComponent<MeshRenderer>();
			//renderer3.material = material3;
			renderer3.mesh = sphereMesh3;

			renderer3.mesh = boxMesh3;

			material3.albedo = new KodEBase.Color(albedo);

			MeshCollider collider3 = cube3.AttachComponent<MeshCollider>();
			collider3.mesh = boxMesh3;
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