using System.Reflection;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Component
{
	public class PBS_Metallic : Core.Component
	{
		public System.Uri shaderUri { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public UnityEngine.Material material { get; set; }

		public ReferenceField<Texture2D> texture;

		private Color _albedo = new Color(1, 1, 1, 1);

		public Color albedo
		{
			get
			{
				return _albedo;
			}
			set
			{
				_albedo = value;
				OnChange();
			}
		}

		public override string helpText
		{
			get
			{
				return "The PBS_Metallic material is a material used for creating reflective, metallic appearing objects.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			texture = new ReferenceField<Texture2D>();
			Engine.OnCommonUpdate += OnUpdate;
			UnityEngine.Shader shader = UnityEngine.Resources.Load<UnityEngine.Shader>("Shaders/Root_Folder/Standard");
			material = new UnityEngine.Material(shader);
			//UnityEditor.Presets.Preset preset = UnityEngine.Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic");
			//preset.ApplyTo(material);
		}

		public override void OnDestroy()
		{
		}

		public override void OnUpdate()
		{

		}

		public override void OnChange()
		{
			material.color = albedo.unityColor;

			if (texture.refID.Resolve() != null)
			{
				Texture2D texture2D = (Texture2D)texture.refID.Resolve();
				PropertyInfo textureProperty = typeof(Texture2D).GetProperty("texture");
				material.mainTexture = (UnityEngine.Texture2D)textureProperty.GetValue(texture2D);
			}
		}
	}
}