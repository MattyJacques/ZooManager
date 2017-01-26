// Title        : Enclosure.cs
// Purpose      : This class controls both the GUI and functionality for the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using UnityEngine;
using UnityEngine.UI;

//TODO: separate the GUI into it's own class
public class Enclosure : MonoBehaviour
{
    public int maxNameLength = 20;
    public int minNameLength = 3;

    public enum State {
        Idle,
        DisplayingMenu,
    };
    public State _state = State.Idle;

    private string _name;
    private GameObject _canvas;
    //The "ghost" representation of the selected interiorItem when moving it
    private Transform _selectedInteriorItemGhost;
    public enum UIState
    {
        Exit,
        Main,
        EditName
    } 

    private void Start()
    {   //Initialization
        if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
        {
            Debug.LogError ("Enclosure " + name + " at " + transform.position.ToString () + " has no collider!");
        }
    }   //Start()

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

        Button button_Exit = _canvas.transform.FindChild ("Button_Exit").GetComponent<Button> ();
        button_Exit.onClick.AddListener (delegate
        {
            UIChangeState (UIState.Exit);
        });
        //Delete

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

        Destroy (gameObject);

    }   //DeleteThisEnclosure

}