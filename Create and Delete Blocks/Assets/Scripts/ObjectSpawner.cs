using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(ObjectManager))]
public class ObjectSpawner : MonoBehaviour
{
    // Main camera (freecam)
    public Camera cam;

    public bool show;

    // Min placeable distance from camera
    public float minDist = 1;

    // Max placeable distance from camera
    public float maxDist = 20;

    public float initDist = 10;

    // Select last object from list
    public KeyCode previousElem = KeyCode.Z;

    // Select nect object from list 
    public KeyCode nextElem = KeyCode.C;

    // List of spawnable objects
    public List<GameObject> objects;

    // List of objects in scene
    public List<GameObject> objectsInScene;

    // Object maanager object
    ObjectManager objectManager;

    // Selection manager object 
    public SelectionManager selectionManager;

    // Link manager object
    public LinkObjects linkManager;

    // List of placeholder objects (same as objects but different mat)
    List<GameObject> placeholders;

    public Material outlineMat;

    // Index of list of objects
    int objectIndex = 0;

    int scrollSpeed = 10;

    // Parent object for transforms
    GameObject parent;

    // Boolean to show placeholders
    public bool showPlace;

    // Scroll distance
    float distance;

    // Current object pos and rotation
    Vector3 currentPos;
    Quaternion currentRot;

    // Time delay
    float delay;

    // Camera (Cinemachine)
    public GameObject vCam;

    // Free camera component
    public FreeCamera Freecam;

    // Start is called before the first frame update
    void Start()
    {
        // Assign game objects
        selectionManager = GetComponentInChildren<SelectionManager>();
        objectManager = GetComponent<ObjectManager>();
        linkManager = GetComponentInChildren<LinkObjects>();

        // Get cinemachine Vcam and its component for FreeCamera
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();

        // Assign initial distance
        distance = initDist;

        placeholders = new List<GameObject>();

        // Load placeholder objects
        SpawnPlaceholders();

        // Calculate placeholder position
        CalculatePos();

        delay = 0;

        // Add cinemachine listener
        CinemachineCore.CameraUpdatedEvent.AddListener(UpdateObjectPos);
    }

    // Update is called once per frame
    void Update()
    {
        // If in GodMode
        // If in edit mode or link mode dont show placeholders or perform place or delete actions
        if (Freecam.GetGodMode())
        {
            if (selectionManager.GetEditMode())
            {
                showPlace = false;
            }
            else if (linkManager.GetModes())
            {
                showPlace = false;

                // Create delay between link mode and ability to place blocks
                delay = 0.5f;
            }
            else
            {
                showPlace = true;
            }
        }
        else
        {
            showPlace = false;
        }

        showMarker(showPlace);

        // If in build mode (showing placeholders)
        if (showPlace)
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                {
                    // Mouse wheel input
                    var dist = Input.GetAxis("Mouse ScrollWheel");

                    // If getting input, change distance based on input (within constraints)
                    if (dist != 0)
                    {
                        distance += Time.deltaTime * (dist > 0 ? +scrollSpeed : -scrollSpeed);
                    }

                    distance = Mathf.Clamp(distance, minDist, maxDist);

                    // Activate current placeholder
                    placeholders[objectIndex].SetActive(true);

                    // Place object if left mouse is pressed
                    if (Input.GetMouseButtonUp(0))
                    {
                        var toSpawn = Instantiate(objects[objectIndex], currentPos, currentRot);
                        AddObject(toSpawn);
                    }

                    // Raycast and delete object if right mouse is pressed
                    else if (Input.GetMouseButtonUp(1))
                    {
                        RaycastHit hit;

                        // Raycast to mouse position (locked centre)
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                        // If hit: check and destroy
                        if (Physics.Raycast(ray, out hit, 100))
                        {
                            objectManager.DestroyObject(hit.transform.gameObject);
                            objectManager.RemoveMoving(hit.transform.gameObject);
                        }
                    }


                    // Cycle placeholders
                    if (Input.GetKeyDown(previousElem))
                        CyclePlaceHolders(false);
                    else if (Input.GetKeyDown(nextElem))
                        CyclePlaceHolders(true);
                }
            }
        }
    }

    // Recalculate position and rotation for object spawning
    void CalculatePos()
    {
        // Position
        currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));

        // Rotation (0.001f assigned to stop unity bug)
        currentRot = Quaternion.LookRotation(new Vector3(0.001f, 0, 0));
    }

    // Add all children of an object with a rigid body to spawned objects 
    void AddObject(GameObject spawned)
    {
        var children = spawned.GetComponentsInChildren<Rigidbody>();

        foreach (var c in children)
        {
            objectManager.AddObject(c.gameObject);
        }
    }


    // Update current object pos
    void UpdateObjectPos(CinemachineBrain brain)
    {
        CalculatePos();
        // Update parent object pos (so as to maintain local positions)
        parent.transform.position = currentPos;
        parent.transform.rotation = currentRot;
    }

    // Spawn placeholders (no rigid body or colliders)
    void SpawnPlaceholders()
    {
        parent = new GameObject();
        parent.name = "Placed";

        for (int i = 0; i < objects.Count; i++)
        {
            // Instantiate and deactivate the object
            GameObject temp = Instantiate(objects[i], parent.transform);
            temp.SetActive(false);

            // Remove colliders 
            var colliders = temp.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
                Destroy(collider);

            // Remove rigidbodies
            var rigidBodies = temp.GetComponentsInChildren<Rigidbody>();
            foreach (var rigidBody in rigidBodies)
                Destroy(rigidBody);

            // Render mesh and materials
            var meshRenderers = temp.GetComponentsInChildren<Renderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = outlineMat;

                // Turn off shadow
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            placeholders.Add(temp);
        }
    }


    // Cycle through placeholders (selection)
    void CyclePlaceHolders(bool increment)
    {
        // Deactivate current object
        placeholders[objectIndex].SetActive(false);

        // Change placeholder
        if (increment)
        {
            objectIndex++;
            if (objectIndex == placeholders.Count)
                objectIndex = 0;
        }
        else
        {
            objectIndex--;
            if (objectIndex < 0)
                objectIndex = placeholders.Count - 1;
        }

        // Activate current object
        placeholders[objectIndex].SetActive(true);
    }

    void OnDestroy()
    {
        // Remove listener for spawner
        CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdateObjectPos);
    }

    // Show/hide placeholders
    public void showMarker(bool show)
    {
        // Change alpha value based on parameter
        var mat = outlineMat.color;
        mat.a = show ? 0.5f : 0f;
        outlineMat.color = mat;
    }
}
