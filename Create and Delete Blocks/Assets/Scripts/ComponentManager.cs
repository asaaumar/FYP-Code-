using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectManager))]
public class ComponentManager : MonoBehaviour
{
    public Camera cam;
    ObjectManager objectManager;

    // Camera (Cinemachine)
    public GameObject vCam;

    // Free camera component
    public FreeCamera Freecam;

    // Start is called before the first frame update
    void Start()
    {
        objectManager = GetComponent<ObjectManager>();


        // Get cinemachine Vcam and its component for FreeCamera
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();
    }

    // Add/remove rigidBody component 
    void RigidBodyMod()
    {
        // Use raycast to choose object
        GameObject toMod = null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyUp(KeyCode.F))
        {
            // If hit turn the hit into a game object
            if (Physics.Raycast(ray, out hit, 100))
            {
                toMod = hit.transform.gameObject;

                // If object has a rigid body toggle gravity effect
                if (toMod.GetComponent<Rigidbody>() != null)
                {
                    if (toMod.GetComponent<Rigidbody>().isKinematic == true && !objectManager.movingObjects.Contains(toMod))
                    {
                        objectManager.SetMoving(toMod);
                    }
                    else
                    {
                        objectManager.RemoveMoving(toMod);
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Freecam.GetGodMode())
        {
            RigidBodyMod();
        }
    }
}
