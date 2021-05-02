using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private void Update()
    {

        float rotX;
        float rotY;

        // if middlouse button pressed and mouse moved: rotate object 
        if (Input.GetMouseButton(2))
        {
            rotY = Input.GetAxis("Mouse Y") * 50 * Mathf.Deg2Rad;
            rotX = Input.GetAxis("Mouse X") * 50 * Mathf.Deg2Rad;

            transform.localEulerAngles += (Vector3.up * -rotX);
            transform.localEulerAngles += (Vector3.right * rotY);
            transform.localEulerAngles += (Vector3.forward * -rotX);
        }
        else
        {
            // stop rotation
            rotY = 0;
            rotX = 0;
        }
    }
}
