using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KodEngine.KodEBase
{
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
}

