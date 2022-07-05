using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public delegate void Changed(Vector3 float3);

public class PlayerNetworkInstance : NetworkBehaviour
{
	public event Changed changed;

	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

	public string worldID;

	
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnPositionChanged(Vector3 previous, Vector3 current)
	{
		changed?.Invoke(current);
	}

	[ServerRpc]
	public void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
	{
		Position.Value = new Vector3(2, 1, 0);
	}

	public override void OnNetworkSpawn()
	{
		if (IsOwner)
		{
			Debug.Log("Hi");
		}
	}
}
