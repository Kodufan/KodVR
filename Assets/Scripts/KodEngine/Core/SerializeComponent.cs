using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KodEngine.Core
{
	public abstract class SerializeComponent
	{
		public abstract string helpText { get; set; }
		public bool isEnabled { get; set; }
		public int updateOrder { get; set; }

		public SerializeComponent() { }

		public abstract void OnAttach();
		public abstract void OnUpdate();
		public abstract void OnChange();
		public abstract void OnDestroy();
	}
}


