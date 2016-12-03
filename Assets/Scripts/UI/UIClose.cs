using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Allows you to close open tabs(Press the black block it's only placeholder obviously)
public class UIClose : MonoBehaviour, IPointerDownHandler
{
    public GameObject mainTab;
    //allows you to pick the tab you want closing
    public void OnPointerDown(PointerEventData eventData)
    {
        //allows you to close the host tab of the black button
        mainTab.SetActive(false);
    }

    }
