using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent
{
	Slot owner { get; }
	bool isEnabled { get; set; }
	int updateOrder { get; set; }

	public abstract void OnAttach();
	
	public abstract void OnUpdate();
	public abstract void OnDestroy();
}
