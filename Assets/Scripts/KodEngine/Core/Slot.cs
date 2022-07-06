using KodEngine.Component;
using KodEngine.KodEBase;
using System;
using System.Collections.Generic;

namespace KodEngine.Core
{
	public delegate void OnDestroy();
	public class Slot : IComparable
	{
		public string name;
		public string tag;
		private int orderOffset;

		[Newtonsoft.Json.JsonIgnore]
		private Float3 _position { get; set; }

		public Float3 position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				gameObject.transform.localPosition = value.unityVector3;
			}
		}

		[Newtonsoft.Json.JsonIgnore]
		private FloatQ _rotation { get; set; }

		public FloatQ rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				gameObject.transform.localRotation = value.unityQuaternion;
			}
		}

		[Newtonsoft.Json.JsonIgnore]
		private Float3 _scale { get; set; }

		public Float3 scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
				gameObject.transform.localScale = value.unityVector3;
			}
		}

		[Newtonsoft.Json.JsonIgnore]
		public Slot parent;
		
		public List<Slot> children;
		public List<Component> components;
		public bool isActive;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.GameObject gameObject;

		public event OnDestroy onDestroy;

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public Slot() : this("", "", Float3.zero, FloatQ.identity, Float3.one, null, true)
		{
		}

		public Slot(string name) : this(name, "", Float3.zero, FloatQ.identity, Float3.one, null, true)
		{
		}

		public Slot(string name, string tag, Float3 position, FloatQ rotation, Float3 scale, Slot parent, bool isActive)
		{
			// Must be created first for pos/rot/scale
			this.gameObject = new UnityEngine.GameObject(name);

			this.name = name;
			this.tag = tag;
			this.orderOffset = 0;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			this.parent = parent;	
			this.children = new List<Slot>();
			this.components = new List<Component>();
			this.isActive = true;
			
			if (this.parent != null)
			{
				this.gameObject.transform.SetParent(this.parent.gameObject.transform);
			}
		}

		public void SetParent(Slot parent)
		{
			if (parent != null && this.parent != parent)
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

		public void Destroy()
		{
			Destroy(this);
		}

		private void Destroy(Slot slot)
		{
			if (slot.parent != null)
			{
				UnityEngine.Debug.Log(slot.parent.children.IndexOf(slot));
				slot.parent.children.RemoveAt(slot.parent.children.IndexOf(slot));
			}

			onDestroy?.Invoke();

			slot.parent = null;
			UnityEngine.Object.Destroy(slot.gameObject);
		}

		public Slot CreateChild()
		{
			Slot newSlot =  new Slot(this.name + " - Child", "", Float3.zero, FloatQ.identity, Float3.one, this, true);
			newSlot.SetParent(this);
			children.Add(newSlot);
			children.Sort();
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

		public override string ToString()
		{
			return name;
		}
		
		public User TryGetUser()
		{
			foreach (User user in World.users)
			{
				if (TryGetParentSlot(user.userRoot) != null)
				{
					return user;
				}
			}
			return null;
		}

		public Slot TryGetParentSlot(Slot target)
		{
			
			Slot tempSlot = this;
			while (tempSlot != null)
			{
				if (tempSlot == target)
				{
					return tempSlot;
				}
				tempSlot = tempSlot.parent;
			}
			return null;
		}

		public void AttachPlane(UnityEngine.Color albedo)
		{
			Slot cube3 = new Slot("Floor");
			cube3.SetParent(World.root);
			
			cube3.position = new Float3(0, -1, 0);

			// When a cube is scaled to 0, the renderer will fail and it will turn black
			cube3.scale = new Float3(10, 0.00001f, 10);

			Texture2D tex3 = cube3.AttachComponent<Texture2D>();
			tex3.uri = new System.Uri("C:\\Users\\koduf\\Desktop\\Memes\\718c6523d13d52ea0d5decf15988d119d2d24305a72b1e680f5acb24e943295d_1.png");

			PBS_Metallic material3 = cube3.AttachComponent<PBS_Metallic>();
			//material3.texture = tex3;

			ProceduralSphereMesh sphereMesh3 = cube3.AttachComponent<ProceduralSphereMesh>();
			ProceduralBoxMesh boxMesh3 = cube3.AttachComponent<ProceduralBoxMesh>();
			MeshRenderer renderer3 = cube3.AttachComponent<MeshRenderer>();
			renderer3.material = material3;
			renderer3.mesh = sphereMesh3;

			renderer3.mesh = boxMesh3;

			material3.albedo = new Color(albedo);

			MeshCollider collider3 = cube3.AttachComponent<MeshCollider>();
			collider3.mesh = boxMesh3;
		}
		
		public T GetComponent<T>() where T : Component
		{
			Type type = typeof(T);
			
			foreach (Component component in components)
			{
				if (component.GetType() == type)
				{
					return (T) component;
				}
			}
			return null;
		}

		public T AttachComponent<T>() where T : Component, new()
		{
			// This is gross. Component should take a slot as a constructor and then call OnAttach when completed.
			T component = new T();
			onDestroy += component.OnDestroy;
			components.Add(component);
			component.owner = this;
			component.OnAttach();
			return component;
		}
	}
}