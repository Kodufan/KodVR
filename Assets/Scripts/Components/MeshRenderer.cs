using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderer : Component
{
	public Mesh mesh;
	public PBS_Metallic material;
	public BuiltInMaterial builtInMaterial;
	private UnityEngine.MeshRenderer renderer;
	private MeshFilter filter;
	
	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
		renderer = owner.gameObject.AddComponent<UnityEngine.MeshRenderer>();
		filter = owner.gameObject.AddComponent<MeshFilter>();

		builtInMaterial = new BuiltInMaterial();
		builtInMaterial.owner = owner;
		builtInMaterial.OnAttach();
	}

	public override void OnAwake()
	{
	}

	public override void OnDestroy()
	{
	}

	public override void OnUpdate()
	{
		if (filter != null)
		{
			filter.mesh = mesh;
		}

		if (renderer != null && material != null)
		{
			renderer.material = material.material;
			//Debug.Log(material.material);
		}

		if (material == null)
		{
			renderer.material = builtInMaterial.material;
		}
	}
}
