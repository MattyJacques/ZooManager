// Title        : Enclosure.cs
// Purpose      : This class controls the enclosure
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enclosure : MonoBehaviour
{
    private List<InteriorItem> _interiorItems = new List<InteriorItem> ();

    public class InteriorItem {
        public Transform transform;
        public Type type;
        public enum Type { Water, Food };
    }

    private void Start()
    {   //Initialization
        if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
        {
            Debug.LogError ("Enclosure " + name + " at " + transform.position.ToString () + " has no collider!");
        }
    }   //Start()

    public void Interact()
    {   //Opens the GUI that allows for player interaction

        //Delete
        //Resize
        //Edit interior items
            //Lights up somehow, makes interior objects moveable within the enclosure, debug buttons for adding items

    }   //Interact()

    public Vector3 GetClosest(InteriorItem.Type itemType)
    {   //Returns the closest object of itemType
        return Vector3.zero;
    }   //GetClosest()

}