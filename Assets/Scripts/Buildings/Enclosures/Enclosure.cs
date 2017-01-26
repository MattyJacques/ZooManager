// Title        : Enclosure.cs
// Purpose      : This class controls both the GUI and functionality for the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using System.Collections.Generic;
using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
//TODO: Edit interioritem method takes generic interior item, can be passed either from list or existing item
//TODO: separate the GUI into it's own class
public class Enclosure : MonoBehaviour
{
    public int maxNameLength = 20;
    public int minNameLength = 3;

    //Contains all the available interior items for read-only purposes
    private static EnclosureInteriorItem[] _interiorItemsList = null;
    private List<EnclosureInteriorItem> _interiorItems = new List<EnclosureInteriorItem> ();
    public enum State {
        Idle,
        DisplayingMenu,
        EditingInteriorItem,
    };
    public State _state = State.Idle;

    private string _name;
    private GameObject _canvas;
    //The interiorItem used when in state EditingInteriorItem
    private EnclosureInteriorItem _selectedInteriorItem;
    //The "ghost" representation of the selected interiorItem when moving it
    private Transform _selectedInteriorItemGhost;
    public enum UIState
    {
        Exit,
        Main,
        EditName,
        AddInteriorItem,
        EditInteriorItems
    } 

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

    void Update()
    {
        //TODO: Detect if a click is NOT on the enclosure or any GUI object and then exit the GUI menu
        if (_state == State.EditingInteriorItem)
        {
            if (_selectedInteriorItemGhost == null)
            {
                Debug.Log ("No selected interior item ghost!");
                UIChangeState (UIState.Main);
            }
            else
            {
                //TODO: Move to own method and constrain within enclosure
                Vector3 mousePos = Vector3.zero;
                if (InputHelper.GetMousePositionInWorldSpace (out mousePos, (1<<9))) //We only want to check layer 9 aka "Ground"
                {
                    _selectedInteriorItemGhost.position = mousePos + _selectedInteriorItem.positionOffset;
                }
            }

            if (Input.GetMouseButtonDown (0))
            {
                FinalizeMovingInteriorItem ();
            }

            if (Input.GetMouseButtonDown (1))
            {
                CancelMovingInteriorItem ();
            }
        }
    }

    public void Interact()
    {   //Is called when the player clicks on the Enclosure NOTE: replace with onclick?
        //Instantiates and initializes the GUI that allows for player interaction with the enclosure

        //Don't create a new canvas if we already have one
        if (_canvas != null)
        {
            return;
        }

        //Create the new canvas
        _canvas = Instantiate (Resources.Load ("Menus/Prefabs/Canvas_Enclosure") as GameObject);
        Debug.Log (_canvas.name + ": " + _canvas.transform.position.ToString());
        _canvas.transform.FindChild ("Text_EnclosureName").GetComponent<Text> ().text = _name;
        _canvas.transform.position = transform.position + new Vector3 (0f, 6f, 0f); //TODO: place canvas so that the camera sees it

        //Hook up functionality to the different GUI events
        InputField input = _canvas.transform.FindChild ("InputField").GetComponent<InputField> ();
        input.onEndEdit.AddListener (delegate
        {
            Rename (input.text);
            //Go back to main after we have edited the name
            UIChangeState (UIState.Main);
        });

        Button button_Rename = _canvas.transform.FindChild ("InitialButtons/Button_Rename").GetComponent<Button> ();
        button_Rename.onClick.AddListener (delegate
         {
             UIChangeState (UIState.EditName);
         });
        
        Button button_EditInteriorItems = _canvas.transform.FindChild ("InitialButtons/Button_EditItems").GetComponent<Button> ();
        button_EditInteriorItems.onClick.AddListener (delegate
        {
            UIChangeState (UIState.EditInteriorItems);
        });

        Button button_AddNewInteriorItem = _canvas.transform.FindChild ("InitialButtons/Button_AddItem").GetComponent<Button> ();
        button_AddNewInteriorItem.onClick.AddListener (delegate
        {
            UIChangeState (UIState.AddInteriorItem);
        });

        Button button_Exit = _canvas.transform.FindChild ("Button_Exit").GetComponent<Button> ();
        button_Exit.onClick.AddListener (delegate
        {
            UIChangeState (UIState.Exit);
        });
        //Delete
        //Edit interior items
        //Lights up somehow, makes interior objects moveable within the enclosure, debug buttons for adding items

        UIChangeState (UIState.Main);
    }   //Interact()

    //TODO: Move ui stuff to it's own class
    public void UIChangeState(UIState state)
    {   //Changes the state of the canvas that is attached to the enclosure

        if (_canvas == null)
        {
            Debug.LogWarning ("UIChangeState was called, but there is no canvas attached to enclosure.");
            return;
        }

        //Reset everything
        _canvas.transform.FindChild ("InitialButtons").gameObject.SetActive (false);
        _canvas.transform.FindChild ("InputField").gameObject.SetActive (false);
        var addNewInteriorItemDiv = _canvas.transform.FindChild ("AddNewInteriorItemList");
        addNewInteriorItemDiv.gameObject.SetActive (false);
        var interiorItemListDiv = _canvas.transform.FindChild ("InteriorItemList");
        interiorItemListDiv.gameObject.SetActive (false);

        //Activate whatever UI object we need for this state
        switch (state) {
            case UIState.Exit:
                Destroy (_canvas);
                _canvas = null;
                break;

            //Main state, displays navigation buttons
            case UIState.Main:
                _canvas.transform.FindChild ("InitialButtons").gameObject.SetActive (true);
                break;

            case UIState.EditName:
                _canvas.transform.FindChild ("InputField").gameObject.SetActive (true);
                _canvas.transform.FindChild ("InputField").GetComponent<InputField> ().text = "";
                break;

            case UIState.AddInteriorItem:
                PopulateInteriorItemGUIList (_interiorItemsList.ToArray(), addNewInteriorItemDiv);
                addNewInteriorItemDiv.gameObject.SetActive (true);
                break;

            case UIState.EditInteriorItems:
                interiorItemListDiv.gameObject.SetActive (true);
                PopulateInteriorItemGUIList (_interiorItems.ToArray (), interiorItemListDiv);
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
        var item = _interiorItemsList.Where (x => x.name == itemName).FirstOrDefault();
        if (item == null)
        {
            Debug.LogError ("Could not find InteriorItem " + itemName 
                + " in list of available interior items.");
        }
        return item;
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

    //InteriorItemGUI List
    private void PopulateInteriorItemGUIList(EnclosureInteriorItem[] interiorItemArr, Transform contentDiv)
    {
        if (interiorItemArr.Length <= 0)
        {
            Debug.LogWarning ("PopulateInteriorItemGUIList got an empty array as parameter.");
            return;
        }

        Transform itemGUIPrefab = Resources.Load<Transform> ("Menus/Prefabs/EnclosureInteriorItemGUISelection");
        for (int i = 0; i < interiorItemArr.Length; i++)
        {
            Debug.Log ("Adding item " + i);
            Transform itemGUIObject = Instantiate (itemGUIPrefab);
            itemGUIObject.SetParent (contentDiv);
            itemGUIObject.localScale = new Vector3 (1f, 1f, 1f);
            itemGUIObject.localRotation = Quaternion.Euler (Vector3.zero);

            //Move the position downward by it's height
            itemGUIObject.localPosition = new Vector3 (0f,
                -itemGUIObject.GetComponent<RectTransform> ().sizeDelta.y * i,
                0f);

            //Set the name
            itemGUIObject.GetComponentInChildren<Text> ().text = interiorItemArr[i].name;

            //Set the onClick event
            int _i = i;
            itemGUIObject.GetComponentInChildren<Button> ().onClick.AddListener (delegate 
            { //Create a new interior item and immediately start editing it's position
                var interiorItem = AddInteriorItem (_interiorItemsList[_i]);    //TODO: add support for both edit and add
                MoveInteriorItem (interiorItem);
            });
        }
    }

    private void MoveInteriorItem(EnclosureInteriorItem interiorItem)
    {
        if (interiorItem.transform == null)
        { //This shouldn't happen
            Debug.LogError ("Tried moving interiorItem without an instantiated transform!\n"
                + "Type: " + interiorItem.type.ToString () + ", named: " + interiorItem.name);
        }
        else
        {
            _state = State.EditingInteriorItem;
            _selectedInteriorItem = interiorItem;
            _selectedInteriorItemGhost = Instantiate(interiorItem.transform);
            UIChangeState (UIState.Exit);
        }
    }

    private void CancelMovingInteriorItem()
    { //Resets interiorItem's position //TODO: refunds cost
        Debug.Log ("Canceled item move.");
        Destroy (_selectedInteriorItemGhost.gameObject);
        _state = State.DisplayingMenu;
        UIChangeState (UIState.Main);
    } //CancelMovingInteriorItem

    private void FinalizeMovingInteriorItem()
    { //Update the InteriorItems position
        Debug.Log ("Finalized item move");
        GameObject g = Instantiate (_selectedInteriorItem.transform.gameObject);
        g.transform.position = _selectedInteriorItemGhost.position;
        g.transform.rotation = _selectedInteriorItemGhost.rotation;
        Destroy (_selectedInteriorItemGhost.gameObject);
        _state = State.DisplayingMenu;
        UIChangeState (UIState.Main);
    } //FinalizeMovingInteriorItem

    public EnclosureInteriorItem AddInteriorItem(EnclosureInteriorItem interiorItem)
    { //Adds a new interior item to the enclosure
        //TODO: Tie up with money system
        //Instantiate it off-screen
        interiorItem.Instantiate (new Vector3(5000, 5000, 5000));
        _interiorItems.Add (interiorItem);
        return interiorItem;
    } //AddInteriorItem

}