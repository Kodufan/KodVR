using KodEngine.Component;
using KodEngine.KodEBase;
using System;
using System.Collections.Generic;

namespace KodEngine.Core
{
	public delegate void OnDestroy();
	public class Slot : WorldElement, IComparable
	{
		public string name;
		public string tag;
		private int orderOffset;

		public ValueField<Float3> position;

		public ValueField<FloatQ> rotation;

		public ValueField<Float3> scale;

		public ReferenceField<Slot> parent;

		public List<RefID> children;
		public List<RefID> components;

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

		public Slot(string name, string tag, Float3 position, FloatQ rotation, Float3 scale, RefID parent, bool isActive) : base()
		{
			// Must be created first for pos/rot/scale
			this.gameObject = new UnityEngine.GameObject(name);

			this.name = name;
			this.tag = tag;
			this.orderOffset = 0;
			this.position = new ValueField<Float3>(position);
			this.rotation = new ValueField<FloatQ>(rotation);
			this.scale = new ValueField<Float3>(scale);
			this.parent = new ReferenceField<Slot>(parent);
			this.children = new List<RefID>();
			this.components = new List<RefID>();
			this.isActive = true;

			if (this.parent.target != null)
			{
				this.gameObject.transform.SetParent((this.parent.Resolve()).gameObject.transform);
			}
		}

		public void SetParent(RefID parent)
		{
			if (parent != null && this.parent.target != parent)
			{
				Slot parentSlot = (Slot)parent.Resolve();

				if (this.parent.target != null)
				{
					List<RefID> children = ((Slot)this.parent.target.Resolve()).children;
					children.RemoveAt(children.IndexOf(this.refID));
				}

				this.parent.target = parent;
				parentSlot.children.Add(this.refID);
				this.gameObject.transform.SetParent(parentSlot.gameObject.transform);
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

		public Slot CreateChild()
		{
			Slot newSlot =  new Slot(this.name + " - Child", "", Float3.zero, FloatQ.identity, Float3.one, this.refID, true);
			newSlot.SetParent(this.refID);
			children.Add(newSlot.refID);
			return newSlot;
		}
		
		public Slot GetChild(int index)
		{
			return (Slot)children[index].Resolve();
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

		public Slot TryGetParentSlot(RefID target)
		{
			Slot tempSlot = this;
			while (tempSlot != null)
			{
				if (tempSlot == target.Resolve())
				{
					return tempSlot;
				}
				tempSlot = tempSlot.parent.Resolve();
			}
			return null;
		}

		public void AttachPlane(UnityEngine.Color albedo)
		{
			Slot cube3 = new Slot("Floor");
			cube3.SetParent(World.root);

			cube3.SetPosition(new Float3(0, -0.5f, 0));

			// When a cube is scaled to 0, the renderer will fail and it will turn black
			cube3.SetScale(new Float3(10, 0.0000f, 10));

			Texture2D tex3 = cube3.AttachComponent<Texture2D>();
			tex3.uri = new System.Uri(@"C:\Users\koduf\Downloads\white_cliff_top_8k.png");

			PBS_Metallic material3 = cube3.AttachComponent<PBS_Metallic>();
			material3.texture = tex3;

			ProceduralSphereMesh sphereMesh3 = cube3.AttachComponent<ProceduralSphereMesh>();
			ProceduralBoxMesh boxMesh3 = cube3.AttachComponent<ProceduralBoxMesh>();
			MeshRenderer renderer3 = cube3.AttachComponent<MeshRenderer>();
			material3.albedo = new Color(albedo);
			renderer3.material = material3;

			renderer3.mesh = boxMesh3;

			MeshCollider collider3 = cube3.AttachComponent<MeshCollider>();
		}
		
		public T GetComponent<T>() where T : Component
		{
			Type type = typeof(T);
			
			foreach (RefID component in components)
			{
				if (component.ResolveType() == type)
				{
					return (T)component.Resolve();
				}
			}
			return null;
		}

		public T AttachComponent<T>() where T : Component, new()
		{
			// This is gross. Component should take a slot as a constructor and then call OnAttach when completed.
			T component = new T();
			onDestroy += component.Destroy;
			components.Add(component.refID);
			component.owner = this.refID;
			component.OnAttach();
			return component;
		}

		public void SetPosition(Float3 value)
		{
			position.value = value;
			gameObject.transform.localPosition = value.unityVector3;
		}

		public void SetRotation(FloatQ value)
		{
			rotation.value = value;
			gameObject.transform.localRotation = value.unityQuaternion;
		}

		public void SetScale(Float3 value)
		{
			scale.value = value;
			gameObject.transform.localScale = value.unityVector3;
		}

		public override void OnDestroy()
		{
			position.Destroy();
			rotation.Destroy();
			scale.Destroy();

			UnityEngine.Object.Destroy(gameObject);
			onDestroy?.Invoke();

			for (int i = 0; i < children.Count; i++)
			{
				Slot child = (Slot)children[i].Resolve();
				child.Destroy();
				children.RemoveAt(i);
				i--;
			}
		}
	}
}