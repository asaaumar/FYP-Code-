using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    // Character controller component 
    public CharacterController Controller;

    // Move speed
    public float speed = 8f;

    // Gravity value
    public float gravity = -9.81f;

    // Velocity (vector)
    Vector3 velocity;

    // Floor check object
    public Transform feet;

    // Ground check sphere radius
    public float groundDist;

    // Check if hitting object not player
    public LayerMask groundMask;

    bool grounded;

    // jump height
    public float jumpHeight = 2f;

    // Camera (Cinemachine)
    public GameObject vCam;
    
    // Free camera component
    public FreeCamera Freecam;

    private void Start()
    {
        // Get cinemachine Vcam and its component for FreeCamera
        vCam = GameObject.Find("CM vcam1");
        Freecam = vCam.GetComponent<FreeCamera>();
    }

    public void MovePlayer()
    {
        grounded = Physics.CheckSphere(feet.position, groundDist, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        vCam.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Camera.main.transform.right * x + Camera.main.transform.forward * z;

        Controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }
}
