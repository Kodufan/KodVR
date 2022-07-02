using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class ProceduralSphereMesh : Core.Mesh
	{
		public Mesh sphere { get; set; }

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

			Debug.Log(meshFilter);
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