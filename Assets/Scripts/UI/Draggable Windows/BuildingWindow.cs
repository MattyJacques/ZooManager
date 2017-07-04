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
 
  private GameObject[] buildbuttons;                         //                      

 
  private GameObject window;                                     // The GUI window in which the buttons are placed
  private JSONNode jsonInfo;                                      // Used to load the buildingData Json file
  private TextAsset jsonText;
  private Sprite previewPicture;                                    // 
  private int x;                                                                      // number of colums of building buttons

  void Start()
  {    // Loads in the BuildingData json file, Then Generates a series of text buttons with the building name that lets the player then place an building from these
 
     jsonText = Resources.Load<TextAsset>("Buildings/Buildings_Base");
     jsonInfo = JSON.Parse(jsonText.text);
 
    GameObject button;
 
    string buildingID;
    string buildingType; 
 
    buildbuttons = new GameObject[jsonInfo["buildingTemplates"].Count];
  
    for (int i = 0; i < jsonInfo["buildingTemplates"].Count; i++) 
      {
        buildingID = jsonInfo["buildingTemplates"][i]["id"];

        if (jsonInfo["buildingTemplates"][i]["type"] != null)
        {
          buildingID = jsonInfo["buildingTemplates"][i]["type"] + "/" + buildingID;
        }
  
       button = (GameObject)Instantiate (buttonPrefab,this.transform.position, Quaternion.identity);
       button.GetComponent<BuildingFromGUI>().buildingName = buildingID;
 
       button.GetComponentInChildren<Text>().text = jsonInfo["buildingTemplates"][i]["buildingname"];
       button.transform.SetParent(this.transform);

       x = (int) this.GetComponent<RectTransform>().rect.width / 40 - 1;
       button.GetComponent<RectTransform> ().localPosition = new Vector3 (-120 + (i % x * 40), -10 - ((Mathf.Floor(i/x - 1)) * 40), 0);
       button.GetComponent<RectTransform> ().localScale = new Vector3 (.4F, .4F, 1);
       buildbuttons[i] = button;
      }
    } // Start()

 void Update () 
  {

  } // Update()
 

 
  public void OnScrollChange()
  {
    for (int i = 0; i < buildbuttons.Length; i++) {
      buildbuttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buildbuttons[i].GetComponent<RectTransform>().localPosition.x, -10 - ((Mathf.Floor(i/x - 1)) * 40) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(buildbuttons.Length/3) * 40), 0);
    }
  } // OnScrollChange()

} 