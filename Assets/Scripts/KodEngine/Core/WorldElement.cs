using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public abstract class WorldElement
	{
		public RefID refID;

		// RefIDs of value 0 will auto increment. Any other number, and the ID will be set to that number
		[Newtonsoft.Json.JsonConstructor]
		public WorldElement()
		{
			refID = new RefID();
			Engine.refTable.RefIDDictionary.Add(refID, this);
		}

		

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