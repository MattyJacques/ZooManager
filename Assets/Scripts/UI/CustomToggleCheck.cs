using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
// Title        : CustomToggleCheck.cs
// Purpose      : Checks not only if a toggle is changed, but whether it has been turned on or off
// Author       : WeirdGamer
// Date         : 26/01/2017
public class CustomToggleCheck : MonoBehaviour 
{
  public Toggle toggle;
  public UnityEvent terrainMethod;

  private TerrainTools terrainTools;
  private bool changed;

  void Start()
  {

  }

  void Update()
  {
    if (changed && toggle.isOn)
    {
      terrainMethod.Invoke();
      changed = false;
    }
  }
  public void OnValueChanged()
  { 
    changed = true;
  }



}
  


