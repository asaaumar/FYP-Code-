using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    /// <summary>
    /// Normal speed of camera movement.
    /// </summary>
    public float movementSpeed = 5f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;

    // Managers object
    public GameObject managers;

    // Selection manager
    public SelectionManager selectionManager;

    // God Mode (Create mode)
    public bool godMode;

    // Player GameObject
    public GameObject player;

    // Player controller
    FPSController FPSController;

    // Lock camera
    public bool lockCamera;

    private void Start()
    {
        // Assign game objects
        managers = GameObject.Find("Managers");
        selectionManager = managers.GetComponentInChildren<SelectionManager>();

        player = GameObject.Find("Player");
        FPSController = player.GetComponent<FPSController>();
        godMode = true;

        // Hide and lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // lock camera mode = edit mode
        lockCamera = selectionManager.GetEditMode();

        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
        }

        if (godMode)
        {
            // If camera is free
            if (!lockCamera)
            {
                // If A pressed move left
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
                }

                // If D pressed move right
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
                }

                // If W pressed move forwards
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
                }

                // If S pressed move back
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
                }

                // If space pressed move up
                if (Input.GetKey(KeyCode.Space))
                {
                    transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime);
                }

                // If left shift pressed move down
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    transform.position = transform.position + (-transform.up * movementSpeed * Time.deltaTime);
                }
            }
            player.transform.position = transform.position;
        }
        else
        {
            FPSController.MovePlayer();
        }

        // Set camera rotation = mouse input about X and Y
        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
        transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);

        // If camera is locked (in edit mode), look at object to edit
        if (lockCamera)
        {
            transform.LookAt(selectionManager.GetSelected());
        }
    }

    // Returns godMode
    public bool GetGodMode()
    {
        return godMode;
    }
}
