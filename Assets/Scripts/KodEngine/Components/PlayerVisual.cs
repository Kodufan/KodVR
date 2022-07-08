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

		public PlayerVisual(RefID owner) : base(owner)
		{
		}

		[Newtonsoft.Json.JsonConstructor]
		public PlayerVisual(RefID refID, bool isEnabled, int updateOrder) : base(refID, isEnabled, updateOrder)
		{
		}

		public override void OnAttach()
		{
			Slot ownerSlot = (Slot)owner.Resolve();
			ProceduralBoxMesh mesh = ownerSlot.AttachComponent<ProceduralBoxMesh>();
			PBS_Metallic material = ownerSlot.AttachComponent<PBS_Metallic>();
			MeshRenderer renderer = ownerSlot.AttachComponent<MeshRenderer>();

			renderer.mesh.target = mesh.refID;
			renderer.material.target = material.refID;

			User user = ownerSlot.TryGetUser();
			user.networkInstance.changed += OnUpdate;
		}
		
		public void OnUpdate(UnityEngine.Vector3 value)
		{
			// This is bad dumb code that should be changed later.
			Slot ownerSlot = (Slot)owner.Resolve();
			ownerSlot.position.value.unityVector3 = value;
		}

		public override void OnUpdate()
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