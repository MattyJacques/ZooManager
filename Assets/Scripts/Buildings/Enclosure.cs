﻿// Title        : Enclosure.cs
// Purpose      : This class controls the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enclosure : MonoBehaviour
{
    public int maxNameLength = 20;
    public int minNameLength = 3;

    private List<InteriorItem> _interiorItems = new List<InteriorItem> ();
    public enum State { Idle, DisplayingMenu, EditingInteriorItems };
    public State _state = State.Idle;

    private string _name;
    private GameObject _canvas;

    public class InteriorItem {
        public Transform transform;
        public Type type;
        public enum Type { Water, Food };
    }

    private void Start()
    {   //Initialization
        if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
        {
            Debug.LogError ("Enclosure " + name + " at " + transform.position.ToString () + " has no collider!");
        }
    }   //Start()

    private void OnGUI()
    {
        if (_state != State.DisplayingMenu)
        {
            return;
        }


    }

    public void Interact()
    {   //Opens and initializes the GUI that allows for player interaction

        //Don't create a new canvas if we already have one
        if (_canvas != null)
        {
            return;
        }

        _canvas = Instantiate (Resources.Load ("GUI/Prefabs/Canvas_Enclosure") as GameObject);
        _canvas.transform.FindChild ("Text_EnclosureName").GetComponent<Text> ().text = _name;
        _canvas.transform.position = transform.position + new Vector3 (0f, 2f, 0f); //TODO: place canvas so that the camera sees it

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
        //Resize
        //Edit interior items
            //Lights up somehow, makes interior objects moveable within the enclosure, debug buttons for adding items

    }   //Interact()

    //make ui stuff private if possible
    public void UIChangeState(int state)
    {
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
    }

    public Vector3 GetClosest(InteriorItem.Type itemType)
    {   //Returns the closest object of itemType
        return Vector3.zero;
    }   //GetClosest()

    public bool Rename(string name)
    {
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
    }

}