// Title        : UItransparency.cs
// Purpose      : Controls the Transparency of the GUI
// Author       : WeirdGamer
// Date         : 29/08/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransparency : MonoBehaviour {

  public float opacity;
  public Slider opacitySlider;

	// Initializing
	void Start() 
  { // Defaults opacity to 80%, TODO: ready defaults in for a text file with all default settings?
    opacity = .8f;
    SetOpacity();
  } // Start()
	
	void Update()
  {
  } // Update()

  private void SetOpacity()
  { // Finds all UI elements attached to this and sets their opacity to the given opacity value
    foreach (Image image in this.GetComponentsInChildren<Image>())
    {
      Color color = image.color;
      color.a = opacity;
      image.color = color;
    }
  } // SetOpacity()

  public void OnSliderChange()
  { // Allows for outside control of the opacity value
    opacity = opacitySlider.value / opacitySlider.maxValue;

    SetOpacity();
  } // OnSliderChange()
}
