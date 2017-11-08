// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Enclosure;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnclosureInteraction : MonoBehaviour
{
  private LayerMask _layerMask;

  void Start()
  {
    _layerMask = 1<<LayerMask.NameToLayer("Enclosure"); //Interacts with all layers
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
        EnclosureComponent enclosure = rayHit.collider.GetComponent<EnclosureComponent>();
        if (enclosure != null)
        {
          enclosure.OnClick();
        }
      }
    }
  } // Update()

} // EnclosureInteraction
