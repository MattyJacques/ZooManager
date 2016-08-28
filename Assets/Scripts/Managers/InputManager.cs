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
		// Objects
		public Terrain terrain;

        // Defines
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private const string ZOOM = "Zoom";

        // Bounds
        private const float MINCAMERAHEIGHT = 5f;
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
			if (terrain == null) {
				terrain = GameObject.FindObjectOfType<Terrain> ();
			}
		} // Start()


        private void Update()
        {
            // Check player input for camera movement

            // Get mouse position
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            Vector3 movement = Vector3.zero;

            if (mouseX < SCROLLBORDERLIMIT) // Check mouse left
                movement.x -= 1f;
			if (mouseX > Screen.width - SCROLLBORDERLIMIT) // Check mouse right
                movement.x += 1f;
            if (mouseY > Screen.height - SCROLLBORDERLIMIT) // Check mouse up
                movement.z += 1f;
            if (mouseY < SCROLLBORDERLIMIT) // Check mouse down
                movement.z -= 1f;

			// Check keyboard for movement
			movement += new Vector3 (Input.GetAxis (HORIZONTAL), 0, Input.GetAxis (VERTICAL));

            // Zoom Camera in or out
			movement += new Vector3 (0, -Input.GetAxis("Mouse ScrollWheel"), 0);

            if (movement != Vector3.zero)
                Move(movement);


            // Check for rotation
			if (Input.GetMouseButton (1)) {
				// Lock cursor whilst rotating camera
				Cursor.lockState = CursorLockMode.Locked;
				Rotate (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
			} else {
				Cursor.lockState = CursorLockMode.Confined;
			}

        } // Update()


        private void Move(Vector3 movement)
        {

            float yRot = Camera.main.transform.eulerAngles.y;

            Vector3 newPos = new Vector3(
                Mathf.Cos(yRot*Mathf.Deg2Rad)*movement.x + Mathf.Sin(yRot*Mathf.Deg2Rad)*movement.z,
                movement.y,
                Mathf.Cos(yRot*Mathf.Deg2Rad)*movement.z - Mathf.Sin(yRot*Mathf.Deg2Rad)*movement.x
                );


			// Apply a speed multipler based on camera height
			float heightMultipler = Mathf.Pow(((transform.position.y - MINCAMERAHEIGHT) / (MAXCAMERAHEIGHT - MINCAMERAHEIGHT)) + 1f, 2f);
			transform.position = Vector3.Lerp (transform.position, transform.position + (newPos * heightMultipler), Time.deltaTime * 5f);

			// Calculate distance from camera position to y0 through the camera forward
			float lookDistance = transform.position.y / (Mathf.Cos((90f - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad));

			float clampedX = Mathf.Clamp (
				transform.position.x, 
				(-transform.forward * lookDistance).x, 
				(terrain.terrainData.size + (-transform.forward * lookDistance)).x);

			float clampedZ = Mathf.Clamp (
				transform.position.z, 
				(-transform.forward * lookDistance).z,
				(terrain.terrainData.size + (-transform.forward * lookDistance)).z);

			float clampedY = Mathf.Clamp (
                 transform.position.y,
                 MINCAMERAHEIGHT,
                 MAXCAMERAHEIGHT);

			transform.position = new Vector3 (clampedX, clampedY, clampedZ);

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