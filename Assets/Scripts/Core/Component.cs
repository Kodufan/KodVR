using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Component
{
	public bool isEnabled;
	public int updateOrder;

	public abstract void OnAttach();
	public abstract void OnUpdate();
	
}
