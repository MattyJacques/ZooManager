// Title        : CameraController.cs
// Purpose      : Allows player to control the game camera
// Author       : exoticCentipede
// Date         : 18/08/2016

using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour
{
  // Defines
  private const string HORIZONTAL = "Horizontal";
  private const string VERTICAL = "Vertical";
  private const string ZOOM = "Zoom";

  // Bounds
  private const float MINCAMERAHEIGHT = 10f;
  private const float MAXCAMERAHEIGHT = 40f;
  private float minCameraX;
  private float maxCameraX;
  private float minCameraZ;
  private float maxCameraZ;

  // Speeds
  private const float SCROLLSPEED = 2.0f;      // Speed of camera movement
  private const float ROTATESPEED = 50f;       // Rotatation speed of camera

  // Misc
  private const int SCROLLBORDERLIMIT = 15;    // How close to border for scroll


  void Start()
  {

  } // Start()


  void Update()
  { // Check player input for camera movement

    // Get mouse position
    float mouseX = Input.mousePosition.x;
    float mouseY = Input.mousePosition.y;

    // Check mouse position for movement
    if (mouseX < SCROLLBORDERLIMIT)                 // Check mouse left
      Move(HORIZONTAL, -5);
    if (mouseX > Screen.width - SCROLLBORDERLIMIT)  // Check mouse right
      Move(HORIZONTAL, 5);
    if (mouseY > Screen.height - SCROLLBORDERLIMIT) // Check mouse up
      Move(VERTICAL, 5);
    if (mouseY < SCROLLBORDERLIMIT)                 // Check mouse down
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
    if (Input.GetAxis("Mouse ScrollWheel") < 0)       // Check zoom out
      Move(ZOOM, 0.2f);
    if (Input.GetAxis("Mouse ScrollWheel") > 0)       // Check zoom in
      Move(ZOOM, -0.2f);

    // Check for rotation
    if (Input.GetMouseButton(1))
    {
      Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

  } // Update()


  void Move(string direction, float speed)
  { // Process movement of the camera, check if movement is in bounds, if so, 
    // translate camera

    Vector3 newPos = new Vector3(0, 0, 0);

    switch (direction)
    {
      case (HORIZONTAL):
        newPos = new Vector3(speed, 0, 0) * Time.deltaTime * SCROLLSPEED;
        break;

      case (VERTICAL):
        newPos = new Vector3(0, speed, 0) * Time.deltaTime * SCROLLSPEED;
        break;

      case (ZOOM):
        newPos = new Vector3(0, 0, -speed);
        break;
    }

    // Translate using local axes
    transform.Translate(newPos, Space.Self);

  } // Move()


  void Rotate(float xInput, float yInput)
  { // Rotate the camera to the desired angle

    Vector3 cameraAngle = Camera.main.transform.eulerAngles;

    cameraAngle.x -= yInput * ROTATESPEED;
    cameraAngle.y += xInput * ROTATESPEED;

    Camera.main.transform.eulerAngles = Vector3.MoveTowards(Camera.main.transform.eulerAngles, 
                                                            cameraAngle, 
                                                            Time.deltaTime * ROTATESPEED);

  } // Rotate()

} // CameraController