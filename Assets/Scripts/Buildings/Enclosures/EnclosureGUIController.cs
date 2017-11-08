// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Enclosure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Buildings.Enclosures
{
    public class EnclosureGUIController : MonoBehaviour
    {
        private EnclosureComponent _enclosure;
        private Transform _canvas;
        public UIState _state;

        public enum UIState
        {
            Uninitialized,  //The gui hasn't been initialized
            Hidden, //The gui is initialized, but the canvas has been hidden
            MainMenu, //We're showing the main GUI
            EditEnclosureName,  //We're at the edit enclosure name screen
        }

        private void Start()
        {
            _enclosure = gameObject.GetComponent<EnclosureComponent> (); ;
        }

        public void Initialize()
        { //Initialization. Is called from the Enclosure class.
    
            //Create the canvas
            _canvas = Instantiate (Resources.Load<Transform> ("Menus/Prefabs/Canvas_Enclosure"));
            _canvas.transform.position = transform.position + new Vector3 (0f, 6f, 0f); //Position

            //Set our initial state
            ChangeState (UIState.MainMenu); //Assume we're always starting in the main state

            //Attach the actions to the buttons
            _canvas.transform.Find ("Button_Exit").GetComponent<Button> ()
                .onClick.AddListener (delegate
                {
                    HideCanvas ();
                });
            //TODO: update the enclosures name in the UI
            _canvas.transform.Find ("InitialButtons/Button_Rename").GetComponent<Button> ()
                .onClick.AddListener (delegate
                {
                    ChangeState (UIState.EditEnclosureName);
                });

            InputField inputField = _canvas.transform.Find ("InputField").GetComponent<InputField> ();
            inputField.onEndEdit.AddListener (delegate 
            {
                _enclosure.Rename (inputField.text);
                ChangeState (UIState.MainMenu);
            });

        } //Initialize()

        public void HideCanvas()
        { //Removes the canvas from sight
            _canvas.gameObject.SetActive (false);
            ChangeState(UIState.Hidden);
        } //HideCanvas()

        public void ShowCanvas()
        { //Displays the canvas
            _canvas.gameObject.SetActive (true);
            ChangeState(UIState.MainMenu);
        } //ShowCanvas()

        private void ChangeState(UIState newState)
        { //Changes the UI state to newState (if it's a legal move)
    

            //Reset everything
            _canvas.transform.Find("InitialButtons").gameObject.SetActive(false);
            _canvas.transform.Find("InputField").gameObject.SetActive(false);

            //Activate whatever UI object we need for this state
            switch (newState)
            {
                //Main state, displays navigation buttons
                case UIState.MainMenu:
                    _canvas.transform.Find("InitialButtons").gameObject.SetActive(true);
                    break;

                case UIState.EditEnclosureName:
                    _canvas.transform.Find("InputField").gameObject.SetActive(true);
                    _canvas.transform.Find("InputField").GetComponent<InputField>().text = "";
                    break;
            }

            _state = newState;

        } //ChangeState()
    }
}
