using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace KodEngine.KodEBase
{
	public class SyncValue<T>
	{
		public T value;
		public RefID refID;
	}

	public class RefID
	{
		public static ulong currID;
		public ulong id;

		public RefID()
		{
			this.id = currID;
			currID++;
		}
	}
	public class Color
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
	}

	public class Float3
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
	}

	public class FloatQ
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
	}
}

