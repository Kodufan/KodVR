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
				material.color = value.unityColor;
				_albedo = value;
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

		public PBS_Metallic(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public PBS_Metallic(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
			// Stinky solution
			UnityEngine.Shader shader = UnityEngine.Resources.Load<UnityEngine.Shader>("Shaders/Root_Folder/Standard");
			material = new UnityEngine.Material(shader);
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
			texture.Destroy();
		}

		public override void OnUpdate()
		{

		}

		public override void OnChange()
		{
		}

		public void SetTexture(RefID refID)
		{
			if (refID.ResolveType() == typeof(Texture2D))
			{
				texture.target = refID;
				Texture2D texture2D = (Texture2D)texture.target.Resolve();
				material.mainTexture = texture2D.texture;
			}
		}

		public void SetColor(Color color)
		{
			albedo = color;
			material.color = color.unityColor;
		}
	}
}