using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // List of spawned objects
    public List<GameObject> spawned = new List<GameObject>();

    // List of moving objects
    public List<GameObject> movingObjects = new List<GameObject>();


    // Add to spawned list
    public void AddObject(GameObject o)
    {
        spawned.Add(o);
    }

    // Check if object is in list and destroy
    public bool DestroyObject(GameObject o)
    {
        // If removable
        if (spawned.Remove(o))
        {
            // Destroy object
            Destroy(o);
            return true;
        }
        return false;
    }

    // If set to move (via component manager) add to movingObjects
    public void SetMoving(GameObject o)
    {
        movingObjects.Add(o);
    }

    // If set to no longer move remove from movingObjects
    public void RemoveMoving(GameObject o)
    {
        movingObjects.Remove(o);
    }
}
