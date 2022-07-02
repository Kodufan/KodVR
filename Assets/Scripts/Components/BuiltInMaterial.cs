using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltInMaterial : Component
{
	public Material material { get; set; }

	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
		material = Resources.Load<Material>("Materials/BuiltInMaterial");
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
