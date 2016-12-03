using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//This is a script written specifically to allow for interaction with the underlaying UI as of right now it just allows button press to open windows
public class UInput : MonoBehaviour, IPointerDownHandler
{
    public bool ControlST = false;
    //a boolean to prevent the stafftab being activated in other tabs
    public bool ControlFT = false;
    //a boolean to prevent the financestab being activated in other tabs
    public bool ControlBPT = false;
    //a boolean to prevent the blueprintstabs being activated in other tabs
    public bool ControlSHT = false;
    //a boolean to prevent the shopstab being activated in other tabs
    public bool ControlAT = false;
    //a boolean to prevent the animalstab being activated in other tabs
    public bool ControlTT = false;
    //a boolean to prevent the terraintab being activated in other tabs
    public bool ControlPAT = false;
    //a boolean to prevent the plantstab being activated in other tabs
    public bool ControlBT = false;
    //a boolean to prevent the bulldozetab being activated in other tabs
    public bool ControlPT = false;
    //a boolean to prevent the pathstab being activated in other tabs
    public bool ControlBD = false;
    //a boolean to prevent the buidling tab being activated in other tabs
   
    public GameObject StaffTab;
    public GameObject FinancesTab;
    public GameObject BluePrintsTab;
    public GameObject ShopsTab;
    public GameObject AnimalsTab;
    public GameObject PathsTab;
    public GameObject TerrainTab;
    public GameObject BullDozeTab;
    public GameObject BuildingsTab;
    public GameObject PlantsTab;
    void Awake()
    {
        //automatically setting all tabs to false on awake
        StaffTab.SetActive(false);
        FinancesTab.SetActive(false);
        BluePrintsTab.SetActive(false);
        ShopsTab.SetActive(false);
        AnimalsTab.SetActive(false);
        PathsTab.SetActive(false);
        TerrainTab.SetActive(false);
        BullDozeTab.SetActive(false);
        BuildingsTab.SetActive(false);
        PlantsTab.SetActive(false);
    }
    //This will handle on down input
    public void OnPointerDown(PointerEventData eventData)
    {
        //all of these just make it so you can open certain tabs if it's set to false
        if(ControlST == false)
        {
            StaffTab.SetActive(true);
        }

        if (ControlBPT == false)
        {
            BluePrintsTab.SetActive(true);
        }

        if (ControlFT == false)
        {
            FinancesTab.SetActive(true);
        }

        if (ControlSHT == false)
        {
            ShopsTab.SetActive(true);
        }

        if (ControlAT == false)
        {
            AnimalsTab.SetActive(true);
        }

        if (ControlTT == false)
        {
            TerrainTab.SetActive(true);
        }
        if (ControlBT == false)
        {
            BullDozeTab.SetActive(true);
        }
        if (ControlPT == false)
        {
            PathsTab.SetActive(true);
        }
        if(ControlBD == false)
        {
            BuildingsTab.SetActive(true);
        }
        if(ControlPAT == false)
        {
            PlantsTab.SetActive(true);
        }
    }
    }
