using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Title: SmokeRemover.cs
//Author: Aimmmmmmmmm
//Date: 1/28/2017
//Purpose: To remove smoke after it's done.
public class SmokeRemover : MonoBehaviour {
	private float time = 0;
	// Use this for initialization
	void Update(){
		time = time + Time.deltaTime;
		if(time > 5){
			Destroy (this.gameObject);
		}
	}
}
