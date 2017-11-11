using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

// Title        : PlacementWindow.cs
// Purpose      : Create animal window and populates it with a list of buttons to spawn animals from
// Author       : WeirdGamer
// Date         : 25/05/2017

public class PlacementWindow : MonoBehaviour
{

  public GameObject buttonPrefab;                           // The prefab used for the button
  public GameObject dragScrollBar;                          // Allows for scrolling in the GUI menu
  public string type;
  public string buildingType;

  private string objectID;                                  // The ID of the building to be placed
  private Sprite buttonSprite;                              // Sprite to use for the button
  private List<GameObject> buttons;                         // List of buttons, one for each animal   
  private GameObject window;                                // The GUI window in which the buttons are placed
  private JSONNode jsonInfo;                                // Used to load the buildingData Json file
  private TextAsset[] jsonTexts;                            // List of JSON files used for animals
  private int x;                                            // number of colums of building buttons
  private int count;

  void Start()
  {

    count = 0;
    GameObject button;
    buttons = new List<GameObject>();
    string pathname = "";

    // Loads in all the animals Json files to read the list of animals from
    switch (type)
    {
      case "animal":
        pathname = "Animals";
        break;
      case "building":
        pathname = "Buildings";
        break;  
    }

    jsonTexts = Resources.LoadAll<TextAsset>(pathname + "/");

    foreach (TextAsset jsonText in jsonTexts)
    {

      jsonInfo = JSON.Parse(jsonText.text);

      for (int i = 0; i < jsonInfo[type + "Templates"].Count; i++)
      {
        objectID = jsonInfo[type + "Templates"][i]["id"];
        buttonSprite = Resources.Load<Sprite>(pathname + "/Previews/" + objectID);

       // if (type == "building") { objectID = jsonInfo["buildingTemplates"][i]["type"] + "/" + objectID; }

        button = (GameObject)Instantiate(buttonPrefab, this.transform.position, Quaternion.identity);
        // Create a new button for this animal and add functionality
        switch (type)
        {
          case "animal":
            button.GetComponent<AnimalFromGUI>().animalID = objectID;
            break;
          case "building":
            if (string.Compare(jsonInfo["buildingTemplates"][i]["type"], buildingType) == 0) {
                button.GetComponent<BuildingFromGUI>().buildingID = jsonInfo["buildingTemplates"][i]["type"] + "/" + objectID;
            } else
            {
              Destroy(button);
              continue;
            }
            break;
        }
        

        // Set the button sprite, if there is no sprite for the button fill the button with the name of the animal
        if (buttonSprite != null) { button.GetComponent<Image>().sprite = buttonSprite; }
        else { button.GetComponentInChildren<Text>().text = jsonInfo[type + "Templates"][i][type + "name"]; }

        // Put the button in the right location dependent on the count of animals
        button.transform.SetParent(this.transform);
        x = (int)this.GetComponent<RectTransform>().rect.width / 80 - 1;
        button.GetComponent<RectTransform>().localPosition = new Vector3(-160 + (count % x * 80), -40 - ((Mathf.Floor(count / x - 1)) * 80), 0);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        buttons.Add(button);
        count++;
      }
    }
  } // Start()


  void Update()
  {

  } // Update()

  public void OnScrollChange()
  {
    // Scrollbar functionality of the window
    for (int i = 0; i < buttons.Count; i++)
    {
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -40 - ((Mathf.Floor(i / x - 1)) * 80) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(buttons.Count / 3) * 80), 0);
    }
  }
}