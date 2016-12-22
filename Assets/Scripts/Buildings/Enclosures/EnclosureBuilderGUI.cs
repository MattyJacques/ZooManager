using UnityEngine;

public class EnclosureBuilderGUI : MonoBehaviour
{   //An example of how to use the EnclosureBuilder class

    public EnclosureBuilder _targetEnclosureBuilder;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 80, 25), "Build enclosure")) {
            _targetEnclosureBuilder.BeginBuildingEnclosure ();
        }
    } 
}   //EnclosureBuilderGUI
