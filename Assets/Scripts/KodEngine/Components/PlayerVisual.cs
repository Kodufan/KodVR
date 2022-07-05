//using UnityEngine;
//using UnityEngine.InputSystem;
using KodEngine.Core;
using KodEngine.Component;
using KodEngine.KodEBase;

namespace KodEngine.Component
{
	public class PlayerVisual : Core.Component
	{
		


		public override string helpText
		{
			get
			{
				return "This PlayerVisual component is a temporary component to place a player visual on a user.";
			}
			set
			{
			}
		}

		public override void OnAttach()
		{
			ProceduralBoxMesh mesh = owner.AttachComponent<ProceduralBoxMesh>();
			PBS_Metallic material = owner.AttachComponent<PBS_Metallic>();
			MeshRenderer renderer = owner.AttachComponent<MeshRenderer>();

			renderer.mesh = mesh;
			renderer.material = material;

			User user = owner.TryGetUser();
			UnityEngine.Debug.Log(user);
			user.networkInstance.changed += OnUpdate;
		}
		
		public void OnUpdate(UnityEngine.Vector3 value)
		{
			owner.position.unityVector3 = value;
		}

		public override void OnUpdate()
		{
		}

		public void OnConnection(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, Unity.Netcode.NetworkManager.ConnectionApprovalResponse response)
		{
			UnityEngine.Debug.Log("Hi");
		}

		public override void OnAwake()
		{
		}

		public override void OnAsleep()
		{
		}

		public override void OnDestroy()
		{
		}

		public override void OnChange()
		{
		}
	}
}