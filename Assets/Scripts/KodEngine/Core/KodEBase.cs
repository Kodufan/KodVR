using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using KodEngine.Core;


namespace KodEngine.KodEBase
{
	public class SyncValue<T> where T : WorldElement
	{
		public T value;
		public RefID refID;
	}

	public class ValueField<T> : WorldElement where T : IValue
	{
		public T value;
		
		public ValueField(T value) : base(true)
		{
			if (value == null)
			{
				this.value = (T)System.Activator.CreateInstance(typeof(T));
			} else
			{
				this.value = value;
			}
		}

		[Newtonsoft.Json.JsonConstructor]
		public ValueField(T value, RefID refID) : base()
		{
			if (value == null)
				
			{
				this.value = (T)System.Activator.CreateInstance(typeof(T));
			}
			else
			{
				this.value = value;
			}

			this.refID = refID;
			Engine.refTable.RefIDDictionary.Add(refID, this);
		}

		public override void OnDestroy()
		{

		}
	}

	public class ReferenceField<T> : WorldElement where T : WorldElement
	{
		private RefID _target;
		public RefID target
		{
			get
			{
				return _target;
			}
			set
			{
				System.Type refIDType = target?.ResolveType();
				if (refIDType != typeof(T) && refIDType != null && refIDType.GetType().IsSubclassOf(typeof(T)))
				{
					Debug.LogError("ReferenceField: Target is not of type " + typeof(T).Name);
				}
				_target = value;
			}
		}

		public ReferenceField(RefID target) : this()
		{
			this.target = target;
		}

		public ReferenceField() : base(true)
		{
			
		}
		
		[Newtonsoft.Json.JsonConstructor]
		public ReferenceField(RefID refID, RefID target) : base()
		{
			this.target = target;
			this.refID = refID;
			Engine.refTable.RefIDDictionary.Add(refID, this);
		}

		public T Resolve()
		{
			if (target != null && Engine.refTable.RefIDDictionary.ContainsKey(target))
			{
				return (T)target.Resolve();
			}
			return null;
		}

		public override void OnDestroy()
		{
			
		}
	}
	
	public class RefTable
	{
		[Newtonsoft.Json.JsonIgnore]
		public static readonly ulong RESERVED_IDS = 1000;
		[Newtonsoft.Json.JsonIgnore]
		public static ulong currID = RESERVED_IDS;
		
		public Dictionary<RefID, WorldElement> RefIDDictionary;

		[Newtonsoft.Json.JsonConstructor]
		public RefTable()
		{
			RefIDDictionary = new Dictionary<RefID, WorldElement>();
		}
		
		public void Clear()
		{
			this.RefIDDictionary.Clear();
			currID = RESERVED_IDS;
		}
	}
	

	// Create the ability to "rebalance" world IDs, as currently, it may be possible to run out of IDs by IDs not being flushed when deleted.
	public class RefID
	{
		public ulong id;

		public RefID()
		{
			this.id = RefTable.currID;
			RefTable.currID++;
		}

		// Do not manually assign RefIDs above RESERVED_IDS. It will break stuff.
		public RefID(ulong id)
		{
			if (Engine.refTable.RefIDDictionary.ContainsKey(this))
			{
				UnityEngine.Debug.LogError("RefID already taken!");
			}
			else
			{
				this.id = id;
			}
		}

		[Newtonsoft.Json.JsonConstructor]
		private RefID(string id)
		{
			this.id = ulong.Parse(id);
		}

		// This is hilariously dumb and stupid. Only used for getting dictionary values
		public RefID(ulong id, bool doesntMatter)
		{
			this.id = id;
		}

		public static explicit operator RefID(string x)
		{
			return new RefID(ulong.Parse(x.Substring(2)));
		}

		public override string ToString()
		{
			return "ID" + id.ToString();
		}

		public WorldElement Resolve()
		{
			if (Engine.refTable.RefIDDictionary.TryGetValue(this, out WorldElement value))
			{
				return value;
			}
			return null;
		}

		public System.Type ResolveType()
		{
			if (Engine.refTable.RefIDDictionary.TryGetValue(this, out WorldElement value))
			{
				return value.GetType();
			}
			return null;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is RefID))
			{
				return false;
			}
			
			RefID objID = (RefID)obj;
			if (this.id == objID.id)
			{
				return true;
			} else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}
	}
	
	public interface IRefID<out T> where T : WorldElement
	{
		T Resolve();
	}

	public interface IValue
	{
		public IValue GetValue();
		public void SetValue(IValue value);
	}

	public class Color : IValue
	{
		private float _r;
		public float r
		{
			get { return _r; }
			set {
				_r = value;
				_unityColor.r = value;
			}
		}

		private float _g;
		public float g
		{
			get { return _g; }
			set
			{
				_g = value;
				_unityColor.g = value;
			}
		}

		private float _b;
		public float b
		{
			get { return _b; }
			set
			{
				_b = value;
				_unityColor.b = value;
			}
		}

		private float _a;
		public float a
		{
			get { return _a; }
			set
			{
				_a = value;
				_unityColor.a = value;
			}
		}

		private UnityEngine.Color _unityColor;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.Color unityColor
		{
			get { return _unityColor; }
			set
			{
				_unityColor = value;
				_r = value.r;
				_g = value.g;
				_b = value.b;
				_a = value.a;
			}
		}

		public Color(UnityEngine.Color unityColor) : this(unityColor.r, unityColor.g, unityColor.b, unityColor.a) {}

		public Color() : this(0, 0, 0, 1) {}

		public Color(float r, float g, float b) : this(r, g, b, 1) {}

		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;

			unityColor = new UnityEngine.Color(r, g, b, a);
		}

		public override string ToString()
		{
			return unityColor.ToString();
		}

		public IValue GetValue()
		{
			return this;
		}

		public void SetValue(IValue value)
		{
			if (value.GetType() != typeof(Color))
			{
				throw new System.Exception("Cannot set value to Color from " + value.GetType());
			}

			Color color = (Color)value;
			r = color.r;
			g = color.g;
			b = color.b;
		}
	}

	public class Float3 : IValue
	{
		private float _x;
		public float x
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
				_unityVector3.x = value;
			}
		}
		
		private float _y;
		public float y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
				_unityVector3.y = value;
			}
		}

		private float _z;
		public float z
		{
			get
			{
				return _z;
			}
			set
			{
				_z = value;
				_unityVector3.z = value;
			}
		}
		private UnityEngine.Vector3 _unityVector3;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.Vector3 unityVector3
		{
			get
			{
				return _unityVector3;
			}
			set
			{
				_unityVector3 = value;
				_x = value.x;
				_y = value.y;
				_z = value.z;
			}
		}

		public static Float3 zero
		{
			get
			{
				return new Float3(0, 0, 0);
			}
		}

		public static Float3 one
		{
			get
			{
				return new Float3(1, 1, 1);
			}
		}

		public Float3() : this(0, 0, 0) {}
		
		public Float3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;

			unityVector3 = new UnityEngine.Vector3(x, y, z);
		}

		public override string ToString() 
		{
			return unityVector3.ToString();
		}

		public IValue GetValue()
		{
			return this;
		}

		public void SetValue(IValue value)
		{
			if (value.GetType() != typeof(Float3))
			{
				throw new System.Exception("Cannot set value to Float3 from " + value.GetType());
			}

			Float3 float3 = (Float3)value;
			x = float3.x;
			y = float3.y;
			z = float3.z;
		}
	}

	public class FloatQ : IValue
	{
		private float _x;
		public float x
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
				_unityQuaternion.x = value;
			}
		}
		
		private float _y;
		public float y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
				_unityQuaternion.y = value;
			}
		}
		
		private float _z;
		public float z
		{
			get
			{
				return _z;
			}
			set
			{
				_z = value;
				_unityQuaternion.z = value;
			}
		}
		
		private float _w;
		public float w
		{
			get
			{
				return _w;
			}
			set
			{
				_w = value;
				_unityQuaternion.w = value;
			}
		}

		private UnityEngine.Quaternion _unityQuaternion;

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.Quaternion unityQuaternion
		{
			get
			{
				return new UnityEngine.Quaternion(_x, _y, _z, _w);
			}
			set
			{
				_x = value.x;
				_y = value.y;
				_z = value.z;
				_w = value.w;
			}
		}

		public static FloatQ identity
		{
			get
			{
				return new FloatQ(0, 0, 0, 1);
			}
		}

		public FloatQ() : this(0, 0, 0, 0) { }

		public FloatQ(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;

			unityQuaternion = new UnityEngine.Quaternion(x, y, z, w);
		}

		public override string ToString()
		{
			return unityQuaternion.ToString();
		}

		public IValue GetValue()
		{
			return this;
		}

		public void SetValue(IValue value)
		{
			if (value.GetType() != typeof(FloatQ))
			{
				throw new System.Exception("Cannot set value to FloatQ from " + value.GetType());
			}

			FloatQ floatQ = (FloatQ)value;
			x = floatQ.x;
			y = floatQ.y;
			z = floatQ.z;
			w = floatQ.w;
		}
	}

	public class Bool : IValue
	{
		public bool value;

		public IValue GetValue()
		{
			return this;
		}

		public Bool() : this(true) { }

		public Bool(bool value) { this.value = value; }

		public void SetValue(IValue value)
		{
			if (value.GetType() != typeof(Bool))
			{
				throw new System.Exception("Cannot set value to Bool from " + value.GetType());
			}
		}
	}
}

