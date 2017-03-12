// Title        : CustomCursor.cs
// Purpose      : To set and change the cursor dynamically depending on user action
// Author       : Jeremy Mann
// Date         : 28/11/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {

	public Texture2D _idleCursor; // For when neither of the below is true
	public Texture2D _clickedCursor; // For when the user is holding down the left mouse button.
	public Texture2D _hoverCursor; // For when the mouse is hovered over interactable objects or UI. Functionality not included here, will likely require a lot of work.

	// Use this for initialization
	void Start () 
	{
    Cursor.SetCursor(_idleCursor,(new Vector2(0,0)),CursorMode.Auto); // Just too set the cursor to its idle at the start
	}

	void Update ()
	{
		if (Input.GetMouseButton(0)) // if Left click is held down
		{
			Cursor.SetCursor(_clickedCursor,(new Vector2(0,0)),CursorMode.Auto);
		}

		else 
		{
			Cursor.SetCursor(_idleCursor,(new Vector2(0,0)),CursorMode.Auto);
		}
	}
}
