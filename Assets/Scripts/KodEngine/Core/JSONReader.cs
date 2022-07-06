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
			JObject obj = JObject.Load(reader);
			string type = (string)obj["$type"];
			UnityEngine.Debug.Log(type);
			Float3 position = new Float3((float)obj["position"]["x"], (float)obj["position"]["y"], (float)obj["position"]["z"]);
			FloatQ rotation = new FloatQ((float)obj["rotation"]["x"], (float)obj["rotation"]["y"], (float)obj["rotation"]["z"], (float)obj["rotation"]["w"]);
			Float3 scale = new Float3((float)obj["scale"]["x"], (float)obj["scale"]["y"], (float)obj["scale"]["z"]);
			Slot s = new Slot((string)obj["name"], (string)obj["tag"], position, rotation, scale, null, (bool) obj["isActive"]);

			UnityEngine.Debug.Log(existingValue);

			
			foreach (Component c in s.components)
			{
				UnityEngine.Debug.Log(c.ToString());
			}
			return new Version(s.ToString());
		}
	}
}