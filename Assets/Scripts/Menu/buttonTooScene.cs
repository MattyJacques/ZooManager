// Title        : ButtonTooScene.cs
// Purpose      : To be able to load a new scene from a button press
// Author       : Jeremy Mann
// Date         : 17/11/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttonTooScene : MonoBehaviour {

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~

	public string targetScene; //Type the Scene you want too load in the Unity Editor
	private bool buttonPressedRef;
	public Sprite buttonPressed;
	public Sprite buttonHover;
	public Sprite buttonOut;

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~

	// ~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~
	
	void Update () 
	{
	
		buttonPressedRef = this.GetComponentInParent<buttonPrefabMaster>().buttonPressed; // Makes sure that the button alkways knows if any button has been pressed already
	}

	void OnMouseDown() // If the user clicks on it, requires a collider on the gameObject
	{
		if (buttonPressedRef == false)
		{
			this.GetComponentInParent<buttonPrefabMaster>().buttonPressed = true; // report too its Master that a button has been pressed
			this.GetComponent<Image>().sprite = buttonPressed;
			SceneManager.LoadScene (targetScene); // load the targetScene
		}
	}

	void OnMouseEnter() // If the user mouses into it, requires a collider on the gameObject
	{
		if (buttonPressedRef == false)
		{
			this.GetComponent<Image>().sprite = buttonHover; 
		}
	}

	void OnMouseExit() // If the user mouses out of it, requires a collider on the gameObject
	{
		if (buttonPressedRef == false)
		{
			this.GetComponent<Image>().sprite = buttonOut; 
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
}
