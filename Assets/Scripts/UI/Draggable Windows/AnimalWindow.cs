using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

// Title        : AnimalWindow.cs
// Purpose      : Create animal window and populates it with a list of buttons to spawn animals from
// Author       : WeirdGamer
// Date         : 25/05/2017

public class AnimalWindow : MonoBehaviour 
{
  
  public GameObject buttonPrefab;                           // The prefab used for the button
  public GameObject dragScrollBar;                          // Allows for scrolling in the GUI menu

  private string animalID;                                  // The ID of the building to be placed
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
    buttons = new  List<GameObject>();
    
    // Loads in all the animals Json files to read the list of animals from
    jsonTexts = Resources.LoadAll<TextAsset>("Animals/");
    foreach(TextAsset jsonText in jsonTexts)
    {
      
      jsonInfo = JSON.Parse(jsonText.text);

      for (int i = 0; i < jsonInfo["animalTemplates"].Count; i++) 
      {
        animalID =jsonInfo["animalTemplates"][i]["id"];
        buttonSprite = Resources.Load<Sprite>("Animals/Previews/" + animalID);

        // Create a new button for this animal and add functionality
        button = (GameObject)Instantiate (buttonPrefab, this.transform.position, Quaternion.identity);
        button.GetComponent<AnimalFromGUI>().animalID = animalID;

        // Set the button sprite, if there is no sprite for the button fill the button with the name of the animal
        if (buttonSprite != null) {
          button.GetComponent<Image>().sprite = buttonSprite;
        }
        else {
          button.GetComponentInChildren<Text>().text = jsonInfo["animalTemplates"][i]["animalname"];
        }

        // Put the button in the right location dependent on the count of animals
        button.transform.SetParent(this.transform);
        x = (int) this.GetComponent<RectTransform>().rect.width / 80 - 1;
        button.GetComponent<RectTransform> ().localPosition = new Vector3 (-160 + (count % x * 80), -40 - ((Mathf.Floor(count/x - 1)) * 80), 0);
        button.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
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
    // Scrollbar functionality of the window
    for (int i = 0; i < buttons.Count; i++) {
      buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(buttons[i].GetComponent<RectTransform>().localPosition.x, -40 - ((Mathf.Floor(i/x - 1)) * 80) + dragScrollBar.GetComponent<UnityEngine.UI.Scrollbar>().value * (Mathf.Floor(buttons.Count/3) *80), 0);
    }
  }
}