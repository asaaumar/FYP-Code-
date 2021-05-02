using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    [SerializeField] bool scaleMode;

    private void Start()
    {
        scaleMode = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            scaleMode = (scaleMode == false ? scaleMode = true : scaleMode = false);
        }

        if (scaleMode)
        {
            Scale();
        }
    }

    void Scale()
    {
        float scaleRate = 5;
        var scale = Input.GetAxis("Mouse ScrollWheel");
        if (scale != 0)
        {
            var tempScale = transform.localScale;
            if (Input.GetKey(KeyCode.X))
            {
                tempScale.x += tempScale.x * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                transform.localScale = tempScale;
            }
            else
            if (Input.GetKey(KeyCode.Y))
            {
                tempScale.y += tempScale.y * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                transform.localScale = tempScale;
            }
            else
            if (Input.GetKey(KeyCode.Z))
            {
                tempScale.z += tempScale.z * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
                transform.localScale = tempScale;
            }
            else
            {
                transform.localScale += transform.localScale * Time.deltaTime * (scale > 0 ? +scaleRate : -scaleRate);
            }
        }
    }
}
