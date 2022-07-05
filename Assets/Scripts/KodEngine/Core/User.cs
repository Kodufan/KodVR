using System.Collections;
using System.Collections.Generic;
using System;
using KodEngine.Core;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Core
{
	public class User
	{
		public string userName;
		public string userID;
		public string machineID;
		
		public World owningWorld;
		public Slot userRoot { get; set; }

		public PlayerNetworkInstance networkInstance { get; set; }

		// Fix constructors so that slots can be created anywhere in a heirarchy and also any session
		// Also force slots to be placed in sessions
		public User(string userName, string userID, string machineID, World owningWorld)
		{
			this.userName = userName;
			this.userID = userID;
			this.machineID = machineID;
			this.owningWorld = owningWorld;
			//this.userRoot = userRoot;
			this.networkInstance = networkInstance;
		}
	}
}