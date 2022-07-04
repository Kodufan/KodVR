using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class ProceduralSphereMesh : Core.Mesh
	{
		public Mesh sphere { get; set; }

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

		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			base.OnAttach();
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;

			meshObject.transform.SetParent(owner.gameObject.transform);

			meshFilter.mesh = Resources.GetBuiltinResource<UnityEngine.Mesh>("Sphere.fbx");
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