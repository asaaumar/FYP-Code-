using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    // Managers object
    public GameObject managers;

    // Selection manager
    public SelectionManager selectionManager;

    // Colour picker prefab
    public GameObject cpPrefab;

    // Colour picker triangle (component of picker object)
    private ColorPickerTriangle cpTri;

    // Colour picker object
    private GameObject pickerObject;

    // Material of object to change
    private Material mat;

    // Game object to colour
    GameObject toPaint;

    private void Start()
    {
        mat = null;
        // Assign game objects
        managers = GameObject.Find("Managers");
        selectionManager = managers.GetComponentInChildren<SelectionManager>();
    }

    private void Update()
    {
        // If an object is selected to be coloured
        if (mat != null)
        {
            // Update material colour
            mat.color = cpTri.TheColor;
        }
    }

    public void StartPaint()
    {
        // Get the selected objects material
        mat = selectionManager.GetMaterial();

        // Create colour picker object and draw it to screen
        pickerObject = (GameObject)Instantiate(cpPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 2) + (Camera.main.transform.right * -0.5f), Quaternion.identity);
        pickerObject.transform.localScale = pickerObject.transform.localScale * 0.4f;
        pickerObject.transform.LookAt(Camera.main.transform);
        cpTri = pickerObject.GetComponent<ColorPickerTriangle>();

        // Get colour based on input 
        cpTri.SetNewColor(mat.color);
    }

    public void StopPaint()
    {
        // Destroy colour picker object
        Destroy(pickerObject);
    }
}
