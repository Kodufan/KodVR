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