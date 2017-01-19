using UnityEngine;
using SimpleJSON;

public class EnclosureInteriorItem
{
    public EnclosureInteriorItem(JSONNode jsonData)
    {
        name = jsonData["name"];
        cost = jsonData["cost"].AsInt;
        prefabLocation = jsonData["prefabLocation"];
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

    public string name { get; set; }
    public string type { get; set; }    //TODO: should be enum
    public int cost { get; set; }
    public Vector3 positionOffset { get; set; }
    public Vector3 rotationOffset { get; set; }
    public string prefabLocation { get; set; }
}