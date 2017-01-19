// Title        : Enclosure.cs
// Purpose      : This class controls the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class Enclosure : MonoBehaviour
{
    public int maxNameLength = 20;
    public int minNameLength = 3;

    //Contains all the available interior items for read-only purposes
    private static EnclosureInteriorItem[] _interiorItemsList = null;
    private List<EnclosureInteriorItem> _interiorItems = new List<EnclosureInteriorItem> ();
    public enum State { Idle, DisplayingMenu, EditingInteriorItems };
    public State _state = State.Idle;

    private string _name;
    private GameObject _canvas;

    private void Start()
    {   //Initialization
        if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
        {
            Debug.LogError ("Enclosure " + name + " at " + transform.position.ToString () + " has no collider!");
        }
        
        //Populates the _interiorItemsList from the BuildingData file if it does not exist
        if (_interiorItemsList == null)
        {
            JSONNode jsonData = JSON.Parse (Resources.Load<TextAsset>("BuildingData").text);
            jsonData = jsonData["interiorEnclosureItems"];
            _interiorItemsList = new EnclosureInteriorItem[jsonData.Count];
            for (int i = 0; i < jsonData.Count; i++)
            {
                EnclosureInteriorItem newInteriorItem = new EnclosureInteriorItem (jsonData[i]);
                _interiorItemsList[i] = newInteriorItem;
            }
        }
    }   //Start()

    public void Interact()
    {   //Instantiates and initializes the GUI that allows for player interaction with the enclosure

        //Don't create a new canvas if we already have one
        if (_canvas != null)
        {
            return;
        }

        //Create the new canvas
        _canvas = Instantiate (Resources.Load ("Menus/Prefabs/Canvas_Enclosure") as GameObject);
        _canvas.transform.FindChild ("Text_EnclosureName").GetComponent<Text> ().text = _name;
        _canvas.transform.position = transform.position + new Vector3 (0f, 6f, 0f); //TODO: place canvas so that the camera sees it

        //Hook up functionality to the different GUI events
        InputField input = _canvas.transform.FindChild ("InputField").GetComponent<InputField> ();
        input.onEndEdit.AddListener (delegate
        {
            Rename (input.text);
            UIChangeState (0);
        });

        Button button_Rename = _canvas.transform.FindChild ("InitialButtons").FindChild ("Button_Rename").GetComponent<Button> ();
        button_Rename.onClick.AddListener (delegate
         {
             UIChangeState (1);
         });

        Button button_Edit = _canvas.transform.FindChild ("InitialButtons").FindChild ("Button_Edit").GetComponent<Button> ();
        button_Edit.onClick.AddListener (delegate
        {
            UIChangeState (2);
        });

        Button button_Exit = _canvas.transform.FindChild ("Button_Exit").GetComponent<Button> ();
        button_Exit.onClick.AddListener (delegate
        {
            UIChangeState (-1);
        });
        //Delete
        //Edit interior items
            //Lights up somehow, makes interior objects moveable within the enclosure, debug buttons for adding items

    }   //Interact()

    public void UIChangeState(int state)
    {   //Changes the state of the canvas that is attached to the enclosure

        if (_canvas == null)
        {
            Debug.LogWarning ("UIChangeState was called, but there is no canvas attached to enclosure.");
            return;
        }

        switch (state) {

            //Exiting
            case -1:
                Destroy (_canvas);
                _canvas = null;
                break;

            //Main state, displays navigation buttons
            case 0:
                _canvas.transform.FindChild ("InitialButtons").gameObject.SetActive (true);
                _canvas.transform.FindChild ("InputField").gameObject.SetActive (false);
                break;

            //Edit name
            case 1:
                _canvas.transform.FindChild ("InitialButtons").gameObject.SetActive (false);
                _canvas.transform.FindChild ("InputField").gameObject.SetActive (true);
                _canvas.transform.FindChild ("InputField").GetComponent<InputField> ().text = "";
                break;

            //Edit interior items
            case 2:
                _canvas.transform.FindChild ("InitialButtons").gameObject.SetActive (false);
                _canvas.transform.FindChild ("InputField").gameObject.SetActive (false);
                break;
        }
    }   //UIChangeState()

    public Vector3 GetClosest()
    {   //Returns the closest object of itemType
        return Vector3.zero;
    }   //GetClosest()

    public bool Rename(string name)
    {   //Renames the enclosure
        if (name.Length > minNameLength || name.Length < maxNameLength)
        {
            _name = name;
            if (_canvas != null)
            {
                _canvas.transform.FindChild ("Text_EnclosureName").GetComponent<Text> ().text = _name;
            }
            return true;
        }
        else
        {
            return false;
        }
    }   //Rename()

    public void DeleteThisEnclosure()
    {   //Deletes this enclosure and all attached objects

        if (_canvas != null)
        {
            Destroy (_canvas);
        }

        foreach (EnclosureInteriorItem interiorItem in _interiorItems)
        {
            //Destroy (interiorItem.transform.gameObject);
        }

        Destroy (gameObject);

    }   //DeleteThisEnclosure

    public EnclosureInteriorItem AddInteriorItem(string itemName)
    {
        //TODO: look for interior items in json/xml and load them in here
        /*
        EnclosureInteriorItem interiorItem = new EnclosureInteriorItem ();
        interiorItem._name = itemName;
        return interiorItem;
        */
        
        return null;
    }

    public void RemoveInteriorItem (EnclosureInteriorItem item)
    {
        if (_interiorItems.Contains (item))
        {
            _interiorItems.Remove (item);
        }
        else
        {
            Debug.LogWarning ("Tried removing InteriorItem of type " + item.name + " from " + name + " " + _name + ", but no such item exists in this enclosure."
                +"\nEnclosure contains " + _interiorItems.Where(x => x.name == item.name).Count() + " other objects of same type.");
        }
    }

}