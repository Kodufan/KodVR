using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBoxMesh : Component
{
	public Mesh box { get; set; }

	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
		MeshFilter mesh = owner.gameObject.AddComponent<MeshFilter>();
		mesh.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
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
}
