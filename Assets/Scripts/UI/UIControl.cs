// Title        : UIControl
// Purpose      : Lets the user control things using the UI
// Author       : Alexander Falk
// Date         : 21/08/2017

using UnityEngine;

public class UIControl : MonoBehaviour
{
  private RtsCamera cameramain;
  private RtsCameraMouse cameramouse;
  private RtsCameraKeys camerakeys;

  private bool zoomIn;
  private bool zoomOut;
  private bool rotateLeft;
  private bool rotateRight;

  void Start()
  { // Loads in the camera elements to be used
    cameramain = GameObject.Find("Main Camera").GetComponent<RtsCamera>();
    camerakeys = GameObject.Find("Main Camera").GetComponent<RtsCameraKeys>();

  } // Start()

  void Update()
  {
    CameraControls();
  } // Update()

  void CameraControls()
  { // Camera Control Functions
    if (zoomIn && camerakeys.AllowZoom)
    {
      cameramain.Distance -= camerakeys.ZoomSpeed * Time.deltaTime;
    }

    if (zoomOut && camerakeys.AllowZoom)
    {
      cameramain.Distance += camerakeys.ZoomSpeed * Time.deltaTime;
    }

    if (rotateLeft)
    {
      cameramain.Rotation += camerakeys.RotateSpeed * Time.deltaTime;
    }

    if (rotateRight)
    {
      cameramain.Rotation -= camerakeys.RotateSpeed * Time.deltaTime;
    }
  } // CameraControls()

  public void ZoomIn()
  {
    zoomIn = true;
  } // ZoomIn()

  public void ZoomOut()
  {
    zoomOut = true;
  } // ZoomOut()

  public void RotateLeft()
  {
    rotateLeft = true;
  } // RotateLeft()

  public void RotateRight()
  {
    rotateRight = true;
  } // RotateRight()

  public void Stop()
  {
    zoomIn = false;
    zoomOut = false;
    rotateLeft = false;
    rotateRight = false;
  } // Stop()
}





