using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class BuiltInMaterial : Core.Component
	{
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

		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;
		}

		public override void OnAwake()
		{
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