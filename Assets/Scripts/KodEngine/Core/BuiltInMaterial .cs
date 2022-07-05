using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Core
{
	public class BuiltInMaterial
	{
		private Material _material;
		public Material material { 
			
			get
			{
				return _material;
			}
		}

		public BuiltInMaterial()
		{
			_material = Resources.Load<Material>("Materials/BuiltInMaterial");
		}
	}
}