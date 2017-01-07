using System.Collections.Generic;
using UnityEngine;

public class EnclosureBuilder : MonoBehaviour
{
    public LayerMask _layerMask;
    public GameObject _wallPrefab;
    public GameObject _cornerPrefab;
    public enum State { Idle, GettingFirstCornerPosition, GettingSecondCornerPosition };
    public State _state = State.Idle;
    private EnclosureBuilderTemplate _template;
    private List<GameObject> _tempWalls = new List<GameObject> ();
    private List<GameObject> _tempCorners = new List<GameObject> ();
    private Vector3 _cornerA;
    private Vector3 _cornerB;

    private void Update()
    {
        bool validMousePos = false;
        Vector3 mousePos = Vector3.zero;
        validMousePos = GetMousePosition (out mousePos);

        switch (_state) {
            case State.GettingFirstCornerPosition:
                if (Input.GetMouseButtonDown (0))
                {
                    if (validMousePos)
                    {
                        AssignFirstCorner (mousePos);
                    }
                }
                else if (Input.GetMouseButtonDown (1))
                {
                    CancelEnclosureBuild ();
                }
                break;

            case State.GettingSecondCornerPosition:
                if (validMousePos)
                {
                    _cornerB = mousePos;
                    DrawVisualizedEnclosure (_cornerA, _cornerB);
                }

                if (Input.GetMouseButtonDown (0))
                {
                    if (validMousePos)
                    {
                        FinalizeEnclosureBuild (_cornerA, _cornerB);
                    }
                }
                else if (Input.GetMouseButtonDown (1))
                {
                    CancelEnclosureBuild ();
                }
                break;
        }
    }

    private bool GetMousePosition(out Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit = new RaycastHit ();
        if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
        {
            mousePosition = rayHit.point;
            return true;
        }

        mousePosition = Vector3.zero;
        return false;
    }

    public void BeginBuildingNewEnclosure()
    {
        _template = new EnclosureBuilderTemplate ();
        _template.Initialize (_wallPrefab, _cornerPrefab);
        _state = State.GettingFirstCornerPosition;
    }

    private void AssignFirstCorner(Vector3 pos)
    {
        if (true) //Sanity check
        {
            _cornerA = pos;
            _state = State.GettingSecondCornerPosition;
        }
    }

    private void DrawVisualizedEnclosure(Vector3 firstCorner, Vector3 secondCorner)
    {
        //clean up old pieces
        //TODO: only clean up superfluous pieces
        if (_tempWalls.Count > 0)
        {
            foreach (GameObject g in _tempWalls)
            {
                _template.Destroy (EnclosureBuilderTemplate.ObjectType.Wall, g);
            }
        }

        if (_tempCorners.Count > 0)
        {
            foreach (GameObject g in _tempCorners)
            {
                _template.Destroy (EnclosureBuilderTemplate.ObjectType.Corner, g);
            }
        }

        //Visualize the enclosure
        List<GameObject>[] tempObjects = BuildSquareEnclosure (firstCorner, secondCorner);
        _tempWalls = tempObjects[0];
        _tempCorners = tempObjects[1];
    }

    private Vector3 ClampToTileSize(Vector3 v)
    {
        float tileSize = 2;
        v.x = v.x - (v.x % tileSize);
        v.y = v.y - (v.y % tileSize);
        v.z = v.z - (v.z % tileSize);
        return v;
    }

    private List<GameObject>[] BuildSquareEnclosure(Vector3 firstCornerPos, Vector3 secondCornerPos)
    {
        firstCornerPos = ClampToTileSize (firstCornerPos);
        secondCornerPos = ClampToTileSize (secondCornerPos);
        int height = (int) Mathf.Abs (secondCornerPos.z - firstCornerPos.z);//This needs to be relative to the camera angle right?
        int width = (int) Mathf.Abs (firstCornerPos.x - secondCornerPos.x);
        Vector3[] corners;
        if (firstCornerPos.z < secondCornerPos.z)
        {
            corners = new Vector3[]
            {
            firstCornerPos,
            firstCornerPos + new Vector3(0, 0, height),
            secondCornerPos,
            secondCornerPos + new Vector3(0, 0, -height)
            };
        }
        else
        {
            corners = new Vector3[]
            {
            firstCornerPos,
            firstCornerPos + new Vector3(0, 0, -height),
            secondCornerPos,
            secondCornerPos + new Vector3(0, 0, height)
            };
        }
        List<GameObject> wallObjects = new List<GameObject> ();
        wallObjects.AddRange (_template.InstantiateMultiple (EnclosureBuilderTemplate.ObjectType.Wall, (height * 2) + (width * 2)));
        int currentWallNum = 0;
        List<GameObject> cornerObjects = new List<GameObject> ();
        cornerObjects.AddRange (_template.InstantiateMultiple (EnclosureBuilderTemplate.ObjectType.Corner, 4));

        for (int i = 0; i < 4; i++)
        {
            //We move around from corner to corner in a clockwise fashion 
            Vector3 cornerA = corners[i];
            Vector3 cornerB;
            if (i == 3)
            {
                cornerB = corners[0];
            }
            else
            {
                cornerB = corners[i + 1];
            }

            cornerObjects[i].transform.position = corners[i];
            //TODO: rot

            int length = (int) ((Vector3.Distance(cornerA, cornerB) + 0.5f) / 2);   //length of a wall is 2
            for (int x = 0; x < length; x++)
            {
                GameObject g = wallObjects[currentWallNum];
                currentWallNum++;

                Vector3 position = cornerA + ((cornerB - cornerA).normalized * x * 2);
                Quaternion rotation = Quaternion.identity;

                g.transform.position = position;
                g.transform.rotation = rotation;
            }
        }

        return new [] { wallObjects, cornerObjects };
    }

    private void FinalizeEnclosureBuild(Vector3 firstCorner, Vector3 secondCorner)
    {
        //Create enclosure gameobject and add script etc to it
        //For obj returned from buildEnclosure set parent to enclosure
        GameObject enclosure = new GameObject ("Enclosure " + Time.realtimeSinceStartup);

        foreach (GameObject g in _tempCorners)
        {
            g.transform.parent = enclosure.transform;
        }
        _tempCorners.Clear ();

        foreach (GameObject g in _tempWalls)
        {
            g.transform.parent = enclosure.transform;
        }
        _tempWalls.Clear ();

        _template.DestroyAllHiddenObjects ();
        _template = null;

        _state = State.Idle;
    }

    private void CancelEnclosureBuild()
    {
        foreach (GameObject g in _tempCorners)
        {
            _template.Destroy (EnclosureBuilderTemplate.ObjectType.Corner, g);
        }
        _tempCorners.Clear ();

        foreach (GameObject g in _tempWalls)
        {
            _template.Destroy (EnclosureBuilderTemplate.ObjectType.Wall, g);
        }
        _tempWalls.Clear ();

        _template.DestroyAllHiddenObjects ();
        _template = null;
        _state = State.Idle;
    }

}   //EnclosureBuilder