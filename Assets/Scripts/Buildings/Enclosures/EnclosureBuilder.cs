// Title        : EnclosureBuilder.cs
// Purpose      : Visualizes and builds enclosures based on player input
// Author       : Eivind Andreassen
// Date         : 18.01.2017

using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
//TODO: Col test from corner to corner with box cast(?)

public class EnclosureBuilder : MonoBehaviour
{
    public enum State { Idle, GettingFirstCornerPosition, GettingSecondCornerPosition };
    public State _state = State.Idle;
    public LayerMask _layerMask; //The layers we want to be able to place an enclosure on
    public GameObject _wallPrefab; //The prefab we want to build this enclosure out of
    public GameObject _cornerPrefab;
    public Material _wallCantBuildMaterial;
    public Material _cornerCantBuildMaterial;
    public GameObject _finalizeBuildDustEffect;
    public AudioClip _finalizeBuildAudioClip;

    private EnclosureBuilderPool _pool; //The pool we get out gameObjects from
    private List<GameObject> _tempWalls = new List<GameObject> (); //The objects we are currently using(not pooled/deleted) to create the enclosure
    private List<GameObject> _tempCorners = new List<GameObject> ();
    private Vector3 _cornerA;   //The first corner of the enclosure
    private Vector3 _cornerB;   //The second corner of the enclosure

    //The offsets account for the models rotation not being imported at zero..
    //If the pipeline ever changes and the models are imported with a different rotation this needs to be changed
    private Vector3 cornerRotationOffset = new Vector3 (0f, 0f, 90f);
    //To account for the corner's pivot not being at 0 for the specific model used in the prototype
    private Vector3 cornerPositionOffset = new Vector3 (0f, -1f, 0f);   
    private Vector3 wallRotationOffset = new Vector3 (0f, 0f, 90f);

    private void Update()
    {
        bool validMousePos = false;
        Vector3 mousePos = Vector3.zero;
        validMousePos = InputHelper.GetMousePositionInWorldSpace(out mousePos, _layerMask.value);

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
                        if (EnclosureParametersAreSane (_cornerA, _cornerB))
                        {
                            FinalizeEnclosureBuild (_cornerA, _cornerB);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown (1))
                {
                    CancelEnclosureBuild ();
                }
                break;
        }
    }

    public void BeginBuildingNewEnclosure()
    {
        _pool = new EnclosureBuilderPool ();
        _pool.Initialize (_wallPrefab, _cornerPrefab, _wallCantBuildMaterial, _cornerCantBuildMaterial);
        _state = State.GettingFirstCornerPosition;
    }

    private void AssignFirstCorner(Vector3 pos)
    {
        if (true) //TODO: Sanity check
        {
            _cornerA = pos;
            _state = State.GettingSecondCornerPosition;
        }
    }
    private bool EnclosureParametersAreSane(Vector3 firstCorner, Vector3 secondCorner)
    {
        int height = (int) Mathf.Abs (secondCorner.z - firstCorner.z) / 2;
        int width = (int) Mathf.Abs (firstCorner.x - secondCorner.x) / 2;

        if (height < 2 || width < 2)
        {
            return false;
        }

        return true;
    }

    private void DrawVisualizedEnclosure(Vector3 firstCorner, Vector3 secondCorner)
    {
        //clean up old pieces
        //TODO: only clean up superfluous pieces
        if (_tempWalls.Count > 0)
        {
            foreach (GameObject g in _tempWalls)
            {
                _pool.Destroy (EnclosureBuilderPool.ObjectType.Wall, g);
            }
        }

        if (_tempCorners.Count > 0)
        {
            foreach (GameObject g in _tempCorners)
            {
                _pool.Destroy (EnclosureBuilderPool.ObjectType.Corner, g);
            }
        }

        //Visualize the enclosure
        List<GameObject>[] tempObjects;
        tempObjects = BuildSquareEnclosure (firstCorner, secondCorner);
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

    private int AngleBetweenVector2(Vector2 vectorA, Vector2 vectorB)
    {
        Vector2 difference = vectorB - vectorA;
        float sign = (vectorB.y < vectorA.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, difference) * sign;

        //How do I even math
        angle += 180; //All this is just to get the angle that we need for this specific case
        angle /= 90;    
        angle -= angle % 1;
        angle = 3 - angle;
        angle -= 1;
        if (angle < 0)
        {
            angle = 3;
        }
        return (int)angle;
    }

    private List<GameObject>[] BuildSquareEnclosure(Vector3 firstCornerPos, Vector3 secondCornerPos)
    {
        int angle = AngleBetweenVector2 (new Vector2 (firstCornerPos.x, firstCornerPos.z), new Vector2 (secondCornerPos.x, secondCornerPos.z));

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
        //2 height = length of 1 wall, but we need one wall on both sides per height.
        //-4 to account for the corners at the start and end of the sides
        int amountOfWallsNeeded = Mathf.Clamp (height - 4, 0, 10000);  
        amountOfWallsNeeded += Mathf.Clamp (width - 4, 0, 10000);
        wallObjects.AddRange (_pool.InstantiateMultiple (EnclosureBuilderPool.ObjectType.Wall, amountOfWallsNeeded));
        int currentWallNum = 0;

        List<GameObject> cornerObjects = new List<GameObject> ();
        cornerObjects.AddRange (_pool.InstantiateMultiple (EnclosureBuilderPool.ObjectType.Corner, 4));

        //Place corners
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

            //This rotation shit is 99% black magic because I'm awful at math. Don't touch any of the rotation stuff unless you want a migraine
            //Okay.. so we rotate the corners by the cornerRotationOffset based on which corner it is..
            cornerObjects[i].transform.Rotate (cornerRotationOffset * (2 + i));

            //And then.. We rotate the corners based on the angle from the first corner to the second corner
            //This is like 99% trial and error, and it all depends on the corner prefabs having a rotation that can be offset by the cornerRotationOffset
            //Somehow the rotation of the reference object in the EnclosureBuilderPool changes between the times it's being instantiated
            //and instead of fixing that, I did.. this
            //If you want to fix this, you need to change all of this code, fix the bug with the rotation changing in the pool, and edit the AngleBetweenVector2 method
            if (angle % 2 == 0)
            {
                cornerObjects[i].transform.Rotate (cornerRotationOffset * angle);
            }
            else
            {
                int val = 1;
                if (angle == 3)
                {
                    if (i % 2 == 0)
                    {
                        val = 1;
                    }
                    else
                    {
                        val = 3;
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        val = 3;
                    }
                }
                //From corner 0 to corner 3, val should be 3, 1, 3, 1 when angle is 1, and 1, 3, 1, 3 when angle is 3
                cornerObjects[i].transform.Rotate (-Vector3.forward, val * 90);
            }

            cornerObjects[i].transform.Translate (cornerPositionOffset, Space.Self);

            //Place walls
            int length = (int) ((Vector3.Distance(cornerA, cornerB) + 0.5f) / 2);   //length of a wall is 2
            for (int x = 1; x < length - 1; x++)
            {
                GameObject g = wallObjects[currentWallNum];
                currentWallNum++;

                const float wallLength = 2;
                Vector3 position = cornerA + ((cornerB - cornerA).normalized * x * wallLength);

                //Move the wall 1 unit forward, since the corner occupies only 1 unity and not 1 in either direction
                const float cornerOffset = 1; 
                position += (cornerB - cornerA).normalized * cornerOffset;

                g.transform.position = position;

                //TODO: rotate the walls based on the angle.. or will anyone even notice
                g.transform.Rotate (wallRotationOffset * (i + 1));
            }
        }

        return new [] { wallObjects, cornerObjects };
    }

    private void FinalizeEnclosureBuild(Vector3 firstCorner, Vector3 secondCorner)
    {
        firstCorner = ClampToTileSize (firstCorner);
        secondCorner = ClampToTileSize (secondCorner);

        //Create the enclosure object
        Vector3 centreOfEnclosure = Vector3.Lerp (firstCorner, secondCorner, 0.5f);
        GameObject enclosure = new GameObject ("Enclosure " + Time.realtimeSinceStartup);
        enclosure.transform.position = centreOfEnclosure;
        enclosure.AddComponent<Enclosure> ();

        BoxCollider enclosureCollider = enclosure.AddComponent<BoxCollider> ();
        enclosureCollider.size = new Vector3 (
            Mathf.Abs (firstCorner.x - secondCorner.x),
            3f,
            Mathf.Abs (secondCorner.z - firstCorner.z));
        enclosureCollider.center = new Vector3 (0f, 1.5f, 0f);
        

        //Clean up the temporary objects, and play the particle effects
        foreach (GameObject g in _tempCorners)
        {
            g.transform.parent = enclosure.transform;

            GameObject particleSystem = Instantiate (_finalizeBuildDustEffect, g.transform.position, g.transform.rotation);
            Destroy (particleSystem, 1f);
        }
        _tempCorners.Clear ();

        foreach (GameObject g in _tempWalls)
        {
            g.transform.parent = enclosure.transform;

            GameObject particleSystem = Instantiate (_finalizeBuildDustEffect, g.transform.position, g.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, 90f)));
            Destroy (particleSystem, 1f);
        }
        _tempWalls.Clear ();

        _pool.DestroyPool ();
        _pool = null;

        //Play sound
        AudioSource.PlayClipAtPoint (_finalizeBuildAudioClip, centreOfEnclosure); 
        //TODO: move sound closer to camera so it essentially is clamped to a higher min value? Or adjust volume bast on distance from camera

        _state = State.Idle;
    }

    public void CancelEnclosureBuild()
    {
        foreach (GameObject g in _tempCorners)
        {
            _pool.Destroy (EnclosureBuilderPool.ObjectType.Corner, g);
        }
        _tempCorners.Clear ();

        foreach (GameObject g in _tempWalls)
        {
            _pool.Destroy (EnclosureBuilderPool.ObjectType.Wall, g);
        }
        _tempWalls.Clear ();

        _pool.DestroyPool ();
        _pool = null;
        _state = State.Idle;
    }

}   //EnclosureBuilder