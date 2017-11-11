// Title        : FreeFormPathGUIController
// Purpose      : This handles functionality on the GUI menu for the free-form enclosure system.
// Author       : Tony Haggar
// Date         : 14/08/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeFormPathGUIController : MonoBehaviour {

    public Button pathModeButton;
    public Button straightButton;
    public Button curveButton;
    public Button circleButton;
    public Canvas canvas;
    public FreeFormPathManager manager;

    private bool isEnclosureMode = false;

    public void flipEnclosureMode()
    {
        isEnclosureMode = (isEnclosureMode) ? false : true;

        if (!isEnclosureMode)
        {
            straightButton.gameObject.SetActive(false);
            curveButton.gameObject.SetActive(false);
            circleButton.gameObject.SetActive(false);
        }
        else
        {
            straightButton.gameObject.SetActive(true);
            curveButton.gameObject.SetActive(true);
            circleButton.gameObject.SetActive(true);   
        }
    }

    private void pathModeButtonClicked()
    {
        flipEnclosureMode();
    }

    private void straightButtonClick()
    {
        manager.BuildNewFreeFormPath(false);
    }

    private void curveButtonClick()
    {
        manager.BuildNewFreeFormPath(true);
    }

    private void circleButtonClick()
    {
        manager.BuildNewCirclePath();
    }



    // Use this for initialization
    void Start () {
        straightButton.gameObject.SetActive(false);
        curveButton.gameObject.SetActive(false);
        circleButton.gameObject.SetActive(false);

        pathModeButton.onClick.AddListener(pathModeButtonClicked);
        straightButton.onClick.AddListener(straightButtonClick);
        curveButton.onClick.AddListener(curveButtonClick);
        circleButton.onClick.AddListener(circleButtonClick);
    }
	
	// Update is called once per frame
	void Update () {

        
		
	}
}
