using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class ProceduralBoxMesh : Core.Mesh
	{
		public override string helpText
		{
			get
			{
				return "The ProceduralBoxMesh component will provide a dynamic cube mesh based on its properties which can be used either for rendering or colliders.";
			}
			set
			{
			}
		}


		public override void OnAttach()
		{
			base.OnAttach();
			Engine.OnCommonUpdate += OnUpdate;

			meshObject.transform.SetParent(owner.gameObject.transform);

			meshFilter.mesh = Resources.GetBuiltinResource<UnityEngine.Mesh>("Cube.fbx");
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

