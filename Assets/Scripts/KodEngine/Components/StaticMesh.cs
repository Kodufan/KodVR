using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class StaticMesh : Core.Mesh
	{
		public System.Uri uri { get; set; }

		public override string helpText
		{
			get
			{
				return "The StaticMesh component loads a mesh either locally or from the cloud with a link, which can be used either for rendering or colliders.";
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
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{

		}
	}
}