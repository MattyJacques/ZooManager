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

	public GameObject targetPrefab; // Select the prefab you want too spawn in the Unity Editor
	private bool buttonPressedRef;
	public Sprite buttonPressed;
	public Sprite buttonHover;
	public Sprite buttonOut;


	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~

	// ~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~
	
	void Update () 
	{
		buttonPressedRef = this.GetComponentInParent<buttonPrefabMaster>().buttonPressed;
	}

	void OnMouseDown() // If the user clicks on it, requires a collider on the gameObject
	{
		if (buttonPressedRef == false)
		{

			this.GetComponentInParent<buttonPrefabMaster>().pushedPrefab = targetPrefab; // Sends the defined prefab to its master for Instantiation
			this.GetComponentInParent<buttonPrefabMaster>().buttonPressed = true; // report too its Master that a button has been pressed
			this.GetComponent<Image>().sprite = buttonPressed;
			this.GetComponentInParent<buttonPrefabMaster>().SpawnAndDestroySelf(); // Instantiate the new prefab and destroy this one
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

		else
		{
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
}