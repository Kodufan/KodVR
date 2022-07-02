using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture2D : Component
{
	public System.Uri uri { get; set; }

	// Incorperate OnChanged functionality into setter
	public UnityEngine.Texture2D texture
	{
		get { return texture; }
		set
		{
			this.texture = texture;
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
		uri = new System.Uri("C:\\Users\\koduf\\Downloads\\null.bmp");

		
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
		Start();
	}

	void Start()
	{
		DownloadImage(uri.ToString());
	}

	IEnumerator DownloadImage(string MediaUrl)
	{
		Debug.Log(MediaUrl);
		UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();
		if (request.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || request.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
			Debug.Log(request.error);
		else
			texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
	}
}
