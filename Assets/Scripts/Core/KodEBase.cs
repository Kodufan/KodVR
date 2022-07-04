using System.Collections;
using System.Collections.Generic;
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
		
	}
	public class Color
	{
		public float r;
		public float g;
		public float b;
		public float a;
		public UnityEngine.Color unityColor;

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
				unityVector3.x = value;
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
				unityVector3.y = value;
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
				unityVector3.z = value;
			}
		}
		public UnityEngine.Vector3 unityVector3;

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
				unityQuaternion.x = value;
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
				unityQuaternion.y = value;
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
				unityQuaternion.z = value;
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
				unityQuaternion.w = value;
			}
		}
		
		public UnityEngine.Quaternion unityQuaternion;

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

