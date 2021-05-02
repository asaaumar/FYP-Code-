using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkObjects : MonoBehaviour
{
    // Object counter
    int count = 0;

    // Linking objects?
    public bool linkMode;

    // Unlinking objects?
    public bool unlink;

    // Parent object (second click)
    public Transform parent;

    // Child object (first click)
    public Transform child;

    // Camera (Cinemachine)
    public GameObject vCam;

    // Free camera component
    public FreeCamera Freecam;

    bool GodMode;

    private void Start()
    {
        // Set default values
        linkMode = false;
        parent = null;
        child = null;

        // Get cinemachine Vcam and its component for FreeCamera
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();
    }

    private void Update()
    {
        GodMode = Freecam.GetGodMode();

        if (GodMode)
        {
            // Set linkMode based on input
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!linkMode)
                {
                    linkMode = true;
                }
                else
                {
                    linkMode = false;
                }
            }

            // If in link mode, raycast to get objects and link
            if (linkMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        // If first hit, set as child
                        if (count == 0)
                        {
                            child = hit.transform;
                            count = 1;
                        }
                        else

                        // If second hit, set as parent
                        if (count == 1)
                        {
                            parent = hit.transform;
                            count = 2;
                        }
                    }
                }

                // If 2 objects have been clicked, link them
                if (count == 2)
                {
                    Link(parent, child);
                }
            }

            // Unlink mode check
            if (Input.GetKeyDown(KeyCode.V))
            {
                unlink = !unlink;
            }

            // If unlinking, get raycast hit and set child as hit
            if (unlink)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        child = hit.transform;
                    }

                    Unlink(child);
                }
            }
        }

        if (!GodMode)
        {
            linkMode = false;
            unlink = false;
        }
    }

    // Take two objects as parameters and parent one to link transforms
    public void Link(Transform parent, Transform child)
    {
        // Set first click as child of second click
        child.parent = parent;

        // Reset variables for next linking operation
        count = 0;
        this.parent = null;
        this.child = null;
        linkMode = false;
    }

    public void Unlink(Transform child)
    {
        // If the object is linked, sever all links
        if (child.parent != null)
        {
            child.parent = null;
        }
        this.child = null;
        unlink = false;
    }

    // If linking or unlinking, return true
    public bool GetModes()
    {
        if (linkMode || unlink)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Return LinkMode
    public bool GetLinkMode()
    {
        return linkMode;
    }

    // Return Unlink
    public bool GetUnlink()
    {
        return unlink;
    }
}
