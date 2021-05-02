using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    public CharacterController Controller;

    public float speed = 12f;

    public float gravity = -9.81f;

    Vector3 velocity;

    public Transform feet;

    public float groundDist;

    public LayerMask groundMask;

    bool grounded;

    public float jumpHeight = 3f;

    public FreeCamera Freecam;

    private void Start()
    {
        Freecam = Camera.main.GetComponent<FreeCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovePlayer()
    {
        grounded = Physics.CheckSphere(feet.position, groundDist, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Camera.main.transform.position = transform.position + new Vector3(0, 1.6f, 0);
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
