using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Selection manager component
    public SelectionManager selectionManager;

    // Object manager component
    public ObjectManager objectManager;

    // Link manager
    public LinkObjects linkManager;

    // Camera (Cinemachine)
    public GameObject vCam;

    // Free camera component
    public FreeCamera Freecam;

    // Canvas group component for hiding UI
    public CanvasGroup UI;

    // Text Variables for UI
    public Text gravityText;
    public Text editModeText;
    public Text scaleModeText;
    public Text colourModeText;
    public Text linkModeText;
    public Text unlinkText;

    // Start is called before the first frame update
    void Start()
    {
        // Assign GameObjects and Components
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();
        selectionManager = GameObject.Find("Managers").GetComponentInChildren<SelectionManager>();
        objectManager = GameObject.Find("Managers").GetComponent<ObjectManager>();
        linkManager = GameObject.Find("Managers").GetComponentInChildren<LinkObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        // If in GodMode, show UI
        if (Freecam.GetGodMode())
        {
            UI.alpha = 1;
        }
        else
        {
            UI.alpha = 0;
        }
        
        // If highlighted object is affected by gravity display appropriate text
        if (selectionManager.GetHighlight() != null)
        {
            if (objectManager.movingObjects.Contains(selectionManager.GetHighlight().gameObject))
            {
                gravityText.text = "Gravity: Yes";
            }
            else
            {
                gravityText.text = "Gravity: No";
            }
        }
        else
        {
            gravityText.text = "";
        }

        // If in EditMode show text
        if (selectionManager.GetEditMode())
        {
            editModeText.text = "Edit Mode";
        }
        else
        {
            editModeText.text = "";
        }

        // If in ScaleMode show text
        if (selectionManager.GetScaleMode())
        {
            scaleModeText.text = "Scale Mode";
        }
        else
        {
            scaleModeText.text = "";
        }

        // If in ColourMode show text
        if (selectionManager.GetColourMode())
        {
            colourModeText.text = "Colour Mode";
        }
        else
        {
            colourModeText.text = "";
        }

        // If in link mode show text 
        if (linkManager.GetLinkMode())
        {
            linkModeText.text = "Link Mode";
        }
        else
        {
            linkModeText.text = "";
        }

        // If Unlinking 
        if (linkManager.GetUnlink())
        {
            unlinkText.text = "Click to unlink";
        }
        else
        {
            unlinkText.text = "";
        }

    }
}
