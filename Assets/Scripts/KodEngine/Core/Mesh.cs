using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Core
{
	public abstract class Mesh : Component
	{
		[Newtonsoft.Json.JsonIgnore]
		public GameObject meshObject;

		[Newtonsoft.Json.JsonIgnore]
		public MeshFilter meshFilter;


		public override void OnAttach()
		{
			meshObject = new GameObject("Mesh Object");
			meshObject.transform.parent = owner.gameObject.transform;
			meshFilter = meshObject.AddComponent<MeshFilter>();
		}
	}
}