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
  public GameObject dragScrollBar;
  
  private string animalID;
  private Sprite buttonSprite;
  private GameObject[] buttons;
  private GameObject window;
  private JSONNode jsonInfo;
  private TextAsset jsonText;
  private int x;

  void Start()
  {
    GameObject button;

    jsonText = Resources.Load<TextAsset>("AnimalData");
    jsonInfo = JSON.Parse(jsonText.text);
   
    buttons = new GameObject[jsonInfo["animalTemplates"].Count];
    buttonSprite = Resources.Load<Sprite>("Animals/Previews/" + animalID);

    for (int i = 0; i < jsonInfo["animalTemplates"].Count; i++) 
    {
      animalID =jsonInfo["animalTemplates"][i]["id"];

      button = (GameObject)Instantiate (buttonPrefab, this.transform.position, Quaternion.identity);
      button.GetComponent<AnimalFromGUI>().animalID = animalID;

      if (buttonSprite != null) {
        button.GetComponent<Image>().sprite = buttonSprite;
        Debug.Log("here!");
      }
      else {
        button.GetComponentInChildren<Text>().text = jsonInfo["animalTemplates"][i]["animalname"];
      }

      button.transform.SetParent(this.transform);
      x = (int) this.GetComponent<RectTransform>().rect.width / 40 - 1;
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
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -10 - ((Mathf.Floor(i/x - 1)) * 40) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(jsonInfo["animalTemplates"].Count/3) *40), 0);
    }
  }


}