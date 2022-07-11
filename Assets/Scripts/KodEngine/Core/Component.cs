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
		public RefID owner { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public abstract string helpText { get; set; }
		public bool isEnabled { get; set; }
		public bool isPersistant { get; set; }
		[Newtonsoft.Json.JsonIgnore]
		public bool shouldSerialize { get; set; }
		public int updateOrder { get; set; }
		
		public Component(RefID owner) : base(true)
		{
			shouldSerialize = true;
			this.owner = owner;
			OnAttach();
		}
		
		public Component(RefID refID, RefID owner, bool isEnabled, int updateOrder) : base()
		{
			Engine.refTable.RefIDDictionary.Add(refID, this);
			this.refID = refID;
			this.owner = owner;
			this.isEnabled = isEnabled;
			this.updateOrder = updateOrder;
			shouldSerialize = true;
			WorldManager.onWorldLoaded += OnInit;
			Slot ownerSlot = (Slot)owner.Resolve();
			ownerSlot.onDestroy += OnDestroy;
			
		}

		public abstract void OnAttach();
		public abstract void OnUpdate();
		public abstract void OnChange();
		public virtual void OnInit()
		{
			OnAttach();
		}
	}
}


