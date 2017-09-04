using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

// Title        : buildingWindow.cs
// Purpose      : Lets you click on an building in the GUI and spawn it
// Author       : WeirdGamer
// Date         : 25/05/2017

public class BuildingWindow : MonoBehaviour 
{

  public GameObject buttonPrefab;                           // The prefab used for the button
  public GameObject dragScrollBar;                          // Allows for scrolling in the GUI menu
  public string buildingType;                               // Type of building: decoration, facilities etc.

  private string buildingID;                                // The ID of the building to be placed
  private Sprite buttonSprite;                              // Sprite to use for the button
  private List<GameObject> buttons;                         // List of buttons, one for each building                     
  private GameObject window;                                // The GUI window in which the buttons are placed
  private JSONNode jsonInfo;                                // Used to load the buildingData Json file
  private TextAsset[] jsonTexts;
  private int x;                                            // number of colums of building buttons
  private int count;

  void Start()
  {    // Loads in the BuildingData json file, Then Generates a series of text buttons with the building name that lets the player then place an building from these
    count = 0;
    GameObject button;
    buttons = new  List<GameObject>();

    jsonTexts = Resources.LoadAll<TextAsset>("Buildings/");
    foreach(TextAsset jsonText in jsonTexts)
    {

      jsonInfo = JSON.Parse(jsonText.text);

      for (int i = 0; i < jsonInfo["buildingTemplates"].Count; i++) 
      {
        if (string.Compare(jsonInfo["buildingTemplates"][i]["type"],buildingType) == 0)
        {
          buildingID = jsonInfo["buildingTemplates"][i]["id"];
          buttonSprite = Resources.Load<Sprite>("Buildings/Previews/" + buildingID);

          buildingID = jsonInfo["buildingTemplates"][i]["type"] + "/" + buildingID;
    
          button = (GameObject)Instantiate(buttonPrefab, this.transform.position, Quaternion.identity);
          button.GetComponent<BuildingFromGUI>().buildingName = buildingID;

          if (buttonSprite != null)
          {
            button.GetComponent<Image>().sprite = buttonSprite;
          }
          else
          {
            button.GetComponentInChildren<Text>().text = jsonInfo["buildingTemplates"][i]["buildingname"];
          }

          button.transform.SetParent(this.transform);
          x = (int)this.GetComponent<RectTransform>().rect.width / 80 - 1;
          button.GetComponent<RectTransform>().localPosition = new Vector3(-160 + (count % x * 80), -40 - ((Mathf.Floor(count / x - 1)) * 80), 0);
          button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
          buttons.Add(button);
          count++;
        }
        else
        {
          Debug.Log(jsonInfo["buildingTemplates"][i]["type"]);
          Debug.Log(buildingType);
        }
      }
    }
  } // Start()

 void Update () 
  {

  } // Update()
 

 
  public void OnScrollChange()
  {
    for (int i = 0; i < buttons.Count; i++) {
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -40 - ((Mathf.Floor(i/x - 1)) * 80) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(buttons.Count/3) * 80), 0);
    }
  } // OnScrollChange()

} 