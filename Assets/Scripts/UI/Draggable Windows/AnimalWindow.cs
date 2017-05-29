using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
// Title        : AnimalWindow.cs
// Purpose      : Lets you click on an animal in the GUI and spawn it
// Author       : WeirdGamer
// Date         : 25/05/2017
public class AnimalWindow : MonoBehaviour 
{

  public GameObject buttonPrefab;
  public string[] animalID; 
  public GameObject dragScrollBar;
  private GameObject[] buttons;
  private GameObject window;
  private JSONNode jsonInfo;
  private TextAsset jsonText;
  private int x = 4;

  void Start()
  {
    GameObject button;
    jsonText = Resources.Load<TextAsset>("AnimalData");
    jsonInfo = JSON.Parse(jsonText.text);
   buttons = new GameObject[jsonInfo["animalTemplates"].Count];



    for (int i = 0; i < jsonInfo["animalTemplates"].Count; i++) 
    {
      button = (GameObject)Instantiate (buttonPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
      button.GetComponent<AnimalFromGUI>().animalID = jsonInfo["animalTemplates"][i]["id"];
      button.GetComponentInChildren<Text>().text = jsonInfo["animalTemplates"][i]["animalname"];
      button.transform.SetParent(this.transform);
      button.GetComponent<RectTransform> ().localPosition = new Vector3 (-120 + (i % x * 40), -10 - ((Mathf.Floor(i/x - 1)) * 40), 0);
      button.GetComponent<RectTransform> ().localScale = new Vector3 (.4F, .4F, 1);
      buttons[i] = button;
    }
  } // Start()


  void Update () 
  {

  } // Update()

  public void OnScrollChange()
  {
    for (int i = 0; i < buttons.Length; i++) {
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -10 - ((Mathf.Floor(i/x - 1)) * 80) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(jsonInfo["animalTemplates"].Count/3) * 80), 0);
    }
  }


}