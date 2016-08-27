// Title        : GameClock.cs
// Purpose      : Open console with ` or '. 
//                Submit with "enter" or "return".
//				  Use the console to spawn items in the "Resources" directory.
//				  spawn <Id/Name/Tag> <Amount> <Position> <Other>
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
using System.Text.RegularExpressions;

namespace Assets.Scripts
{
    public class Console : MonoBehaviour
    {
        private string _inputString = "";

        private GameObject _player;

        private bool _consoleEnabled;

        //List of spawnable items
        private List<string> _spawnItems = new List<string>();
        private List<string> _commands = new List<string>();

        private Transform _transform;

        private Rect _consoleRect = new Rect(10, 10, 1000000, 20);

        //Print stuff to Unity debugging console
        [SerializeField] private bool PRINT_LOADING_PREFABS, PRINT_SPAWNING_PREFABS;

        private void Start()
        {
            //Find gameobjects etc.
            _player = GameObject.FindWithTag("Player");

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

            _consoleEnabled = false;
        }

        private void OnPointerDownDelegate(PointerEventData data)
        {
            print("test");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _consoleEnabled = !_consoleEnabled;
            }

            if (_consoleEnabled && Input.GetMouseButtonDown(0) &&
                !_consoleRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);
                _inputString += hit.collider.GetInstanceID();
            }
        }

        public void Submit(string submitString)
        {
            _commands.Add(submitString);

            //KEYWORDS
            const string spawn = "spawn";
            const string cull = "cull";
            const string clear = "clear";
            const string destroy = "destroy";

            string[] inputParams = submitString.Split(' ');
            int inputParamsLength = inputParams.Length;
            string keyword = inputParams[0];

            switch (keyword)
            {
                    #region Spawn

                case spawn:
                    //FORMAT: spawn <type> <amount> <location> <additional params>
                    if (inputParamsLength > 1)
                    {
                        string type = inputParams[1];
                        inputParams.GetValue(1);
                        int amount = 1;
                        string location = "";

                        foreach (string s in inputParams)
                        {
                            if (s.StartsWith("("))
                            {
                                location = s;
                                break;
                            }
                        }

                        if (inputParamsLength > 2)
                        {
                            if (inputParams[2].IsNumeric())
                            {
                                amount = int.Parse(inputParams[2]);
                            }
                            else
                            {
                                amount = 1;
                            }
                        }

                        //Spawn the require amount
                        for (int i = 0; i < amount; i++)
                        {
                            GameObject objectToSpawn = new GameObject();
                            objectToSpawn.name = objectToSpawn.GetInstanceID().ToString();

                            #region Add Class

                            if (type.Contains("/"))
                            {
                                if (!AddClassToGameObject(objectToSpawn, type.Substring(type.LastIndexOf("/"))))
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (!AddClassToGameObject(objectToSpawn, type))
                                {
                                    break;
                                }
                            }

                            #endregion Add Class

                            #region Move it to correct location

                            if (location.Contains("(") && location.Contains(")"))
                            {
                                objectToSpawn.transform.position =
                                    location.Substring(location.IndexOf('(') + 1, location.Length - 2)
                                        .Split(',')
                                        .ParseVec3();
                            }
                            else
                            {
                                objectToSpawn.transform.position = _transform.position;
                            }

                            #endregion Move it to correct location

                            #region Begin dealing with additional parameters

                            for (int j = 0; j < inputParamsLength; j++)
                            {
                                string currentParam = inputParams[j];
                                if (currentParam.Contains(":"))
                                {
                                    int indexOfColon = currentParam.IndexOf(':');
                                    string param = currentParam.Substring(0, indexOfColon - 1);
                                    float value = float.Parse(currentParam.Substring(indexOfColon + 1));
                                }
                            }

                            #endregion End dealing with additional parameters
                        }
                    }
                    else
                    {
                        _commands.Add("FORMAT: spawn <type> <amount> <location> <additional params>");
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
            }
        }

        private void OnGUI()
        {
            float xSize = _inputString.Length*7.5f;
            xSize = Mathf.Clamp(xSize, 200, 400);
            Vector2 consoleSize = _consoleRect.size;
            consoleSize.x = xSize;
            _consoleRect.size = consoleSize;
            _inputString = GUI.TextField(new Rect(10, 10, consoleSize.x, 20), _inputString);
            if (Event.current.keyCode == KeyCode.Return && _inputString != "")
            {
                Submit(_inputString);
                ClearConsole();
            }

            for (int i = 0; i < _commands.Count; i++)
            {
                GUI.Label(new Rect(10, i*14 + 32, Screen.width, 20), _commands[_commands.Count - i - 1]);
            }
        }

        private void ClearConsole()
        {
            _inputString = "";
        }

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
        }
    }
}