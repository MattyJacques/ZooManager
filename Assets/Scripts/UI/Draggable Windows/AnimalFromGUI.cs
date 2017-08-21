using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : animalFromGUI.cs
// Purpose      : Lets you click on an animal in the GUI and spawn it
// Author       : WeirdGamer
// Date         : 25/05/2017
public class AnimalFromGUI : MonoBehaviour {

  public string animalID;
  private GameObject managers;

  void Awake()
  {
    managers = GameObject.Find("Managers");
  }
  

  public void OnClick()
    {
      managers.gameObject.GetComponent<Assets.Scripts.Managers.AnimalManager> ().CreateFollowMouse (animalID);
    } 
}