// Title        : EnclosureBuilder.cs
// Purpose      : This class allows for the building of enclosures
// Author       : Eivind Andreassen
// Date         : 20/12/2016

using UnityEngine;

public class EnclosureBuilder : MonoBehaviour {
    public enum State { Idle, WaitingForFirstCornerInput, DrawingEnclosureSquare };
    public State _state;
    public LayerMask _layerMask;
    public float _enclosureColliderHeight = 2f;
    public int _minimumEnclosureWidth = 2;
    public int _minimumEnclosureHeight = 2;
    public int _maximumEnclosureWidth = 20;
    public int _maximumEnclosureHeight = 20;

    //The different GameObjects that are used when editing the enclosure
    public GameObject _selectedWall;
    public GameObject _selectedWallCorner;
    public GameObject _selectedWallDoor;
    public GameObject _selectedWallWindow;  //TODO: implement this

    private Vector3 _bottomLeftCorner;
    private Vector3 _topRightCorner;
    private GameObject _visualizationRectangle;

    private void Update()
    {   //Processes player input based on EnclosureBuilder state

        Vector3 mousePosition;
        switch (_state) {
            case State.WaitingForFirstCornerInput:
                if (Input.GetMouseButtonDown (0))
                {
                    if (GetWorldPositionOfMouseClamped(_layerMask, out mousePosition))
                    {
                        AssignFirstCorner (mousePosition);
                    }
                }

                if (Input.GetMouseButtonDown (1))
                {
                    CancelBuilding ();
                }
                break;

            case State.DrawingEnclosureSquare:
                if (GetWorldPositionOfMouseClamped(_layerMask, out mousePosition))
                {
                    _topRightCorner = mousePosition;
                }
                else
                {
                    break;
                }

                DrawEnclosureArea (_bottomLeftCorner, _topRightCorner);

                if (Input.GetMouseButtonDown (0))
                {
                    FinalizeBuilding (_bottomLeftCorner, _topRightCorner);
                }

                if (Input.GetMouseButtonDown (1))
                {
                    CancelBuilding ();
                }
                break;
        }
    }   //Update()

    public bool GetWorldPositionOfMouseClamped(LayerMask layerMask, out Vector3 position)
    {   //Returns the player's mouse position in world space
        
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit = new RaycastHit ();
        if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
        {
            position = rayHit.point;
            position.x -= position.x % 2;
            position.y -= position.y % 2;
            position.z -= position.z % 2;
            return true;
        }
        else
        {
            position = Vector3.zero;
            return false;
        }
        
    }   //GetMousePosition

    public void BeginBuildingEnclosure()
    {
        _state = State.WaitingForFirstCornerInput;
    }

    private void AssignFirstCorner(Vector3 bottomLeftCorner)
    {
        _bottomLeftCorner = bottomLeftCorner;
        _state = State.DrawingEnclosureSquare;
        CreateNewVisualizationRectangle ();
    }

    private void CreateNewVisualizationRectangle()
    {
        _visualizationRectangle = GameObject.CreatePrimitive (PrimitiveType.Quad);
        _visualizationRectangle.transform.eulerAngles = new Vector3 (90f, 0f, 0f);
    }

    private void DrawEnclosureArea(Vector3 cornerBL, Vector3 cornerTR)
    {   //Visualizes the enclosure area for the player
        Vector3 newPos = Vector3.Lerp (cornerBL, cornerTR, 0.5f);
        newPos.y = 0.1f;
        _visualizationRectangle.transform.position = newPos;

        Vector3 newScale = new Vector3(1f, 1f, 1f);
        newScale.x = Mathf.Abs(cornerBL.x - cornerTR.x);
        newScale.y = Mathf.Abs(cornerBL.z - cornerTR.z);
        _visualizationRectangle.transform.localScale = newScale;
    }   //DrawEnclosure()

    private void CancelBuilding()
    {
        _state = State.Idle;
        Destroy (_visualizationRectangle);
    }

    private void FinalizeBuilding(Vector3 cornerBL, Vector3 cornerTR)
    {   //Checks if the shape of the enclosure is OK, if it is then it cleans up and creates the enclosure

        float rectangleWidth = Mathf.Abs (cornerBL.x - cornerTR.x);
        float rectangleHeight = Mathf.Abs (cornerBL.z - cornerTR.z);

        //Check if the size is within bounds
        if (rectangleWidth < _minimumEnclosureWidth
            || rectangleHeight < _minimumEnclosureHeight
            || rectangleWidth > _maximumEnclosureWidth
            || rectangleHeight > _maximumEnclosureHeight)
        {
            Debug.Log ("Can't build enclosure! Enclosure does not fit within size limits! "
                + "\nHeight min: " + _minimumEnclosureHeight.ToString () + ", height max: "
                + _maximumEnclosureHeight.ToString () + ", current height: " + rectangleHeight.ToString ()
                + ".\nWidth min: " + _minimumEnclosureWidth.ToString () + ", width max: "
                + _maximumEnclosureWidth.ToString () + ", current width: " + rectangleWidth.ToString ());
            return;
        }

        //Check if the size is a power of two
        //Round to int to account for any floating point tomfoolery
        if ((int) (rectangleWidth + 0.5f) % 2 != 0
            || (int) (rectangleHeight + 0.5f) % 2 != 0)
        {
            Debug.Log ("Can't build enclosure! Enclosure size is not a power of two! Width: " 
                + rectangleWidth.ToString () + ", height: " + rectangleHeight.ToString () + ".");
            return;
        }

        //Clean up
        _state = State.Idle;
        Destroy (_visualizationRectangle);

        //Create enclosure
        GameObject enclosure = new GameObject ("Enclosure");
        enclosure.transform.position = Vector3.Lerp (cornerBL, cornerTR, 0.5f);
        enclosure.AddComponent<Enclosure> ();
        enclosure.GetComponent<Enclosure> ().Rename ("New Enclosure");

        BoxCollider col = enclosure.AddComponent<BoxCollider> ();
        col.size = new Vector3 (rectangleWidth, _enclosureColliderHeight, rectangleHeight);
        col.center = new Vector3 (0f, _enclosureColliderHeight / 2, 0f);
    }   //FinalizeBuilding()

}   //EnclosureBuilder