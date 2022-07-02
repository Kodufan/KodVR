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

					renderer.mesh = sphereMesh;

					root.AttachPlane(UnityEngine.Color.grey);
					break;
				case WorldType.Space:
					root.AttachPlane(UnityEngine.Color.black);
					break;
				case WorldType.Gridspace:

					cube = new Slot("Cube", this);
					cube.SetParent(root);
					tex = cube.AttachComponent<Texture2D>();
					tex.uri = new System.Uri("C:\\Users\\koduf\\Desktop\\Memes\\718c6523d13d52ea0d5decf15988d119d2d24305a72b1e680f5acb24e943295d_1.png");

					material = cube.AttachComponent<PBS_Metallic>();
					material.texture = tex;

					sphereMesh = cube.AttachComponent<ProceduralSphereMesh>();
					boxMesh = cube.AttachComponent<ProceduralBoxMesh>();
					renderer = cube.AttachComponent<MeshRenderer>();
					renderer.material = material;
					renderer.mesh = boxMesh;
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