using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Component
{
	public class PBS_Metallic : Core.Component
	{
		public System.Uri shaderUri { get; set; }
		public UnityEngine.Material material { get; set; }
		private Texture2D _texture;
		public Texture2D texture
		{
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				OnChange();
			}
		}

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


		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;
			UnityEngine.Shader shader = UnityEngine.Resources.Load<UnityEngine.Shader>("Shaders/Root_Folder/Standard");
			material = new UnityEngine.Material(shader);
			//UnityEditor.Presets.Preset preset = UnityEngine.Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic");
			//preset.ApplyTo(material);
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
			//UnityEngine.Debug.Log(albedo.unityColor);
			material.color = albedo.unityColor;

			if (texture != null)
			{
				material.mainTexture = texture.texture;
			}
		}
	}
}