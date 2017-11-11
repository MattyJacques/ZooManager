// Title        : FreeFormPathManager.cs
// Purpose      : This script will manage all of the 'FreeFormEnclosures' that are created using the 'FreeFormEnclosureBuilder'
// Author       : Tony Haggar
// Date         : 13/08/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFormPathManager : MonoBehaviour
{
    public FreeFormPathBuilder builder;

    //These are just passthrough's to the same method name within the 'FreeFormEnclosureBuilder'
    public void BuildNewFreeFormPath(bool curvedMode)
    {
        builder.BuildNewFreeFormPath(curvedMode);
    }

    public void BuildNewCirclePath()
    {
        builder.BuildNewCirclePath();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
