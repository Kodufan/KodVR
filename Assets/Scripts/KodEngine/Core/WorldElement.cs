using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public abstract class WorldElement
	{
		public RefID refID;

		// This is hilariously bad. The reason this exists is that the deserializer will call this constructor and fuck up the RefIDs,
		// so I am creating a constructor with an additional parameter so that the deserializer will not call it.
		public WorldElement(bool doesntMatter)
		{
			refID = new RefID();
			Engine.refTable.RefIDDictionary.Add(refID, this);
		}

		[Newtonsoft.Json.JsonConstructor]
		public WorldElement() {}

		// This is stupid and bad dumb code.
		public void SetID(ulong id)
		{
			Engine.refTable.RefIDDictionary.Remove(this.refID);
			refID = new RefID(id);
			Engine.refTable.RefIDDictionary.Add(refID, this);
		}

		public void Destroy()
		{
			Engine.refTable.RefIDDictionary.Remove(refID);
			OnDestroy();
		}

		public abstract void OnDestroy();
	}
}