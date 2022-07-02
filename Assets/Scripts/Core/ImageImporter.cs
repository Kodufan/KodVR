using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageImporter : MonoBehaviour
{
	public static UnityEngine.Texture2D Import(string path)
	{
		UnityEngine.Texture2D texture;
		byte[] bytes = System.IO.File.ReadAllBytes(path);
		texture = new UnityEngine.Texture2D(2, 2);
		texture.LoadImage(bytes);
		return texture;
	}

	void Start()
	{
		var texture = Import("C:\\Program Files (x86)\\Steam\\userdata\\207376680\\760\\remote\\740250\\screenshots\\20220630194923_1.jpg");
		UnityEngine.Material material = new Material(Shader.Find("Specular"));
		UnityEditor.Presets.Preset preset = Resources.Load<UnityEditor.Presets.Preset>("Materials/PBS_Metallic");
		preset.ApplyTo(material);
		material.mainTexture = texture;

		gameObject.GetComponent<Renderer>().material = material;
	}
}
