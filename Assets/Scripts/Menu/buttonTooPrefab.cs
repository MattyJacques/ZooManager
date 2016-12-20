// Title        : ButtonTooScene.cs
// Purpose      : To be able to load a new scene from a button press
// Author       : Jeremy Mann
// Date         : 17/11/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonTooPrefab : MonoBehaviour {

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~

	public GameObject _targetPrefab; // Select the prefab you want too spawn in the Unity Editor
	private bool _buttonPressedRef;
	public Sprite _buttonPressed;
	public Sprite _buttonHover;
	public Sprite _buttonOut;


	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~

	// ~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~
	
	void Update () 
	{
		_buttonPressedRef = this.GetComponentInParent<buttonPrefabMaster>()._buttonPressed;
	}

	void OnMouseDown() // If the user clicks on it, requires a collider on the gameObject
	{
		if (__buttonPressedRef == false)
		{

			this.GetComponentInParent<buttonPrefabMaster>().pushedPrefab = _targetPrefab; // Sends the defined prefab to its master for Instantiation
			this.GetComponentInParent<buttonPrefabMaster>()._buttonPressed = true; // report too its Master that a button has been pressed
			this.GetComponent<Image>().sprite = _buttonPressed;
			this.GetComponentInParent<buttonPrefabMaster>().SpawnAndDestroySelf(); // Instantiate the new prefab and destroy this one
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

		else
		{
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
}