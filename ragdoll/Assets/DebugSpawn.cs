using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawn : MonoBehaviourExt
{
    public Transform spawnedUnit;
    bool spawned = false;

	void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.S) == true && spawned == false)
        {
            Instantiate(
                spawnedUnit, 
                new Vector2(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 
                spawnedUnit.rotation);
            spawned = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) == false && spawned == true)
        {
            spawned = false;
        }
	}
}