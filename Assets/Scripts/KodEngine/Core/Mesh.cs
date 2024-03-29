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

		public Mesh(RefID refID, RefID owner, bool isEnabled, int updateOrder) : base(refID, owner, isEnabled, updateOrder)
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

		public override void OnDestroy()
		{
			UnityEngine.GameObject.Destroy(meshFilter);
			UnityEngine.GameObject.Destroy(meshObject);
		}
	}
}