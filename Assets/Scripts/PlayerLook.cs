using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Sensitivity of X axis movement
    [SerializeField] private float sensX;
    // Sensitivity of Y axis movement
    [SerializeField] private float sensY;

    // Camera
    [SerializeField] Transform cam;
    // Orientation of camera
    [SerializeField] Transform orientation;

    // Wall running script
    [SerializeField] WallRun wallRun;

    // X axis
    float mouseX;
    // Y axis
    float mouseY;
    float multiplier = 0.01f;

    // X rotation magnitude
    float xRotation;
    // Y rotation magnitude
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Locks mouse cursor inside game screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        // Applies changes to camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void MyInput()
    {
        // Grabs x and y axis values from mouse movement
        mouseX = Input.GetAxisRaw("Mouse X");
        // 0, -1, 1
        mouseY = Input.GetAxisRaw("Mouse Y");
        
        // Adds magnitude to values
        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        // Limits the player's ability to look "up" and "down",
        // so they can't do a "360"
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
