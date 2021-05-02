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

    // Lock camera
    public bool lockCamera;

    public bool godMode;

    public GameObject player;

    FPSController FPSController;

    private void Start()
    {
        player = GameObject.Find("Player");
        FPSController = player.GetComponent<FPSController>();
        godMode = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
        }

        if (godMode)
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

            player.transform.position = transform.position - new Vector3(0, 1.6f, 0);
        }
        else
        {
            FPSController.MovePlayer();
        }

        // Set camera rotation = mouse input about X and Y
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;

            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);

            // Hide and lock cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool GetGodMode()
    {
        return godMode;
    }
}

