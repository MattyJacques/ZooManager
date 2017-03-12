using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : DraggableWindow.cs
// Purpose      : Allows Window To Be Dragged + Closed
// Author       : aimmmmmmmmm
// Date         : 26/01/2017
public class DraggableWindow : MonoBehaviour {
  private bool windowHeld = false;
  private Vector3 offset = new Vector3(0, 0, 0);
	
	// Update is called once per frame
	void Update () 
	{
    if(windowHeld){ this.GetComponent<RectTransform>().position = new Vector3 (Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y, 0); }
	}

	public void OnWindowClick()
	{
    if (windowHeld) { windowHeld = false; }
    else 
    { 
      windowHeld = true;
      offset = this.GetComponent<RectTransform> ().position - Input.mousePosition;
    }
	}

  public void CloseWindow()
  {
    Destroy (this.gameObject);
  }
}
