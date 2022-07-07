using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public abstract class WorldElement
	{
		public RefID refID;

		public WorldElement()
		{
			refID = new RefID();
			RefTable.RefIDDictionary.Add(refID, this);
		}

		public void Destroy()
		{
			RefTable.RefIDDictionary.Remove(refID);
			OnDestroy();
		}

		public abstract void OnDestroy();
	}
}