using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMesh : Component
{
	public System.Uri uri { get; set; }

	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
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
