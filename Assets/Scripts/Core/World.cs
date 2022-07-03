using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using KodEngine.Component;
using KodEngine.Core;

namespace KodEngine.Core
{
	public delegate void Focus();

	public class World
	{
		public Slot root;
		public event Focus OnFocusGained;
		public event Focus OnFocusLost;

		public void Close()
		{

		}

		public World() : this(WorldType.Default)
		{
		}

		// Constructor
		public World(WorldType type)
		{
			// Create the root object
			root = CreateSlot("Root");

			UnityEngine.GameObject gameObject = new UnityEngine.GameObject(type.ToString());
			gameObject.transform.parent = WorldManager.worldRoot.transform;
			root.SetParent(gameObject);

			// Create the world
			switch (type)
			{
				case WorldType.Default:
					Slot cube = new Slot("Cube", this);
					cube.SetParent(root);

					cube.gameObject.transform.localPosition = new UnityEngine.Vector3(1, 0, -.75f);

					Texture2D tex = cube.AttachComponent<Texture2D>();
					tex.uri = new System.Uri("C:\\Program Files (x86)\\Steam\\userdata\\207376680\\760\\remote\\740250\\screenshots\\20220630194923_1.jpg");

					PBS_Metallic material = cube.AttachComponent<PBS_Metallic>();
					material.texture = tex;

					ProceduralSphereMesh sphereMesh = cube.AttachComponent<ProceduralSphereMesh>();
					ProceduralBoxMesh boxMesh = cube.AttachComponent<ProceduralBoxMesh>();
					MeshRenderer renderer = cube.AttachComponent<MeshRenderer>();
					renderer.material = material;
					renderer.mesh = sphereMesh;

					renderer.mesh = boxMesh;

					material.albedo = new KodEBase.Color(1, 1, 1, 1);

					MeshCollider collider = cube.AttachComponent<MeshCollider>();
					collider.mesh = boxMesh;


					
					Slot cube2 = new Slot("Cube2", this);
					cube2.SetParent(root);

					cube2.gameObject.transform.localPosition = new UnityEngine.Vector3(1, 0, .75f);

					Texture2D tex2 = cube2.AttachComponent<Texture2D>();
					tex2.uri = new System.Uri("C:\\Users\\koduf\\Desktop\\Memes\\718c6523d13d52ea0d5decf15988d119d2d24305a72b1e680f5acb24e943295d_1.png");

					PBS_Metallic material2 = cube2.AttachComponent<PBS_Metallic>();
					material2.texture = tex2;

					ProceduralSphereMesh sphereMesh2 = cube2.AttachComponent<ProceduralSphereMesh>();
					ProceduralBoxMesh boxMesh2 = cube2.AttachComponent<ProceduralBoxMesh>();
					MeshRenderer renderer2 = cube2.AttachComponent<MeshRenderer>();
					renderer2.material = material2;
					renderer2.mesh = sphereMesh2;

					renderer2.mesh = boxMesh2;

					material2.albedo = new KodEBase.Color(1, 1, 1, 1);

					MeshCollider collider2 = cube2.AttachComponent<MeshCollider>();
					collider2.mesh = boxMesh2;



					root.AttachPlane(UnityEngine.Color.grey);
					break;
				case WorldType.Space:
					root.AttachPlane(UnityEngine.Color.black);
					break;
				case WorldType.Gridspace:
					root.AttachPlane(UnityEngine.Color.white);
					break;
				case WorldType.Debug:
					root.AttachPlane(UnityEngine.Color.blue);
					break;
				case WorldType.Custom:
					root.AttachPlane(UnityEngine.Color.red);
					break;
			}
			root.AttachComponent<CharacterController>();
		}

		public void Focus()
		{
			root.gameObject.SetActive(true);
			OnFocusGained?.Invoke();
		}

		public void Unfocus()
		{
			root.gameObject.SetActive(false);
			OnFocusLost?.Invoke();
		}

		public Slot CreateSlot(string name)
		{
			return new Slot(name, this);
		}
	}

}