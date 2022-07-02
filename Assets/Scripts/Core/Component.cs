using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Component
{
	// Owner is settable in order to allow attaching components. I do not like this solution,
	// but I do not know how to avoid it
	public Slot owner { get; set; }
	public bool isEnabled { get; set; }
	public int updateOrder { get; set; }

	public Component(Slot owner)
	{
		this.owner = owner;
	}

	public Component() {}

	public abstract void OnAttach();
	public abstract void OnAwake();
	public abstract void OnAsleep();
	public abstract void OnUpdate();
	public abstract void OnDestroy();
}
