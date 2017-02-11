// Title        : EnclosureInteraction.cs
// Purpose      : This is a demonstration of a class that allows the player to interact with existing enclosures
// Author       : Eivind Andreassen
// Date         : 20/12/2016
using UnityEngine;
using UnityEngine.EventSystems;

public class EnclosureInteraction : MonoBehaviour
{
  private LayerMask _layerMask;

  void Start()
  {
    _layerMask = -1; //Interacts with all layers
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit rayHit = new RaycastHit();

      //If we're clicking a UI element, don't interact with the enclosure
      if (EventSystem.current != null)
      {
        if (EventSystem.current.IsPointerOverGameObject ())
        {
          return;
        }
      }

      if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
      {
        Enclosure enclosure = rayHit.collider.GetComponent<Enclosure>();
        if (enclosure != null)
        {
          enclosure.Interact();
        }
      }
    }
  } // Update()
} // EnclosureInteraction()
