using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Core
{
	public abstract class Mesh : Component
	{
		public GameObject meshObject = new GameObject("Mesh Object");
		public MeshFilter meshFilter;


		public override void OnAttach()
		{
			meshObject.transform.parent = owner.gameObject.transform;
			meshFilter = meshObject.AddComponent<MeshFilter>();
		}
	}
}