// Title        : CameraController.cs
// Purpose      : Allows player to control the game camera
// Author       : exoticCentipede
// Date         : 18/08/2016

using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

  // Consts
  private const string HORIZONTAL = "Horizontal";
  private const string VERTICAL   = "Vertical";
  private const string ZOOM       = "Zoom";

  // Speeds
  private float scrollSpeed = 2.0f;            // Speed of camera movement

  // Misc
  private int scrollBorderLimit = 15;           // How close to border for scroll

	void Start ()
  {
	
	} // Start()
	

	void Update ()
  { // Check player input for camera movement

    // Get mouse position
    float mouseX = Input.mousePosition.x;
    float mouseY = Input.mousePosition.y;

    // Check mouse position for movement
    if (mouseX < scrollBorderLimit)                 // Check mouse left
      Move(HORIZONTAL, -5);
    if (mouseX > Screen.width - scrollBorderLimit)  // Check mouse right
      Move(HORIZONTAL, 5);
    if (mouseY > Screen.height - scrollBorderLimit) // Check mouse up
      Move(VERTICAL, 5);
    if (mouseY < scrollBorderLimit)                 // Check mouse down
      Move(VERTICAL, -5);

    // Check keyboard for movement
    if (Input.GetAxis(HORIZONTAL) < 0)                // Check left movement
      Move(HORIZONTAL, Input.GetAxis(HORIZONTAL));
    if (Input.GetAxis(HORIZONTAL) > 0)                // Check right movement
      Move(HORIZONTAL, Input.GetAxis(HORIZONTAL));
    if (Input.GetAxis(VERTICAL) < 0)                  // Check down movement
      Move(VERTICAL, Input.GetAxis(VERTICAL));
    if (Input.GetAxis(VERTICAL) > 0)                  // Check up movement
      Move(VERTICAL, Input.GetAxis(VERTICAL));

    // Zoom Camera in or out
    if (Input.GetAxis("Mouse ScrollWheel") < 0)
    {
      Move(ZOOM, 0.2f);
    }
    if (Input.GetAxis("Mouse ScrollWheel") > 0)
    {
      Move(ZOOM, -0.2f);
    }

  } // Update()


  void Move(string direction, float speed)
  { // Process movement of the camera, check if movement is in bounds, if so, 
    // translate camera

    Vector3 newPos = new Vector3(0, 0, 0);

    switch (direction)
    {
      case (HORIZONTAL):
        //Vector3 horiTarget = new Vector3(speed, 0, 0);
        newPos = new Vector3(speed, 0 ,0) * Time.deltaTime * scrollSpeed;
        break;

      case (VERTICAL):
        //Vector3 vertTarget = new Vector3(0, 0, speed);
        newPos = new Vector3(0, 0, speed) * Time.deltaTime * scrollSpeed;
        break;

      case (ZOOM):
        Vector3 target = new Vector3(0, speed, 0);
        newPos = target;
        break;
    }

    transform.Translate(newPos, Space.World);

  } // Move()

} // CameraController
