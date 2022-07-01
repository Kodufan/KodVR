using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KodEngine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		WorldManager worldManager = new WorldManager(gameObject);

		worldManager.LoadDefaultWorld();
		worldManager.LoadWorld(WorldTypes.Gridspace);
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			Debug.Log("Focusing world 1...");
			WorldManager.FocusWorld(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("Focusing on world 2...");
			WorldManager.FocusWorld(1);
		} else if (Input.GetKeyDown(KeyCode.Delete))
		{
			Debug.Log("Deleting slots in world 1...");
			WorldManager.FocusWorld(0);
			WorldManager.focusedWorld.root.GetChild(0).destroy();
		}
	}
}
