using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;
using KodEngine.KodEBase;

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

		public BuiltInMaterial(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public BuiltInMaterial(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
		}

		public override void OnAttach()
		{
			Engine.OnCommonUpdate += OnUpdate;
		}

		public override void OnDestroy()
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{
		}
	}
}