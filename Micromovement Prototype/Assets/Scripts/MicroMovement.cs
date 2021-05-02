using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.01f, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.01f, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, 0.01f);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(0, -0.01f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -0.01f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, 0.01f);
        }
    }
}
