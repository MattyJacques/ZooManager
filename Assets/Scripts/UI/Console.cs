// Title        : GameClock.cs
// Purpose      : Open console with ` or '. 
//                Submit with "enter" or "return".
//				        Use the console to spawn items in the "Resources" directory.
//				        spawn <Id/Name/Tag> <Amount> <Position> <Other>
// Author       : Dan Budworth-Mead
// Date         : 21/08/2016

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.UI
{
  public class Console : MonoBehaviour
  {
    private string _inputString = "";

    private GameObject _player;
    public  AStar _aStar;
    private Component _buildMgr;
    private Component _animalMgr;
    private Component _fundsMgr;

    private bool _consoleEnabled = true;    // Whether the console is shown and active
    private bool _swapConsoleFunction;      // Switch between entering data and finding IDs

    //List of spawnable items
    private List<string> _spawnItems = new List<string>();
    private List<string> _commands = new List<string>();

    private Transform _transform;

    // Rect for the console text box
    private Rect _consoleRect = new Rect(10, 10, 1000000, 20);

#pragma warning disable
    //Print stuff to Unity debugging console
    [SerializeField]
    private bool PRINT_LOADING_PREFABS, PRINT_SPAWNING_PREFABS;
#pragma warning restore

    private void Start()
    {
      //Find gameobjects etc.
      _player = GameObject.FindWithTag("Player");
      _buildMgr = GetComponent("BuildingManager");
      _animalMgr = GetComponent("AnimalManager");
      _fundsMgr = GetComponent("LogBookManager");

      //Filling SPAWN_ITEMS
      DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources");
      DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
      _spawnItems = new List<string>();
      foreach (DirectoryInfo dir in subDirectories)
      {
        if (PRINT_LOADING_PREFABS) Debug.Log("Searching directory: " + dir.Name);
        foreach (FileInfo file in dir.GetFiles())
        {
          string newString;
          if (file.Name.EndsWith("prefab"))
          {
            newString = dir.Name + "/" + file.Name.Substring(0, file.Name.IndexOf('.'));

            _spawnItems.Add(newString);
            if (PRINT_LOADING_PREFABS) Debug.Log("Loaded " + newString);
          }
        }
      }
      if (PRINT_LOADING_PREFABS) Debug.Log("End loading prefabs");
    }// Start()

    private void OnPointerDownDelegate(PointerEventData data)
    {
      print("test");
    }//OnPointerDownDelegate()

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.BackQuote))
      {
        _swapConsoleFunction = !_swapConsoleFunction;
      }

      if (!_consoleEnabled)
      {
        if (_buildMgr.GetComponent<Assets.Scripts.Managers.BuildingManager>()._currentBuild == null)
        {
          _consoleEnabled = true;
        }
      }
      else if (_swapConsoleFunction && _consoleEnabled && Input.GetMouseButtonDown(0) &&
          !_consoleRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
      {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        _inputString += "ID: " + hit.collider.GetInstanceID();
      }
    }// Update()

    public void Submit(string submitString)
    {
      _commands.Add(submitString);

      //KEYWORDS
      const string spawn = "spawn";
      const string cull = "cull";
      const string clear = "clear";
      const string destroy = "destroy";
      const string create = "create";
      const string fund = "fund";

      string[] inputParams = submitString.Split(' ');
      int inputParamsLength = inputParams.Length;
      string keyword = inputParams[0];

      switch (keyword)
      {
        #region Fund
        case fund:
          if (inputParamsLength == 1)
          {
            _fundsMgr.GetComponent<Assets.Scripts.Managers.LogBookManager>().ShowLog();
          }
          else if (inputParamsLength == 4)
          {
            Debug.Log("Create Command");
            float amount = float.Parse(inputParams[1]);
            string type = inputParams[2];
            string forWhat = inputParams[3];
            Receipt.Type curType = Receipt.Type.NA;
            switch(type)//{Product,Paid,Task,Payday,NA};
            {
              case "Product":
                curType = Receipt.Type.Product;
                break;
              case "Paid":
                curType = Receipt.Type.Paid;
                break;
              case "Task":
                curType = Receipt.Type.Task;
                break;
              case "Payday":
                curType = Receipt.Type.Payday;
                break;
              default:
                curType = Receipt.Type.NA;
                break;
            }
            if (amount > 0)//float amount, Receipt.Type type = Receipt.Type.NA, string whatFor = "N/A")
              _fundsMgr.GetComponent<Assets.Scripts.Managers.LogBookManager>().AddFunds(amount,curType,forWhat);
            else
              _fundsMgr.GetComponent<Assets.Scripts.Managers.LogBookManager>().AllocateFunds(amount,curType,forWhat);
          }
          break;
        #endregion
        
        #region Create

        case create:
          //FORMAT: create buildingname
          if (inputParamsLength == 2)
          {
            Debug.Log("Create Command");
            string type = inputParams[1];
            _buildMgr.GetComponent<Assets.Scripts.Managers.BuildingManager>().Create(type);

          }
          else if (inputParamsLength == 3)
          {
            Debug.Log("Pave Command");
            string type = inputParams[2];
            _buildMgr.GetComponent<Assets.Scripts.Managers.BuildingManager>().Pave(type);
          }
          break;

        #endregion


        #region Spawn

        case spawn:

          //FORMAT: spawn <type> <amount> <location> <additional params>
          if (inputParamsLength > 1)
          {

            string type = inputParams[1];

            _animalMgr.GetComponent<Assets.Scripts.Managers.AnimalManager>()
              .CreateFollowMouse(type);

          }
          else
          {
            _commands.Add("FORMAT: spawn <AnimalID>");
          }
          break;

        #endregion

        #region Cull

        case cull:
          break;

        #endregion Cull

        #region Destroy

        case destroy:
          if (inputParamsLength > 1)
          {
            GameObject go = GameObject.Find(inputParams[1]);
            print(go.name);
            Destroy(go);
            print(inputParams[1]);
          }
          break;

        #endregion Destroy

        #region Clear

        case clear:
          _commands.Clear();
          break;

          #endregion
      } // switch()
    } // Submit()

    private void OnGUI()
    {
      float xSize = _inputString.Length * 7.5f;
      xSize = Mathf.Clamp(xSize, 200, 400);
      Vector2 consoleSize = _consoleRect.size;
      consoleSize.x = xSize;
      _consoleRect.size = consoleSize;
      if (_consoleEnabled)
      {
        _inputString = GUI.TextField(new Rect(10, 10, consoleSize.x, 20), _inputString);
        if (Event.current.keyCode == KeyCode.Return && _inputString != "")
        {
          Submit(_inputString);
          ClearConsole();
          _consoleEnabled = false;
        }

        for (int i = 0; i < _commands.Count; i++)
        {
          GUI.Label(new Rect(10, i * 14 + 32, Screen.width, 20), _commands[_commands.Count - i - 1]);
        }
      }
    } // OnGUI()

    private void ClearConsole()
    {
      _inputString = "";
    }// ClearConsole()

    //Private methods
    private bool AddClassToGameObject(GameObject obj, string s)
    {
      if (!obj.AddComponent(Type.GetType(s)))
      {
        _commands.Add("Could not find \"" + s + "\" class");
        Destroy(obj);
        return false;
      }
      return true;
    } // AddClassToGameObject()
  }
}