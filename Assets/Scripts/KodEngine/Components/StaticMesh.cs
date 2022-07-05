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