using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KodEngine;
using KodEngine.KodEBase;

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

		public override string helpText
		{
			get
			{
				return "The Texture2D component loads a texture either locally or from the cloud with a link, which can be used on materials. To view an image, please " +
					"create a plane and appropriate material, assigning both to a MeshRenderer";
			}
			set
			{
			}
		}

		// Incorperate OnChanged functionality into setter
		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.Texture2D texture { get; set; }

		public Texture2D(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public Texture2D(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
		}

		public override void OnAttach()
		{
			Engine.OnCommonUpdate += OnUpdate;

			uri = new System.Uri(@"C:\NullTex");
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
			byte[] bytes;
			try
			{
				bytes = System.IO.File.ReadAllBytes(path);
				texture = new UnityEngine.Texture2D(2, 2);
				texture.LoadImage(bytes);
				return texture;

			} catch (System.Exception e)
			{
				return (UnityEngine.Texture2D) Engine.builtInMaterial.material.mainTexture;
			}
		}
	}
}