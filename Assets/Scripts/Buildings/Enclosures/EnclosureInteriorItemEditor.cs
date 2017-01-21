using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EnclosureInteriorItemEditor : MonoBehaviour
{
    public enum State
    {
        Inactive,
        EditingPlacementOfItem
    }
    public State _state;

    private void Update()
    {
        if (_state == State.EditingPlacementOfItem)
        {

        }
    }

    public void StartEditingPlacementOfItem(EnclosureInteriorItem item, Vector3 cornerA, Vector3 cornerB)
    {   

    }
}