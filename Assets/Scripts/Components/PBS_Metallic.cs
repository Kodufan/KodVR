using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBS_Metallic : Component
{
	public System.Uri shaderUri { get; set; }
	public Material material { get; set; }
	public Color albedo;
	public Texture2D texture;

	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
		material = Resources.Load<Material>("Materials/Standard");
	}

	public override void OnAwake()
	{
	}

	public override void OnDestroy()
	{
	}

	public override void OnUpdate()
	{
		if (albedo != null)
		{
			material.color = albedo;

		}
	}
}
