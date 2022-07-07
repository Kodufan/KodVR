using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.Core;

namespace KodEngine.Component
{
	public class ProceduralSphereMesh : Core.Mesh
	{
		public UnityEngine.Mesh sphere { get; set; }

		public override string helpText
		{
			get
			{
				return "The ProceduralSphereMesh component will provide a dynamic sphere mesh based on its properties which can be used either for rendering or colliders.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			base.OnAttach();
			Engine.OnCommonUpdate += OnUpdate;

			Slot ownerSlot = (Slot)owner.Resolve();
			meshObject.transform.SetParent(ownerSlot.gameObject.transform);

			meshFilter.mesh = Resources.GetBuiltinResource<UnityEngine.Mesh>("Sphere.fbx");
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