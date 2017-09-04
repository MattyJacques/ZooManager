using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeControl : MonoBehaviour {

  public Slider _sizeScrollbar;

	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update () 
  {
		
	}

  public void ChangeSize()
  {
    float currentValue = _sizeScrollbar.value;
    float normValue = currentValue / _sizeScrollbar.maxValue;

    this.transform.localScale = new Vector3(normValue, normValue, normValue);
  }
}
