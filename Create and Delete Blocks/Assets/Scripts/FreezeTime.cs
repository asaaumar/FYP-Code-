using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectManager))]
public class FreezeTime : MonoBehaviour
{
    // Only works for objects user has placed
    ObjectManager objectManager;

    // Frozen variable 
    bool frozen = true;

    // Frozen image
    public Image frozenImage;

    // Double tap check time
    float DT;

    // First press or second
    private bool firstPress;

    // Reset timer
    private bool reset;

    // Start is called before the first frame update
    void Start()
    {
        objectManager = GetComponent<ObjectManager>();
        DT = 0;
        firstPress = false;
        reset = false;
        ShowImage(frozen);
    }

    void ChangeTime()
    {
        // Set isKinematic to true or false for 'moving' objects (those assigned to move with gravity)
        if (frozen)
        {
            foreach (GameObject o in objectManager.movingObjects)
            {
                o.GetComponent<Rigidbody>().isKinematic = true;
            }
            // Show icon
           ShowImage(frozen);
            frozen = false;
        } else
        {
            foreach (GameObject o in objectManager.movingObjects)
            {
                o.GetComponent<Rigidbody>().isKinematic = false;
            }
            // Hide icon
            ShowImage(frozen);
            frozen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input and call time freeze/unfreeze
        // Double press check, if first press has already happened within 0.5 seconds change time mode
        if (Input.GetKeyDown(KeyCode.Space) && firstPress)
        {
            if (Time.time - DT < 0.5)
            {
                ChangeTime();
            }

            reset = true;
        }

        // If first press has not happened save time of press
        if (Input.GetKeyDown(KeyCode.Space) && !firstPress)
        {
            firstPress = true;
            DT = Time.time;
        }

        // If double pressed, reset variables
        if (reset)
        {
            firstPress = false;
            reset = false;
        }
    }

    
    // Toggle pause image
    void ShowImage(bool f)
    {
        // Change icon alpha
        var frozenColour = frozenImage.color;
        frozenColour.a = f ? 1f : 0f;
        frozenImage.color = frozenColour;
    }
 
}
