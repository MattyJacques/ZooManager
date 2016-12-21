// Title        : EnclosureInteraction.cs
// Purpose      : This is a temporary class that allows the player to interact with enclosures
// Author       : Eivind Andreassen
// Date         : 20/12/2016
using UnityEngine;

public class EnclosureInteraction : MonoBehaviour {
    public LayerMask _layerMask;
    public LayerMask _uiLayer;

	void Update () {
        if (Input.GetMouseButtonDown (0))
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit rayHit = new RaycastHit ();

            //If we're clicking a UI element, don't interact with the enclosure
            if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _uiLayer, QueryTriggerInteraction.Collide))
            {
                return;
            }

            if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Enclosure enclosure = rayHit.collider.GetComponent<Enclosure> ();
                if (enclosure != null)
                {
                    enclosure.Interact ();
                }
            }
        }
	}
}
