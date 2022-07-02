using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBS_Metallic : Component
{
	public System.Uri shaderUri { get; set; }
	public Material material { get; set; }
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
			OnChanged();
		}
	}
	private Color _albedo = Color.white;
	public Color albedo { 
		get
		{
			return _albedo;
		}
		set
		{
			_albedo = value;
			OnChanged();
		}
	}
	

	public override void OnAsleep()
	{
	}

	public override void OnAttach()
	{
		owner.owningWorld.OnFocusGained += OnAwake;
		owner.owningWorld.OnFocusLost += OnAsleep;
		KodEngine.OnCommonUpdate += OnUpdate;
		material = new Material(Shader.Find("Specular"));
		UnityEditor.Presets.Preset preset = Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic");
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

	public void OnChanged()
	{
		material.color = albedo;
		material.mainTexture = texture.texture;
	}
}
