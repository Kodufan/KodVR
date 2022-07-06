using KodEngine.Component;
using KodEngine.KodEBase;
using System;
using System.Collections.Generic;

namespace KodEngine.Core
{
	public class SerializeSlot
	{
		public string name;
		public string tag;
		private int orderOffset;

		public Float3 position;


		public FloatQ rotation;

		public Float3 scale;

		public List<SerializeSlot> children;
		public List<Component> components;
		public bool isActive;

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public SerializeSlot(string name, string tag, int orderOffset, Float3 position, FloatQ rotation, Float3 scale, List<SerializeSlot> children, List<Component> components, bool isActive)
		{
			this.name = name;
			this.tag = tag;
			this.orderOffset = orderOffset;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			this.children = children;
			this.components = components;
			this.isActive = isActive;
		}
	}
}