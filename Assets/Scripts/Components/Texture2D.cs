using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;

namespace KodEngine.Component
{
	public class Texture2D : Core.Component
	{
		private System.Uri _uri;

		public System.Uri uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				_uri = value;
				OnChange();
			}
		}

		// Incorperate OnChanged functionality into setter

		public UnityEngine.Texture2D texture { get; set; }


		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;
			//Debug.Log(uri);
		}

		public override void OnAwake()
		{
		}

		public override void OnDestroy()
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnChange()
		{
			texture = Import(uri.ToString());
		}

		public UnityEngine.Texture2D Import(string path)
		{
			UnityEngine.Texture2D texture;
			path = path.Substring(8);
			byte[] bytes = System.IO.File.ReadAllBytes(path);
			texture = new UnityEngine.Texture2D(2, 2);
			texture.LoadImage(bytes);
			return texture;
		}
	}
}