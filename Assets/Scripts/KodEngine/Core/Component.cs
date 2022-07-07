using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public abstract class Component : WorldElement
	{
		// Owner is settable in order to allow attaching components. I do not like this solution,
		// but I do not know how to avoid it
		[Newtonsoft.Json.JsonIgnore]
		public RefID owner { get; set; }

		public string componentName => "KodEngine." + GetType().Name;

		[Newtonsoft.Json.JsonIgnore]
		public abstract string helpText { get; set; }
		public bool isEnabled { get; set; }
		public int updateOrder { get; set; }

		public Component(RefID owner) : base()
		{
			this.owner = owner;
		}

		public Component() { }

		public abstract void OnAttach();
		public abstract void OnUpdate();
		public abstract void OnChange();
	}
}


