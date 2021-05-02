using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLinks : MonoBehaviour
{
    int count = 0;

    public bool linkMode;

    public bool unlink;

    public Transform parent;

    public Transform child;

    private void Start()
    {
        linkMode = false;

        parent = null;

        child = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
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

        if (linkMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (count == 0)
                    {
                        child = hit.transform;
                        count = 1;
                    }
                    else
                    if (count == 1)
                    {
                        parent = hit.transform;
                        count = 2;
                    }
                }
            }

            if (count == 2)
            {
                Link(parent, child);
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            unlink = !unlink;
        }

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

    // Take two objects as parameters and parent one to link transforms
    public void Link(Transform parent, Transform child)
    {
        child.parent = parent;
        count = 0;
        this.parent = null;
        this.child = null;
        linkMode = false;
    }

    public void Unlink(Transform child)
    {
        if (child.parent != null)
        {
            child.parent = null;
        }
        this.child = null;
        unlink = false;
    }
}
