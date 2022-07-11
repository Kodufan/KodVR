using System.Collections;
using System.Collections.Generic;
using System;
using KodEngine.Core;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public delegate void Initialized(User user);

	public class User : WorldElement
	{
		public string userName;
		public string userID;
		public string machineID;

		public RefID userRootField { get; set; }
		[Newtonsoft.Json.JsonIgnore]
		private ReferenceField<Slot> _userRoot;
		
		[Newtonsoft.Json.JsonIgnore]
		public PlayerNetworkInstance networkInstance { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public ulong unityNetworkID { get; set; }

		public static event Initialized userInitialized;

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public User(string userName, string userID, string machineID, ulong unityNetworkID)
		{
			_userRoot = new ReferenceField<Slot>();
			userRootField = _userRoot.refID;
			this.userName = userName;
			this.userID = userID;
			this.machineID = machineID;
			this.unityNetworkID = unityNetworkID;
			PlayerNetworkInstance.created += OnPlayerNetworkInstanceCreated;
		}

		public override string ToString()
		{
			return userName;
		}

		public void OnPlayerNetworkInstanceCreated(ulong target)
		{
			foreach (ulong uid in Unity.Netcode.NetworkManager.Singleton.ConnectedClientsIds)
			{
				if (target == uid)
				{
					networkInstance = Unity.Netcode.NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerNetworkInstance>();
					userInitialized?.Invoke(this);
				}
			}
		}

		public override void OnDestroy()
		{
			((Slot)userRootField.Resolve())?.Destroy();
		}
	}
}