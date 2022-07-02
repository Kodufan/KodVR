using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;

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
		private UnityEngine.Color _albedo = UnityEngine.Color.white;
		public UnityEngine.Color albedo
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


		public override void OnAsleep()
		{
		}

		public override void OnAttach()
		{
			owner.owningWorld.OnFocusGained += OnAwake;
			owner.owningWorld.OnFocusLost += OnAsleep;
			Engine.OnCommonUpdate += OnUpdate;
			material = new UnityEngine.Material(UnityEngine.Shader.Find("Specular"));
			UnityEditor.Presets.Preset preset = UnityEngine.Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic");
			preset.ApplyTo(material);
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
			material.color = albedo;
			material.mainTexture = texture.texture;
		}
	}
}