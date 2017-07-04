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

  public GameObject buttonPrefab;                             // The prefab used for the button
  public GameObject dragScrollBar;                          // Allows for scrolling in the GUI menu
 
  private string buildingID;
  private Sprite buttonSprite;
  private List<GameObject> buttons;                         //                      
  private GameObject window;                                     // The GUI window in which the buttons are placed
  private JSONNode jsonInfo;                                      // Used to load the buildingData Json file
  private TextAsset[] jsonTexts;
  private int x;                                                                      // number of colums of building buttons
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
        buildingID = jsonInfo["buildingTemplates"][i]["id"];
        buttonSprite = Resources.Load<Sprite>("Buildings/Previews/" + buildingID);

         if (jsonInfo["buildingTemplates"][i]["type"] != null)
         {
           buildingID = jsonInfo["buildingTemplates"][i]["type"] + "/" + buildingID;
         }
    
         button = (GameObject)Instantiate (buttonPrefab,this.transform.position, Quaternion.identity);
         button.GetComponent<BuildingFromGUI>().buildingName = buildingID;

        if (buttonSprite != null) {
          button.GetComponent<Image>().sprite = buttonSprite;
        }
        else {
          button.GetComponentInChildren<Text>().text = jsonInfo["buildingTemplates"][i]["buildingname"];
        }

         button.transform.SetParent(this.transform);
         x = (int) this.GetComponent<RectTransform>().rect.width / 40 - 1;
         button.GetComponent<RectTransform> ().localPosition = new Vector3 (-120 + (count % x * 40), -10 - ((Mathf.Floor(count/x - 1)) * 40), 0);
         button.GetComponent<RectTransform> ().localScale = new Vector3 (.4F, .4F, 1);
         buttons.Add(button);
         count++;
        }
      }
    } // Start()

 void Update () 
  {

  } // Update()
 

 
  public void OnScrollChange()
  {
    for (int i = 0; i < buttons.Count; i++) {
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -10 - ((Mathf.Floor(i/x - 1)) * 40) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(buttons.Count/3) * 40), 0);
    }
  } // OnScrollChange()

} 