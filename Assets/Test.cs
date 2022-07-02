using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
		UnityEngine.Texture2D tex = Resources.Load<UnityEngine.Texture2D>("Textures/null.jpg"); ;
		System.Uri texture = new System.Uri("C:\\Users\\koduf\\Desktop\\Memes\\718c6523d13d52ea0d5decf15988d119d2d24305a72b1e680f5acb24e943295d_1.png");
		Debug.Log(texture);
		UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(texture);
		yield return request.SendWebRequest();
		if (request.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || request.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
			Debug.Log(request.error);
		
		else
			tex = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;

		UnityEngine.MeshRenderer renderer = gameObject.GetComponent<UnityEngine.MeshRenderer>();
		renderer.material.mainTexture = tex;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
