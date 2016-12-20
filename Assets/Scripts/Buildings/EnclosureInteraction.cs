// Title        : EnclosureInteraction.cs
// Purpose      : This is a temporary class that allows the player to interact with enclosures
// Author       : Eivind Andreassen
// Date         : 20/12/2016
using UnityEngine;

public class EnclosureInteraction : MonoBehaviour {
    public LayerMask _layerMask;

	void Update () {
        if (Input.GetMouseButtonDown (0))
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit rayHit = new RaycastHit ();
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
