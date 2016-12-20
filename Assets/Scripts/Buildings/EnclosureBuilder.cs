// Title        : EnclosureBuilder.cs
// Purpose      : This class allows for the building of enclosures
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosureBuilder : MonoBehaviour {
    public enum State { Idle, DrawingEnclosureSquare };
    public State _state;

    private Vector3 _bottomLeftCorner;

    private void Update()
    {
        switch (_state) {
            case State.DrawingEnclosureSquare:
                if (Input.GetMouseButtonDown (0))
                {
                    FinalizeBuilding ();
                }

                if (Input.GetMouseButtonDown (1))
                {
                    CancelBuilding ();
                }
                break;
        }
    }

    public void BeginBuildingEnclosure(Vector3 bottomLeftCorner)
    {
        _bottomLeftCorner = bottomLeftCorner;
        _state = State.DrawingEnclosureSquare;
    }

    private void CancelBuilding()
    {
        _state = State.Idle;
    }

    private void FinalizeBuilding()
    {
        _state = State.Idle;
    }
}
