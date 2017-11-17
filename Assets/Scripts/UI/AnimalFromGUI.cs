using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Title        : animalFromGUI.cs
// Purpose      : Click on an animals button and place the building
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