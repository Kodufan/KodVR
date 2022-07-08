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
	public class JSONReader : Newtonsoft.Json.JsonConverter<Dictionary<RefID, WorldElement>>
	{
		public override void WriteJson(JsonWriter writer, Dictionary<RefID, WorldElement> value, JsonSerializer serializer)
		{
			throw new NotImplementedException("This should only be used for reading JSON");
		}

		public override Dictionary<RefID, WorldElement> ReadJson(JsonReader reader, Type objectType, Dictionary<RefID, WorldElement> existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject obj = JObject.Load(reader);

			Dictionary<RefID, WorldElement> returnDict = new Dictionary<RefID, WorldElement>();

			foreach (JToken token in obj.Children().Children().Children())
			{
				Type type = Type.GetType(token["$type"].ToString());
				
				switch (type) 
				{
					case Type slotType when slotType == typeof(Slot):
						Float3 position = new Float3((float)token["position"]["value"]["x"],
							(float)token["position"]["value"]["y"],
							(float)token["position"]["value"]["z"]);
						FloatQ rotation = new FloatQ((float)token["rotation"]["value"]["x"],
							(float)token["rotation"]["value"]["y"],
							(float)token["rotation"]["value"]["z"],
							(float)token["rotation"]["value"]["w"]);
						Float3 scale = new Float3((float)token["scale"]["value"]["x"],
							(float)token["scale"]["value"]["y"],
							(float)token["scale"]["value"]["z"]);
						Bool isActive = new Bool((bool)token["isActive"]["value"]["value"]);
						Slot slot = new Slot((string)token["name"], (string)token["tag"], position, rotation, scale, isActive);
						slot.SetID((ulong)token["refID"]["id"]);

						returnDict.Add(slot.refID, slot);
						break;
					case Type componentType when componentType == typeof(Component):
						break;
					case Type valueFieldType when valueFieldType == typeof(ValueField<>):
						break;
					case Type referenceFieldType when referenceFieldType == typeof(ReferenceField<>):
						break;
				}
			}

			return returnDict;
			
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