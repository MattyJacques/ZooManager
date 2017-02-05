// Title        : EnclosureInteriorItem.cs
// Purpose      : This is a container for the EnclosureInteriorItem object
// Author       : Eivind Andreassen
// Date         : 26.01.2017

using UnityEngine;

public class EnclosureInteriorItem
{
  public GameObject gameObject;
  public Transform transform;
  public InteriorItemType type;

  public EnclosureInteriorItem(GameObject gameObject, InteriorItemType type)
  {
    transform = gameObject.transform;
    this.gameObject = gameObject;
    this.type = type;
  } // EnclosureInteriorItem()

  public enum InteriorItemType
  {
    Food,
    Water,
    Fun
  }

} // EnclosureInteriorItem
