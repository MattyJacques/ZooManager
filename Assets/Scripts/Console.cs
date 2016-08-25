﻿// Title        : GameClock.cs
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
using System.Text.RegularExpressions;

public class Console : MonoBehaviour
{
    private string inputString = "";

    private GameObject player;

    private bool consoleEnabled;

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
        foreach (DirectoryInfo dir in subDirectories)
        {
            if (PRINT_LOADING_PREFABS) Debug.Log("Searching directory: " + dir.Name);
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

    int Submit(string submitString)
    {
        //spawn foo/foo/bar x y z rot
        Regex regex = new Regex("");
        Match match = Regex.Match(submitString, "spawn");
        if (match.Success)
        {
            GameObject newObject;

            string[] split = Regex.Split(submitString, " ");

            SPAWN_ITEMS.ForEach(item =>
            {
                if (Regex.Match(submitString, item).Success)
                {                    
                    newObject = new GameObject();
                    if (item.Contains("/"))
                    {
                        newObject.AddComponent(Type.GetType(item.Substring(item.LastIndexOf("/"))));
                    }
                    else
                    {
                        newObject.AddComponent(Type.GetType(item));
                    }
                }
            });

            if (split.Length > 2)
            {
                string[] sPos = Regex.Split(split[2], ",");
                Vector3 v3Pos = sPos.ParseVec3();
            }
        }
        else
        {
        }
        return 1;
    }

    Vector3 Parse()
    {
        return Vector3.zero;
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
            if (Event.current.keyCode == KeyCode.Return && inputString != "")
            {
                Submit(inputString);
                inputString = "";
            }
        }
    }
}