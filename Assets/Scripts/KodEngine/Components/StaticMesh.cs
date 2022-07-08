using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;
using KodEngine.KodEBase;

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

		public StaticMesh(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public StaticMesh(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
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