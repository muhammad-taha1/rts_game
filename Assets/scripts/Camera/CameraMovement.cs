using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Camera camera;

    public float edgeThreshold = 10f; // Distance from the screen edge to start moving
    public float scrollSpeed = 20f;   // Speed of camera movement

    public float cameraMoveOnPressSpeed = 20f;

    public float zoomSpeed = 10f;

    public int maxZoom = 500;
    public int minZoom = 300;

    public float rotationSpeed = 10f;


    public float minVerticalAngle = 50f; 
    public float maxVerticalAngle = 85f;  
    public float minHorizontalAngle = -90.0f; 
    public float maxHorizontalAngle = 90.0f;

    private bool isCameraMoveButtonHeld = false;
    private bool isRotating = false;

    private float currentVerticalAngle = 55f;
    private float currentHorizontalAngle = 0.0f;

    private bool shouldZoom = false;

    private void Start()
    {
        camera = Camera.main;

    }


    // Update is called once per frame
    void LateUpdate()
    {
        toggleCameraMovement();
        toggleZoom();

        if (isCameraMoveButtonHeld)
        {
            MoveCameraWhenCameraMoveDown();

        }
        else if (!isRotating)
        {
            ScrollCameraWhenMouseHitsEdge();
        }

    
        handleZoom();
        
        handleRotation();
    }

    private void handleRotation()
    {
        if (Input.GetButtonDown(KeyMappings.MouseWheel))
        {
            isRotating = true;
        }
        else if (Input.GetButtonUp(KeyMappings.MouseWheel)) { 
            isRotating = false; 
        }

        if (isRotating)
        {
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            float horizontalRotation = x * rotationSpeed * Time.deltaTime;
            float verticalRotation = y * rotationSpeed * Time.deltaTime;

            currentHorizontalAngle += horizontalRotation;
            currentVerticalAngle -= verticalRotation;

            // Clamp the rotation angles
            currentHorizontalAngle = Mathf.Clamp(currentHorizontalAngle, minHorizontalAngle, maxHorizontalAngle);
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

            // Apply the clamped rotation angles
            camera.transform.localRotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0.0f);
        }

    }

    private void handleZoom()
    {
        if (shouldZoom)
        {
            Vector3 cameraPosition = camera.transform.position;

            // can zoom in
            if (cameraPosition.y < maxZoom)
            {
                cameraPosition.y += -1 * Input.mouseScrollDelta.y * zoomSpeed;
            }

            // can zoom out
            else if (cameraPosition.y > minZoom)
            {
                cameraPosition.y += Input.mouseScrollDelta.y * zoomSpeed;
            }

            // change only if new position is bounded
            if (cameraPosition.y > minZoom && cameraPosition.y < maxZoom)
            {
                camera.transform.position = cameraPosition;
            }
        }

    }

    private void toggleCameraMovement()
    {
        if (Input.GetButtonDown(KeyMappings.RightClick))
        {
            isCameraMoveButtonHeld = true;
        }

        if (Input.GetButtonUp(KeyMappings.RightClick))
        {
            isCameraMoveButtonHeld = false;
        }
    }

    private void toggleZoom()
    {
        if (Input.GetButtonDown(KeyMappings.Control))
        {
            shouldZoom = true;
        }

        if (Input.GetButtonUp(KeyMappings.Control))
        {
            shouldZoom = false;
        }
    }


    private void MoveCameraWhenCameraMoveDown()
    {
        var x = Input.GetAxis("Mouse X");
        var z = Input.GetAxis("Mouse Y");

        Vector3 cameraPosition = camera.transform.position;

        cameraPosition.x += cameraMoveOnPressSpeed * x;

   
        cameraPosition.z += cameraMoveOnPressSpeed * z;
        

        camera.transform.position = cameraPosition;

    }

    private void ScrollCameraWhenMouseHitsEdge()
    {
        var mousePosition = Input.mousePosition;


        Vector3 cameraPosition = camera.transform.position;

        if (mousePosition.x >= Screen.width - edgeThreshold)
        {
            cameraPosition.x += scrollSpeed * Time.deltaTime;
        }
        if (mousePosition.x <= edgeThreshold)
        {
            cameraPosition.x -= scrollSpeed * Time.deltaTime;
        }
        if (mousePosition.y >= Screen.height - edgeThreshold)
        {
            cameraPosition.z += scrollSpeed * Time.deltaTime;
        }
        if (mousePosition.y <= edgeThreshold)
        {
            cameraPosition.z -= scrollSpeed * Time.deltaTime;
        }

        camera.transform.position = cameraPosition;
    }
}
