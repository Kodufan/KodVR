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

		public RefID positionField;
		[Newtonsoft.Json.JsonIgnore]
		private ValueField<Float3> _position;
		
		public RefID rotationField;
		[Newtonsoft.Json.JsonIgnore]
		private ValueField<FloatQ> _rotation;
		
		public RefID scaleField;
		[Newtonsoft.Json.JsonIgnore]
		private ValueField<Float3> _scale;
		
		public RefID parentField;
		[Newtonsoft.Json.JsonIgnore]
		private ReferenceField<Slot> _parent;
		
		public RefID isActiveField;
		[Newtonsoft.Json.JsonIgnore]
		private ValueField<Bool> _isActive;
		
		public List<RefID> children;
		public List<RefID> components;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.GameObject gameObject;

		public event OnDestroy onDestroy;


		[Newtonsoft.Json.JsonConstructor]
		public Slot(string name, string tag, RefID positionField, RefID rotationField, RefID scaleField, RefID parentField, List<RefID> children, List<RefID> components, RefID isActiveField, RefID refID) : base()
		{
			this.gameObject = new UnityEngine.GameObject(name);

			this.name = name;
			this.tag = tag;
			this.positionField = positionField;
			this.rotationField = rotationField;
			this.scaleField = scaleField;
			this.parentField = parentField;
			this.isActiveField = isActiveField;
			this.children = children;
			this.components = components;
			this.refID = refID;
			Engine.refTable.RefIDDictionary.Add(refID, this);
			WorldManager.onWorldLoaded += OnInit;
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
		
		public Slot(string name, string tag, Float3 position, FloatQ rotation, Float3 scale, RefID parent, Bool isActive) : base(true)
		{
			// Must be created first for pos/rot/scale
			this.gameObject = new UnityEngine.GameObject(name);

			this.name = name;
			this.tag = tag;
			this.orderOffset = 0;
			
			this.positionField = new ValueField<Float3>(position).refID;
			_position = positionField.Resolve() as ValueField<Float3>;
			
			this.rotationField = new ValueField<FloatQ>(rotation).refID;
			_rotation = rotationField.Resolve() as ValueField<FloatQ>;
			
			this.scaleField = new ValueField<Float3>(scale).refID;
			_scale = scaleField.Resolve() as ValueField<Float3>;
			
			this.parentField = new ReferenceField<Slot>(parent).refID;
			_parent = parentField.Resolve() as ReferenceField<Slot>;
			
			this.isActiveField = new ValueField<Bool>(isActive).refID;
			_isActive = isActiveField.Resolve() as ValueField<Bool>;
			
			this.children = new List<RefID>();
			this.components = new List<RefID>();

			if (_parent.target != null)
			{
				this.SetParent(_parent.target);
			}
		}

		public void SetParent(RefID parent)
		{
			if (Engine.refTable.RefIDDictionary.ContainsKey(parent) && parent.ResolveType() == typeof(Slot))
			{
				Slot parentSlot = (Slot)parent.Resolve();

				if (_parent.target != null)
				{
					List<RefID> children = ((Slot)_parent.target.Resolve()).children;
					children.Remove(this.refID);
				}

				_parent.target = parent;
				parentSlot.children.Add(this.refID);
				this.gameObject.transform.SetParent(parentSlot.gameObject.transform);
			}
		}

		public void SetParent(UnityEngine.GameObject parent)
		{
			if (parent != null)
			{
				//_parent.target = null;
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
				tempSlot = ((ReferenceField<Slot>)tempSlot.parentField.Resolve()).target.Resolve() as Slot;
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
			this.onDestroy += component.Destroy;
			components.Add(component.refID);
			return component;
		}

		public void SetPosition(Float3 value)
		{
			_position.value = value;
			gameObject.transform.localPosition = value.unityVector3;
		}

		public void SetRotation(FloatQ value)
		{
			_rotation.value = value;
			gameObject.transform.localRotation = value.unityQuaternion;
		}

		public void SetScale(Float3 value)
		{
			_scale.value = value;
			gameObject.transform.localScale = value.unityVector3;
		}
		
		public void RebalanceHeirarchy()
		{
			foreach (RefID child in children)
			{
				// This is very very very bad. Replace this with a dictionary that looks up values based on the ulong ID and not refID memory address.

				// I gotchu fam
				
				Slot childSlot = (Slot)(new RefID(child.id, true)).Resolve();
				childSlot.RebalanceHeirarchy();
			}
			if (_parent.target != null)
			{
				this.SetGameObjectParent(_parent.target);
			}
		}

		public void SetGameObjectParent(RefID target) 
		{
			if (_parent.target == target)
			{
				this.gameObject.transform.SetParent(((Slot)target.Resolve()).gameObject.transform);
			}
		}

		public override void OnDestroy()
		{
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

			// May be better to find a way to bind valuefields to the thing they're attached to and have them call their own destroy with an action.
			positionField.Resolve().Destroy();
			rotationField.Resolve().Destroy();
			scaleField.Resolve().Destroy();
			parentField.Resolve().Destroy();
			isActiveField.Resolve().Destroy();

			UnityEngine.Object.Destroy(gameObject);
			onDestroy?.Invoke();
		}
		
		public void OnInit()
		{
			_position = positionField.Resolve() as ValueField<Float3>;
			SetPosition(_position.value);
			
			_rotation = rotationField.Resolve() as ValueField<FloatQ>;
			SetRotation(_rotation.value);
			
			_scale = scaleField.Resolve() as ValueField<Float3>;
			SetScale(_scale.value);
			
			_parent = parentField.Resolve() as ReferenceField<Slot>;
			_isActive = isActiveField.Resolve() as ValueField<Bool>;

		}
	}
}