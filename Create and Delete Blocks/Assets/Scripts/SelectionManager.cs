using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    // Materials to assign when highlighted
    [SerializeField] private Material highlightMaterial;

    // Pre highlight material
    [SerializeField] private Material previousMaterial;

    // Enter/exit edit mode
    [SerializeField] public bool editMode;

    // Enter/exit scale mode
    [SerializeField] public bool scaleMode;

    // Enter/exit colour mode
    [SerializeField] private bool colourMode;

    // Current highlight
    private Transform highlighted;

    // Main Camera
    public Camera cam;

    // Selected object (different to highlighted)
    public Transform selection;

    // Colour picker
    public GameObject colourPickerPrefab;

    // Pick colour triangle
    private ColorPickerTriangle colourPickerTriangle;

    // Object of colour picker
    private GameObject go;

    // Material of object to paint 
    private Material mat;

    // Manager object
    GameObject managers;

    // Change colour script
    ChangeColour changeColour;

    // GodMode
    bool GodMode;

    // Camera (Cinemachine)
    public GameObject vCam;

    // Free camera component
    public FreeCamera Freecam;

    // Start is called before the first frame update
    void Start()
    {
        // Set bools to starting states and assign game objects
        editMode = false;
        colourMode = false;
        managers = GameObject.Find("Managers");
        changeColour = managers.GetComponentInChildren<ChangeColour>();

        selection = null;

        // Get cinemachine Vcam and its component for FreeCamera
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        GodMode = Freecam.GetGodMode();

        if (GodMode)
        {
            // If nothing is highlighted set to previous material (once not hovering over)
            if (highlighted != null)
            {
                var selectedRenderer = highlighted.GetComponent<Renderer>();
                selectedRenderer.material = previousMaterial;
                highlighted = null;
            }

            // Ray for raycast 
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast and if hit highlight/select object
            if (Physics.Raycast(ray, out hit, 100))
            {
                // hit object transform (component) = selected
                var selected = hit.transform;

                // If an object is highlighted and not in colour mode get renderer (component) for selection
                if (selected.GetComponent<Renderer>() != null && !colourMode)
                {
                    var selectedRenderer = selected.GetComponent<Renderer>();
                    if (selected != null)
                    {
                        // Save current material (for restoration after highlighting)
                        previousMaterial = selectedRenderer.material;

                        // Set hit object rendered material to highlight material
                        selectedRenderer.material = highlightMaterial;

                        // If an object is highlighted and middle mouse pressed, enter object edit mode
                        if (Input.GetMouseButtonDown(2))
                        {
                            // Set selection to highlighted object (for parsing)
                            editMode = (editMode == false ? editMode = true : editMode = false);
                        }
                        else
                        {
                            // If not editing an object clear selection
                            selection = null;
                        }

                        if (editMode)
                        {
                            // Set selection to highlighted object (for parsing)
                            selection = selected;

                            if (Input.GetKeyDown(KeyCode.Q))
                            {
                                scaleMode = (scaleMode == false ? scaleMode = true : scaleMode = false);
                            }
                            // If in edit mode and not colouring object, allow user to scale, rotate and move the object
                            if (!colourMode)
                            {
                                // If not in scale mode, rotate and move the object (To not interfer with axis lock)
                                if (!scaleMode)
                                {
                                    RotateObject(selected);
                                    MoveObject(selected);
                                }
                                else
                                {
                                    Scale(selected);
                                }
                            }
                        }
                    }

                    // If in colour mode set material to object material (not the highlight material)
                    if (colourMode)
                    {
                        selectedRenderer.material = previousMaterial;
                    }
                    highlighted = selected;
                }
            }

            // If in edit mode check for colour mode input
            if (editMode)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // If not in colour mode, unlock and show cursor
                    if (!colourMode)
                    {
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = true;

                        // Start colouring
                        changeColour.StartPaint();
                        colourMode = true;
                    }
                    // If in colour mode, lock and hide cursor
                    else
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;

                        // Stop Colouring
                        changeColour.StopPaint();
                        editMode = false;
                        colourMode = false;
                    }
                }
            }
            if (!editMode)
            {
                scaleMode = false;
            }
        }

        // Exit GodMode resets
        if (!GodMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            editMode = GodMode;

            changeColour.StopPaint();

            if (selection != null)
            {
                selection.GetComponent<Renderer>().material = previousMaterial;
            }

            if (highlighted != null)
            {
                highlighted.GetComponent<Renderer>().material = previousMaterial;
            }
        }
    }

    // Rotate object
    void RotateObject(Transform selected)
    {
        float rotX = 0;
        float rotY = 0;

        // Rotation change = mouse axis input (multiplied by constant for speed control)
        rotY = Input.GetAxis("Mouse Y") * 50 * Mathf.Deg2Rad;
        rotX = Input.GetAxis("Mouse X") * 50 * Mathf.Deg2Rad;

        // Lock rotation axis based on user input
        if (Input.GetKey(KeyCode.X))
        {
            selected.transform.localEulerAngles += (cam.transform.up * -rotX);
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            selected.transform.localEulerAngles += (cam.transform.forward * rotY);
        }

        // Rotate freely
        else
        {
            selected.transform.localEulerAngles += (cam.transform.forward * rotY);
            selected.transform.localEulerAngles += (cam.transform.up * -rotX);
        }
    }

    // Move object based on input 
    void MoveObject(Transform selected)
    {
        // Move +x
        if (Input.GetKey(KeyCode.D))
        {
            selected.position += new Vector3(0.01f, 0);
        }

        // Move -x
        if (Input.GetKey(KeyCode.A))
        {
            selected.position += new Vector3(-0.01f, 0);
        }

        // Move +y
        if (Input.GetKey(KeyCode.Space))
        {
            selected.position += new Vector3(0, 0.01f);
        }

        // Move -y
        if (Input.GetKey(KeyCode.LeftShift))
        {
            selected.position += new Vector3(0, -0.01f);
        }

        // Move -z
        if (Input.GetKey(KeyCode.S))
        {
            selected.position += new Vector3(0, 0, -0.01f);
        }

        // Move +z
        if (Input.GetKey(KeyCode.W))
        {
            selected.position += new Vector3(0, 0, 0.01f);
        }
    }

    // Scale object
    void Scale(Transform selected)
    {
        // Scale rate to control speed
        float scaleRate = 5;

        // Mouse scrollwheel used to scale
        var scale = Input.GetAxis("Mouse ScrollWheel");

        // if getting scrollwheel input
        if (scale != 0)
        {
            // Store temporary scale (cannot isolate axis in active object)
            var tempScale = selected.localScale;

            // Lock scale to x if user holding x
            if (Input.GetKey(KeyCode.X))
            {
                tempScale.x += tempScale.x * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                selected.localScale = tempScale;
            }
            else
            // Lock scale to y if user holding y
            if (Input.GetKey(KeyCode.Y))
            {
                tempScale.y += tempScale.y * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                selected.localScale = tempScale;
            }
            else
            // Lock scale to y if user holding Z
            if (Input.GetKey(KeyCode.Z))
            {
                tempScale.z += tempScale.z * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                selected.localScale = tempScale;
            }
            // Scale uniformly
            else
            {
                selected.localScale += selected.localScale * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
            }
        }
    }

    // Returns edit mode
    public bool GetEditMode()
    {
        return editMode;
    }

    // Returns selected object
    public Transform GetSelected()
    {
        return selection;
    }

    // Returns selected material
    public Material GetMaterial()
    {
        return previousMaterial;
    }

    // Returns highlighted (for UI)
    public Transform GetHighlight()
    {
        return highlighted;
    }

    public bool GetScaleMode()
    {
        return scaleMode;
    }

    public bool GetColourMode()
    {
        return colourMode;
    }
}
