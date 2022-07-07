using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class BuiltInMaterial : Core.Component
	{
		private Material _material;

		[Newtonsoft.Json.JsonIgnore]
		public Material material
		{
			get
			{
				return Engine.builtInMaterial.material;
			}
		}

		public override string helpText
		{
			get
			{
				return "This material will always display an unlit checkerboard. This is the component version of the fallback material used when a renderer contains" +
					" no materials, or textures have not loaded.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			Engine.OnCommonUpdate += OnUpdate;
		}

		public override void OnDestroy()
		{
			UnityEngine.Debug.Log("Destroy was called");
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{
		}
	}
}