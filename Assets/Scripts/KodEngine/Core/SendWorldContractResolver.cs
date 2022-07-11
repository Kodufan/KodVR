using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using KodEngine.Core;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace KodEngine.Core
{
	public class SendWorldContractResolver : DefaultContractResolver
	{
		public static readonly SendWorldContractResolver Instance = new SendWorldContractResolver();

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty(member, memberSerialization);
			
			if (property.DeclaringType.IsSubclassOf(typeof(Component)))
			{
				property.ShouldSerialize =
					instance =>
					{
						Core.Component c = (Core.Component)instance;
						return c.shouldSerialize;
					};
			}
			return property;
		}
	}
}

