using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public abstract class Mesh : Component
	{
		[Newtonsoft.Json.JsonIgnore]
		public GameObject meshObject;

		[Newtonsoft.Json.JsonIgnore]
		public MeshFilter meshFilter;

		protected Mesh(RefID owner) : base(owner)
		{
		}

		public Mesh(RefID owner, bool isEnabled, int updateOrder) : base(owner, isEnabled, updateOrder)
		{
			this.isEnabled = isEnabled;
			this.updateOrder = updateOrder;
		}

		public override void OnAttach()
		{
			meshObject = new GameObject("Mesh Object");
			Slot ownerSlot = (Slot)owner.Resolve();
			meshObject.transform.parent = ownerSlot.gameObject.transform;
			meshFilter = meshObject.AddComponent<MeshFilter>();
		}
	}
}