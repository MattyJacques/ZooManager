// Title        : ButtonTooPrefab.cs
// Purpose      : To destroy current menu prefab, Instantiate a new target menu prefab upon button press.
// Author       : Jeremy Mann
// Date         : 17/11/2016


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPrefabMaster : MonoBehaviour {

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~

	public bool _buttonPressed;
	public GameObject _pushedPrefab;

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
	// ~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~

	void Start () 
	{
		_buttonPressed = false;

		// By default it will take the transform attributes of the gameobject that spawned it but interpret it as WorldSpace instead of LocalSpace. Brute force fixing that here, will be a problem for anyone redesigning the menu.
		transform.localPosition = new Vector3(-195.4971f,-147.3622f,934.964f);
		transform.localScale = new Vector3(207.8461f,207.8461f,207.8461f);
	}


	public void SpawnAndDestroySelf () 
	{
		if (_pushedPrefab == null) // This will the case if the button pressed is a TooScene type.
		{
			Destroy(gameObject);
		}

		else
		{
			Instantiate(_pushedPrefab,this.transform.parent); // spawn prefab associated too button, parented too the Canvas
			Destroy(gameObject);
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~
}
