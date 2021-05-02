using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    public GameObject cpPrefab;

    private ColorPickerTriangle cpTri;

    private bool isPaint = false;

    private GameObject pickerObject;

    private Material mat;

    public Transform testTransform;

    private void Start()
    {
        testTransform = GameObject.Find("Cube").transform;
        mat = testTransform.gameObject.GetComponent<MeshRenderer>().material;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaint)
            {
                StopPaint();
            }
            else
            {
                StartPaint();
            }
        }

        if (isPaint)
        {
            mat.color = cpTri.TheColor;
        }
    }

    void StartPaint()
    {
        pickerObject = (GameObject)Instantiate(cpPrefab, testTransform.position + Vector3.up * 1.4f, Quaternion.identity);
        pickerObject.transform.localScale = Vector3.one * 1.3f;
        pickerObject.transform.LookAt(Camera.main.transform);
        cpTri = pickerObject.GetComponent<ColorPickerTriangle>();
        cpTri.SetNewColor(mat.color);
        isPaint = true;
    }

    private void StopPaint()
    {
        Destroy(pickerObject);
        isPaint = false;
    }
}
