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

	public string _targetScene; //Type the Scene you want too load in the Unity Editor
	private bool _buttonPressedRef;
	public Sprite _buttonPressed;
	public Sprite _buttonHover;
	public Sprite _buttonOut;

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~

	// ~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~
	
	void Update () 
	{
	
		__buttonPressedRef = this.GetComponentInParent<buttonPrefabMaster>()._buttonPressed; // Makes sure that the button alkways knows if any button has been pressed already
	}

	void OnMouseDown() // If the user clicks on it, requires a collider on the gameObject
	{
		if (__buttonPressedRef == false)
		{
			this.GetComponentInParent<buttonPrefabMaster>()._buttonPressed = true; // report too its Master that a button has been pressed
			this.GetComponent<Image>().sprite = _buttonPressed;
			SceneManager.LoadScene (_targetScene); // load the targetScene
		}
	}

	void OnMouseEnter() // If the user mouses into it, requires a collider on the gameObject
	{
		if (_buttonPressedRef == false)
		{
			this.GetComponent<Image>().sprite = _buttonHover; 
		}
	}

	void OnMouseExit() // If the user mouses out of it, requires a collider on the gameObject
	{
		if (_buttonPressedRef == false)
		{
			this.GetComponent<Image>().sprite = _buttonOut; 
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
}
