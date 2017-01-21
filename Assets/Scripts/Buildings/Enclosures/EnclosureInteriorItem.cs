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
    public string prefabLocation { get; set; }

    public enum ItemType
    {
        Decorative, //Default, or is used if the enum can't be parsed
        Water,
        Food
    }

    public EnclosureInteriorItem(JSONNode jsonData)
    {
        name = jsonData["name"];
        cost = jsonData["cost"].AsInt;
        prefabLocation = jsonData["prefabLocation"];
        if (Enum.IsDefined (typeof (ItemType), jsonData["type"]))
        {
            type = (ItemType) Enum.Parse (typeof (ItemType), jsonData["type"], true);
        }
        else
        {
            type = ItemType.Decorative;
        }
        positionOffset = offsetToVector3 (jsonData["positionOffset"]);
        rotationOffset = offsetToVector3 (jsonData["rotationOffset"]);
    }

    private Vector3 offsetToVector3(JSONNode offset) {
        string f = offset.ToString ();
        Vector3 v = new Vector3 ();
        v.x = offset[0].AsFloat;
        v.y = offset[1].AsFloat;
        v.z = offset[2].AsFloat;
        return v;
    }
}