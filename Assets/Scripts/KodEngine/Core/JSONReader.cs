using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.Core;
using KodEngine.KodEBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KodEngine.Core
{
	public class JSONReader : Newtonsoft.Json.JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException("This should only be used for reading JSON");
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Slot).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return null;
			//JObject obj = JObject.Load(reader);
			//string type = (string)obj["$type"];
			//if (type.Contains("KodEngine.Core.Slot"))
			//{

			//}

			//UnityEngine.Debug.Log(existingValue);


			//foreach (RefID reference in s.components)
			//{
			//	Component c = (Component)reference.Resolve();
			//	UnityEngine.Debug.Log(c.ToString());
			//}
			//return new Version(s.ToString());
		}
	}
}