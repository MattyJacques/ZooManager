﻿// Title        : EnclosureInteriorItem.cs
// Purpose      : Stores the values of an enclosure's interior item
// Author       : Eivind Andreassen
// Date         : 21.01.2017

using UnityEngine;
using SimpleJSON;
using System;

public class EnclosureInteriorItem
{
    public string name { get; set; }
    public ItemType type { get; set; }
    public int cost { get; set; }
    public Vector3 positionOffset { get; set; }
    public Vector3 rotationOffset { get; set; }
    private string prefabLocation { get; set; }
    private Transform _transform = null;

    public Transform transform
    { //Extra checks to make sure the transform isn't being used wrong
        get {
            if (_transform == null)
            {
                Debug.LogError ("EnclosureInteriorItem " + name + " does not have a transform registered!"
                    + " Have you called Init() before trying to access the transform?");
                return null;
            }
            else
            {
                return _transform;
            }
        }
        set {
            if (_transform != null)
            {
                Debug.LogWarning ("EnclosureInteriorItem already has a transform registered!\n"
                    + name.ToString () + " has transform " + transform.name + " that exists at "
                    + transform.position.ToString ());
            }
            else
            {
                _transform = value;
            }
        }
    }


    public enum ItemType
    {
        Decorative, //Default, or is used if the enum can't be parsed
        Water,
        Food
    }

    public EnclosureInteriorItem(JSONNode jsonData)
    { //Constructor creates a EnclosureInteriorItem from the JSON data stored in the BuildingData file
        name = jsonData["name"];
        cost = jsonData["cost"].AsInt;
        prefabLocation = jsonData["prefabLocation"];
        if (Enum.IsDefined (typeof (ItemType), jsonData["type"].ToString()))
        {
            type = (ItemType) Enum.Parse (typeof (ItemType), jsonData["type"].ToString(), true);
        }
        else
        {   //If we can't parse the type, set it to decorative
            Debug.LogWarning ("Could not recognize type of " + jsonData["type"].ToString ()
                + ". Please make sure the type is set correctly in the JSON file."
                + " Defaulting type do " + ItemType.Decorative.ToString()
                + "Json dump:\n" + jsonData.ToString());
            type = ItemType.Decorative;
        }
        positionOffset = OffsetToVector3 (jsonData["positionOffset"]);
        rotationOffset = OffsetToVector3 (jsonData["rotationOffset"]);
    } //EnclosureInteriorItem

    public void Instantiate(Vector3 position)
    { //Instantiates the transform based on the prefabLocation
        //TODO: optimize by only loading the object once into the static list and instantiating from there
        transform = UnityEngine.Object.Instantiate (Resources.Load(prefabLocation) as GameObject).transform;
        transform.position = position + positionOffset;
        transform.Rotate (rotationOffset);  //NOTE: This might not work
        transform.name = "InteriorItem_" + name;
    } //Instantiate

    private Vector3 OffsetToVector3(JSONNode offset)
    { //Convert the JSONNode array [0, 0, 0] to a vector3
        string f = offset.ToString ();
        Vector3 v = new Vector3 ();
        v.x = offset[0].AsFloat;
        v.y = offset[1].AsFloat;
        v.z = offset[2].AsFloat;
        return v;
    } //OffsetToVector3
}