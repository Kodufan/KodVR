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
		public ValueField<Bool> isActive;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.GameObject gameObject;

		public event OnDestroy onDestroy;


		[Newtonsoft.Json.JsonConstructor]
		public Slot(string name, string tag, ValueField<Float3> position, ValueField<FloatQ> rotation, ValueField<Float3> scale, ReferenceField<Slot> parent, List<RefID> children, List<RefID> components, ValueField<Bool> isActive, RefID refID)
		{
			this.gameObject = new UnityEngine.GameObject(name);

			this.name = name;
			this.tag = tag;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			this.parent = parent;
			this.children = children;
			this.components = components;
			this.isActive = isActive;
			this.refID = refID;

			if (this.parent.target != null)
			{
				this.SetParent(this.parent.target);
			}
		}

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public Slot() : this("", "", Float3.zero, FloatQ.identity, Float3.one, null, new Bool(true))
		{
		}
		
		public Slot(string name) : this(name, "", Float3.zero, FloatQ.identity, Float3.one, null, new Bool(true))
		{
		}

		public Slot(string name, string tag, Float3 position, FloatQ rotation, Float3 scale, Bool isActive) : this(name, tag, position, rotation, scale, null, isActive)
		{
		}

		public Slot(string name, string tag, Float3 position, FloatQ rotation, Float3 scale, RefID parent, Bool isActive) : base()
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
			this.isActive = new ValueField<Bool>(isActive);

			if (this.parent.target != null)
			{
				this.SetParent(this.parent.target);
			}
		}

		public void SetParent(RefID parent)
		{
			if (Engine.refTable.RefIDDictionary.ContainsKey(parent) && parent.ResolveType() == typeof(Slot))
			{
				Slot parentSlot = (Slot)parent.Resolve();

				if (this.parent.target != null)
				{
					List<RefID> children = ((Slot)this.parent.target.Resolve()).children;
					children.Remove(this.refID);
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
				this.parent.target = null;
				this.gameObject.transform.SetParent(parent.gameObject.transform);
			}
		}

		public Slot CreateChild()
		{
			Slot newSlot =  new Slot(this.name + " - Child", "", Float3.zero, FloatQ.identity, Float3.one, null, new Bool(true));
			newSlot.SetParent(this.refID);
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
			cube3.SetScale(new Float3(10, 0.00001f, 10));

			Texture2D tex3 = cube3.AttachComponent<Texture2D>();
			tex3.uri = new System.Uri(@"C:\Program Files (x86)\Steam\userdata\207376680\760\remote\740250\screenshots\20220630194839_1.jpg");

			PBS_Metallic material3 = cube3.AttachComponent<PBS_Metallic>();
			material3.SetTexture(tex3.refID);

			ProceduralBoxMesh boxMesh3 = cube3.AttachComponent<ProceduralBoxMesh>();
			MeshRenderer renderer3 = cube3.AttachComponent<MeshRenderer>();
			
			material3.SetColor(new Color(albedo));

			renderer3.SetMaterial(material3.refID);
			renderer3.SetMesh(boxMesh3.refID);

			MeshCollider collider3 = cube3.AttachComponent<MeshCollider>();
			collider3.SetMesh(boxMesh3.refID);
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

		public T AttachComponent<T>() where T : Component
		{
			// This is gross. Component should take a slot as a constructor and then call OnAttach when completed.

			// Hi past me. Fixed that for ya
			T component = (T)Activator.CreateInstance(typeof(T), new object[] { this.refID });
			onDestroy += component.Destroy;
			components.Add(component.refID);
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
		
		public void RebalanceHeirarchy()
		{
			foreach (RefID child in children)
			{
				// This is very very very bad. Replace this with a dictionary that looks up values based on the ulong ID and not refID memory address.
				RefID childID = null;
				foreach (KeyValuePair<RefID, WorldElement> e in Engine.refTable.RefIDDictionary)
				{
					if (e.Key.id == child.id)
					{
						childID = e.Key;
					}
				}
				Slot childSlot = (Slot)childID.Resolve();
				childSlot.RebalanceHeirarchy();
			}
			if (parent.target != null)
			{
				this.SetGameObjectParent(parent.target);
			}
		}

		public void SetGameObjectParent(RefID target) 
		{
			if (this.parent.target == target)
			{
				this.gameObject.transform.SetParent(((Slot)target.Resolve()).gameObject.transform);
			}
		}

		public override void OnDestroy()
		{
			// May be better to find a way to bind valuefields to the thing they're attached to and have them call their own destroy with an action.
			position.Destroy();
			rotation.Destroy();
			scale.Destroy();
			parent.Destroy();
			isActive.Destroy();

			UnityEngine.Object.Destroy(gameObject);
			onDestroy?.Invoke();

			while (children.Count > 0)
			{
				Slot child = (Slot)children[0].Resolve();
				if (child != null)
				{
					child.Destroy();
				} else
				{
					UnityEngine.Debug.Log(this.refID + ", " + children[0]);
				}
				children.RemoveAt(0);
			}
		}
	}
}