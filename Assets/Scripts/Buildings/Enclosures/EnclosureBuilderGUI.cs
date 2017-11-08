// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Buildings.Enclosures
{
    public class EnclosureBuilderGUI : MonoBehaviour
    {   //An example of how to use the EnclosureBuilder class

        public EnclosureBuilder _targetEnclosureBuilder;

        public void OnGUI()
        {
            if (GUI.Button(new Rect(0, 50, 150, 25), "Build new enclosure")) {
                _targetEnclosureBuilder.BeginBuildingNewEnclosure ();
            }
        } // OnGUI()
    }
}   //EnclosureBuilderGUI
