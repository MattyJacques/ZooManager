// Title        : CameraController.cs
// Purpose      : Allows player to control the game camera
// Author       : exoticCentipede
// Date         : 18/08/2016

using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        // Defines
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private const string ZOOM = "Zoom";

        // Bounds
        private const float MINCAMERAHEIGHT = 10f;
        private const float MAXCAMERAHEIGHT = 40f;
        private float _minCameraX;
        private float _maxCameraX;
        private float _minCameraZ;
        private float _maxCameraZ;

        // Speeds
        private const float SCROLLSPEED = 2.0f; // Speed of camera movement
        private const float ROTATESPEED = 50f; // Rotatation speed of camera

        // Misc
        private const int SCROLLBORDERLIMIT = 15; // How close to border for scroll
        
        private void Start()
        {

        } // Start()


        private void Update()
        {
            // Check player input for camera movement

            // Get mouse position
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            Vector3 movement = Vector3.zero;

            // Check mouse position for movement
            if (mouseX < SCROLLBORDERLIMIT) // Check mouse left
                movement.x = -0.1f;
            if (mouseX > Screen.width - SCROLLBORDERLIMIT) // Check mouse right
                movement.x = 0.1f;
            if (mouseY > Screen.height - SCROLLBORDERLIMIT) // Check mouse up
                movement.z = 0.1f;
            if (mouseY < SCROLLBORDERLIMIT) // Check mouse down
                movement.z = -0.1f;

            // Check keyboard for movement
            if (Input.GetAxis(HORIZONTAL) < 0) // Check left movement
                movement.x = -0.1f;
            if (Input.GetAxis(HORIZONTAL) > 0) // Check right movement
                movement.x = 0.1f;
            if (Input.GetAxis(VERTICAL) < 0) // Check down movement
                movement.z = -0.1f;
            if (Input.GetAxis(VERTICAL) > 0) // Check up movement
                movement.z = 0.1f;

            // Zoom Camera in or out
            if (Input.GetAxis("Mouse ScrollWheel") < 0) // Check zoom out
                movement.y = 0.2f;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) // Check zoom in
                movement.y = -0.2f;

            if (movement != Vector3.zero)
                Move(movement);


            // Check for rotation
            if (Input.GetMouseButton(1))
            {
                Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

        } // Update()


        private void Move(Vector3 movement)
        {
            // Process movement of the camera, check if movement is in bounds, if so, 
            // translate camera


            float yRot = Camera.main.transform.eulerAngles.y;

            Vector3 newPos = new Vector3(
                Mathf.Cos(yRot*Mathf.Deg2Rad)*movement.x + Mathf.Sin(yRot*Mathf.Deg2Rad)*movement.z,
                movement.y,
                Mathf.Cos(yRot*Mathf.Deg2Rad)*movement.z - Mathf.Sin(yRot*Mathf.Deg2Rad)*movement.x
                );

            // Translate using local axes
            transform.Translate(newPos, Space.World);

        } // Move()


        private void Rotate(float xInput, float yInput)
        {
            // Rotate the camera to the desired angle

            Vector3 cameraAngle = Camera.main.transform.eulerAngles;

            cameraAngle.x -= yInput*ROTATESPEED;
            cameraAngle.y += xInput*ROTATESPEED;

            Camera.main.transform.eulerAngles = Vector3.MoveTowards(Camera.main.transform.eulerAngles,
                cameraAngle,
                Time.deltaTime*ROTATESPEED);

        } // Rotate()

    } // CameraController
}