// Title        : EnclosureBuilderGUI.cs
// Purpose      : Creates a GUI button that begins the enclosure building process
// Author       : Eivind Andreassen
// Date         : 21/12/2016
using UnityEngine;

public class EnclosureBuilderGUI : MonoBehaviour
{   //An example of how to use the EnclosureBuilder class

    public EnclosureBuilder _targetEnclosureBuilder;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 150, 25), "Build new enclosure")) {
            _targetEnclosureBuilder.BeginBuildingNewEnclosure ();
        }
    } 
}   //EnclosureBuilderGUI
