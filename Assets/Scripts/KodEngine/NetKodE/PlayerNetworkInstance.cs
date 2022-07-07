using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public delegate void Changed(Vector3 float3);
public delegate void Created(ulong uid);
public delegate void Destroyed(ulong uid);

public class PlayerNetworkInstance : NetworkBehaviour
{
	public event Changed changed;
	public static event Created created;
	public static event Destroyed destroyed;

	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

	public CustomMessagingManager manager;
	
	public string worldID;

	
	// Start is called before the first frame update
	void Start()
    {
		if (IsOwner)
		{
			manager = NetworkManager.Singleton.CustomMessagingManager;
			//manager.RegisterNamedMessageHandler(name, OnRecieveMessage);
			manager.OnUnnamedMessage += OnRecieveMessage;

			//Sending
			using FastBufferWriter writer = new FastBufferWriter(256, Unity.Collections.Allocator.Temp);
			writer.WriteValueSafe("This is a test message from uid: " + NetworkManager.Singleton.LocalClient.ClientId);
			ulong serverID = NetworkManager.ServerClientId;
			manager.SendUnnamedMessage(serverID, writer, NetworkDelivery.ReliableSequenced); //NetworkDelivery is optional.	
		}
	}

	void OnRecieveMessage(ulong senderClientId, FastBufferReader messagePayload)
	{
		messagePayload.ReadValueSafe(out string message); //Example
		Debug.Log(message);
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
			created?.Invoke(OwnerClientId);
		}
	}

	public override void OnNetworkDespawn()
	{
		destroyed?.Invoke(OwnerClientId);
		Debug.Log("Bye :(");
	}
}
