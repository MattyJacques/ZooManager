// Title        : GameClock.cs
// Purpose      : Open console with ` or '. 
//                Submit with "enter" or "return".
//				  Use the console to spawn items in the "Resources" directory.
//				  spawn directory/prefab x y z
// Author       : Dan Budworth-Mead
// Date         : 21/08/2016

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Console : MonoBehaviour
{
    private string inputString = "";

    private GameObject player;

    private bool consoleEnabled;

    //Console Keywords
    const string KEYWORD_SPAWN = "spawn";

    //List of spawnable items
    private List<string> SPAWN_ITEMS;

    //Print stuff to Unity debugging console
    [SerializeField]
    bool PRINT_LOADING_PREFABS, PRINT_SPAWNING_PREFABS;

    void Start()
    {
        //Find gameobjects etc.
        player = GameObject.FindWithTag("Player");

        //Filling SPAWN_ITEMS
        DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources");
        DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

        SPAWN_ITEMS = new List<string>();
        if (PRINT_LOADING_PREFABS) Debug.Log("Begin loading prefabs");
        foreach (DirectoryInfo dir in subDirectories)
        {
            if (PRINT_LOADING_PREFABS) Debug.Log("Searching directory: "+dir.Name);
            foreach (FileInfo file in dir.GetFiles())
            {
                string newString;
                if (file.Name.EndsWith("prefab"))
                {
                    newString = dir.Name + "/" + file.Name.Substring(0, file.Name.IndexOf('.'));

                    SPAWN_ITEMS.Add(newString);
                    if (PRINT_LOADING_PREFABS) Debug.Log("Loaded " + newString);
                }
            }
        }
        if (PRINT_LOADING_PREFABS) Debug.Log("End loading prefabs");

        consoleEnabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            consoleEnabled = !consoleEnabled;
        }
    }

    void Submit()
    {
        string submitText = inputString;
        submitText = submitText.ToLower();

        if (submitText.StartsWith(KEYWORD_SPAWN))
        {
            foreach (string ITEM_TO_SPAWN in SPAWN_ITEMS)
            {
                if (submitText.Contains(ITEM_TO_SPAWN))
                {
                    GameObject spawningThis = (GameObject)Instantiate(Resources.Load(ITEM_TO_SPAWN));
                    spawningThis.name = ITEM_TO_SPAWN;

                    Vector3 spawnPos = player.transform.position;
                    if (submitText.Length > submitText.IndexOf(ITEM_TO_SPAWN) + ITEM_TO_SPAWN.Length)
                    {
                        string xyzString = "";
                        string[] xyzStrings = new string[3];
                        xyzString = submitText.Substring(submitText.IndexOf(ITEM_TO_SPAWN) + ITEM_TO_SPAWN.Length + 1);
                        if (xyzString != "")
                        {
                            xyzStrings = xyzString.Split(' ');

                            spawnPos = player.transform.position;
                            spawnPos.x = float.Parse(xyzStrings[0]);
                            spawnPos.y = float.Parse(xyzStrings[1]);
                            spawnPos.z = float.Parse(xyzStrings[2]);
                        }
                    }
                    spawningThis.transform.position = spawnPos;

                    if (PRINT_SPAWNING_PREFABS) Debug.Log("Spawning " + spawningThis.name + " at X=" + spawnPos.x + " Y=" + spawnPos.y + " Z=" + spawnPos.z);
                }
            }
        }
        inputString = "";
    }

    void ClearField()
    {
    }
    
    void OnGUI()
    {
        if (consoleEnabled)
        {
            float xSize = inputString.Length * 7.5f;
            xSize = Mathf.Clamp(xSize, 200, 400);
            inputString = GUI.TextField(new Rect(10, 10, xSize, 20), inputString, 50);
            if (Event.current.keyCode == KeyCode.Return)
            {
                Submit();
                inputString = "";
            }
        }
    }
}