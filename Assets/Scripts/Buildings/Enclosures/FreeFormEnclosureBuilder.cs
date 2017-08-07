// Title        : FreeFormEnclosureBuilder.cs
// Purpose      : Allows for the creation of free-form enclosures
// Author       : Tony Haggar
// Date         : 29/07/2017

using UnityEngine;
using System.Collections.Generic;

public class FreeFormEnclosureBuilder : MonoBehaviour
{
    //This boolean controls whether the enclosure is built of straight lines, circles,  or uses bezier curves.
    private bool curvedMode;
    private bool circleMode;
    
    //I'm just using a single fence enclosure mesh for now, as I'm presuming that the actual type of fence will be selected through a GUI button press.
    public GameObject enclosurePieceType;

    private enum stateEnum { inactive, choosingStraightOrCurveStartPosition, choosingStraightOrCurveNextEnclosurePiecePosition, establishingCurve, choosingCircleStartPosition, establishingCircle };
    private stateEnum state = stateEnum.inactive;
    private stateEnum previousState = stateEnum.inactive;

    private GameObject nextEnclosurePiece;
    //private GameObject nextIntermediateEnclosurePiece;

    //If creating a circular enclosure, this is the centre point of it.
    private Vector3 circleCentrePoint;

    //The position at which the player first initially clicked to start the enclosure.
    private Vector3 enclosureStartPoint;

    //Set the 'nextPlacementLocationTarget' so that we lerp towards either the mouse cursor, or to the location of an existing piece of enclosure (enclosurePieceType).
    Vector3 nextPlacementLocationTarget = Vector3.zero;

    private Vector3 currentMousePosition = Vector3.zero;
    private float currentMouseDistance = 0;
    private Vector3 lastMousePosition = Vector3.zero;
    //private bool validMousePosition;

    //Used to control whether or not to create a new piece of enclosure when moving the mouse around.
    private bool createdEndEnclosurePiece = false;

    //Used to hold both the acutal pieces of (enclosurePieceType) and all of the intermediate pieces that are generated during dynamic construction.
    private List<GameObject> enclosurePieceList = new List<GameObject>();
    private List<GameObject> intermediateEnclosurePieceList = new List<GameObject>();
    //This list only holds the placed corner pieces and is used as a fast lookup when curving the enclosures.
    private List<GameObject> enclosureCornerPieceList = new List<GameObject>();

    private List<GameObject> circleEnclosurePieceList = new List<GameObject>();

    //Used to control the curve of the current curved enclosure section.
    private GameObject currentCurveControlHandle;

    //Used to store the current/last object hit when moving the mouse around, used to snap enclosures to themselves.
    private GameObject currentRayCastHitObject;
    private GameObject lastRayCastHitObject;
    private bool hoveringOverEnclosurePiece = false;

    //Materials, both standard and highlighted
    public Material highlightMaterial;
    public Material defaultMaterial;

    //Used to anglesnapping 
    private float angle;

    //These variables are used to control the placement of enclosureTypePieces within the game.  Either locking them to the grid, curving them, or constraining placement to a certain axis
    //private bool XLock = false;
    //private bool YLock = false;
    private bool gridLock = false;
    private bool angleLock = false;

    //Used in straight line enclosure mode to create basic curves.
    private bool straightEnclosureCurveLock = false;

    //Used to determine if the bezier control handles have been created.
    private bool createdCurveControlHandle = false;

    //Used to store the current vector3's for the curved enclosure points.
    private Vector3[] curvePoints;
    private float totalCurveLength;

    // Use this for initialization
    void Start()
    {


    }

    //When trying to get the mouse location so we can place enclosures, we only want to get the position from the ground (terrain).  This might change in the future, maybe you'd like to place
    //an enclosure on a roof, who knows.  For now, this function casts a ray and ignores the enclosure layer (layer 9).
    public void storeMouseLocation()
    {
        int layerMask = 1 << 9; //Set this to only collide with layer 9 (enclosure layer)
        layerMask = ~layerMask; //Invert the mask so that it collides with everything EXCEPT layer 9.  If the layer is changed for the (enclosurePieceType) then this will need updating.
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            currentMousePosition = hit.point;
            currentMouseDistance = hit.distance;
            //Debug.Log("Mouse pos: " + currentMousePosition);
        }

    }

    //Fire a ray at the mouse pointer to see if the player is hovering over an enclosure piece.
    public void enclosurePieceRayCheck()
    {
        //Probably a cleaner way of doing this, but just make sure that the default material is applied, it will get highlighted again if needed anyway.
        if (lastRayCastHitObject != null && (lastRayCastHitObject.GetComponent<Collider>().tag == "EnclosurePiecePlaced" || lastRayCastHitObject.GetComponent<Collider>().tag == "EnclosurePiecePlacedCorner"))
        {
            changeMaterial(lastRayCastHitObject, defaultMaterial);
        }

        int layerMask = 1 << 9; //Set this to only collide with layer 9 (enclosure layer)
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            currentRayCastHitObject = hit.collider.gameObject;

            Debug.Log("Hovering over: " + hit.collider.gameObject.name);

            if (currentRayCastHitObject.GetComponent<Collider>().tag == "EnclosurePiecePlaced" || currentRayCastHitObject.GetComponent<Collider>().tag == "EnclosurePiecePlacedCorner")
            {
                changeMaterial(hit.collider.gameObject, highlightMaterial);
                lastRayCastHitObject = currentRayCastHitObject;
                hoveringOverEnclosurePiece = true;
            }
            else
            {
                hoveringOverEnclosurePiece = false;
            }
        }
    }


    //Change material on the object, depending if it's currently being hit by RayCast or not.
    private void changeMaterial(GameObject enclosurePiece, Material material)
    {
        enclosurePiece.GetComponent<Renderer>().material = material;
    }

    private void copyIntermediateEnclosurePiecesToEnclosurePieceList()
    {
        Debug.Log("Saving intermediate pieces to main list");
        //On clicking to confirm the placement of the enclosure piece, add all of the intermediate pieces generated to the enclosurePieceList.
        foreach (GameObject o in intermediateEnclosurePieceList)
        {
            o.tag = "EnclosurePiecePlaced"; //'EnclosurePiecePlaced' tag is used for dynamically placed pieces, 'EnclosurePiecePlacedCorner' identifies player placed ones.
            enclosurePieceList.Add(o);
        }

        //Due to the speed at which update is called, this sometimes gets called prior to any intermediate pieces getting created, make sure there are some first.
        if (intermediateEnclosurePieceList.Count > 0)
        {
            intermediateEnclosurePieceList[intermediateEnclosurePieceList.Count - 1].tag = "EnclosurePiecePlacedCorner";
            enclosureCornerPieceList.Add(intermediateEnclosurePieceList[intermediateEnclosurePieceList.Count - 1]);
            intermediateEnclosurePieceList.Clear();
        }
    }


    //Allows the player to create a new circular enclosure.
    public void buildNewCircleEnclosure()
    {
        Debug.Log("Starting new circular enclosure");

        previousState = state;
        state = stateEnum.choosingStraightOrCurveStartPosition;
        nextEnclosurePiece = Instantiate(enclosurePieceType);
        nextEnclosurePiece.tag = "EnclosurePieceUnplaced";
        lastMousePosition = currentMousePosition;
        circleMode = true;
        state = stateEnum.choosingCircleStartPosition;
    }

    //Called when the button on the GUI is clicked.
    public void buildNewFreeFormEnclosure(bool curveMode)
    {
        if (curveMode)
        {
            Debug.Log("Building new curved free-form enclosure");
        }
        else
        {
            Debug.Log("Building new straight free-form enclosure");

        }

        previousState = state;
        state = stateEnum.choosingStraightOrCurveStartPosition;
        nextEnclosurePiece = Instantiate(enclosurePieceType);
        nextEnclosurePiece.tag = "EnclosurePieceUnplaced";
        lastMousePosition = currentMousePosition;

        //Either use straight lines, or bezier curves.
        this.curvedMode = curveMode;
    }

    //Taken from the existing EnclosureBuilder; works, but can cause some graphical issues, such as mesh's flashing when Unity is trying to clamp to the nearest tile.
    private Vector3 ClampToTileSize(Vector3 v)
    { // Clamp the vector3 to the pre defined tile size
        float tileSize = 2;
        v.x = v.x - (v.x % tileSize);
        v.y = v.y - (v.y % tileSize);
        v.z = v.z - (v.z % tileSize);
        return v;
    } // ClampToTileSize()


    //Determine how many pieces of enclosure are required to make a connection between the start and end point.
    private void fillEnclosureGap()
    {
        //Destroy all of the existing intermediate pieces as the layout has changed.
        if (intermediateEnclosurePieceList.Count > 0)
        {
            foreach (GameObject o in intermediateEnclosurePieceList)
            {
                Destroy(o);
            }
        }
        intermediateEnclosurePieceList.Clear();

        //If trying to snap to an existing piece of enclosure.
        if (hoveringOverEnclosurePiece)
        {
            nextPlacementLocationTarget = currentRayCastHitObject.transform.position;
        }
        //If just following the mouse cursor (no restrictions)
        else
        {
            //If the 'angleLock' key is held down (currently (KeyCode.LeftShit)) then constrain the enclosure to either the x or z axis.
            if (angleLock)
            {
                //Get the last piece of the enclosure that was placed and use that as the target to align to on its axis
                Vector3 alignmentTarget = enclosurePieceList[enclosurePieceList.Count - 1].transform.position;

                //Take both the enclosurePiece and the current mouse position and set their y values to zero.  The y essentially acts as the pivot around which the angle is calculated
                //So we need to ensure that it's pointing straight up.  This will account for any tilts in the mesh, e.g it's laying on uneven terrain.
                Vector3 adjustedMouseMousePosition = currentMousePosition;
                Vector3 adjustedTargetPosition = alignmentTarget;
                adjustedMouseMousePosition.y = 0;
                adjustedTargetPosition.y = 0;

                //Calculate the vector between the enclosure piece and set its length to 1 (normalise).
                Vector3 direction = (adjustedMouseMousePosition - adjustedTargetPosition).normalized;

                //I'll be honest, I don't understand Maths all that well, but from what I've been reading, in order to calculate the angle between two Vector3's in perspective space, 
                //we need to use Atan2.  This bit calculates the angle, and rounds it to an int (as we don't need the FP preceision).
                angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg + 180;
                int rounded = Mathf.RoundToInt(angle);

                //Debug.Log("Angle: " + angle);
                //Debug.Log("AngleRounded: " + rounded);

                //Distance between the placed enclosure piece and the cursor.  Used to control the lenth of the new enclosure run smoothly.
                float distance = Vector3.Distance(alignmentTarget, currentMousePosition);

                //Positive x
                Debug.DrawLine(alignmentTarget, new Vector3(alignmentTarget.x + 10, alignmentTarget.y, alignmentTarget.z), Color.red, 10, false);

                //Negative x
                Debug.DrawLine(alignmentTarget, new Vector3(alignmentTarget.x - 10, alignmentTarget.y, alignmentTarget.z), Color.green, 10, false);

                //Positive z
                Debug.DrawLine(alignmentTarget, new Vector3(alignmentTarget.x, alignmentTarget.y, alignmentTarget.z + 10), Color.blue, 10, false);

                //Negative z
                Debug.DrawLine(alignmentTarget, new Vector3(alignmentTarget.x, alignmentTarget.y, alignmentTarget.z - 10), Color.yellow, 10, false);


                //OK, here we see what angle range the cursor is currently in, and restrict movement to that axis only.  In order for this to work for any given mesh, the mesh MUST
                //be rotated correctly.  In that it must have it's X and Z axis's correctly oriented.  If a new enclosure mesh isn't behaving correctly during angle snap, ensure that its 
                //rotation is correctly applied.
                if (angle > 225 && angle < 315)
                {
                    //Debug.Log("Positive Z");
                    nextPlacementLocationTarget = new Vector3((alignmentTarget.x), alignmentTarget.y, (alignmentTarget.z + distance));
                }

                else if (angle > 45 && angle < 135)
                {
                    //Debug.Log("Negative Z");
                    nextPlacementLocationTarget = new Vector3((alignmentTarget.x), alignmentTarget.y, (alignmentTarget.z + -distance));
                }

                else if (angle > 135 && angle < 225)
                {
                    //Debug.Log("Positive X");
                    nextPlacementLocationTarget = new Vector3((alignmentTarget.x + distance), alignmentTarget.y, (alignmentTarget.z));
                }
                else
                {
                    //Debug.Log("Negative X");
                    nextPlacementLocationTarget = new Vector3((alignmentTarget.x + -distance), alignmentTarget.y, (alignmentTarget.z));
                }
            }
            else
            {
                nextPlacementLocationTarget = currentMousePosition;
            }
        }


        //If following the mouse cursor (snapping to grid)
        if (gridLock)
        {
            nextPlacementLocationTarget = ClampToTileSize(nextPlacementLocationTarget);

        }

        //The next position at which an enclosure piece will be placed.
        Vector3 nextPosition = Vector3.zero;

        //This is the magnitude of the curve if it was straightened out.
        Vector3 straightenCurve = Vector3.zero;

        //How many pieces of enclosure are actually required to fill the gap between the start and current end point (currentMousePosition)
        //This might need some tweaking, as the current value of 0.2f is used to determine the distance between each individual piece.  So if the piece is changed
        //this will also need changing.  I'll set it up so that it determines that actual size of the mesh in the future.
        Vector3 startOfCurve = enclosureCornerPieceList[enclosureCornerPieceList.Count - 1].transform.position;
        int fillCount = Mathf.RoundToInt(Vector3.Distance(startOfCurve, nextPlacementLocationTarget) / 0.2f);

        if (curvedMode && createdCurveControlHandle && totalCurveLength >0)
        {
            //Debug.Log("Totalcurve length: " + totalCurveLength);
            Vector3 size = enclosurePieceType.GetComponent<Renderer>().bounds.size;
            fillCount = Mathf.RoundToInt((totalCurveLength / size.x));
            
        }

        //Debug.Log("Fillcount: " + fillCount);

        if (fillCount > 0)
        {
            //Debug.Log("Fillcount: " + fillCount);
            //Vector3.Lerp takes a vector between 0 (start point) and 1 (end point) and Lerp's at each point on that vector. E.g, 0.5 is midway, 0.25 is a quarter, etc.
            float distance = 1.0f / fillCount;
            float lerpValue = 0;
            GameObject lastPlacedCorner = enclosurePieceList[enclosurePieceList.Count - 1];

            //When using Vector3.Slerp, if both the start and end points are zero, the arc will only travel in the X and Z axis.  
            float starty = lastPlacedCorner.transform.position.y;
            float endy = nextPlacementLocationTarget.y;

            //Debug.Log("Fillcount: " + fillCount);

            for (int i = 0; i < fillCount; i++)
            {
                lerpValue += distance;
                
                if (straightEnclosureCurveLock)
                {
                    nextPosition = Vector3.Slerp(new Vector3(lastPlacedCorner.transform.position.x, lastPlacedCorner.transform.position.y - starty, lastPlacedCorner.transform.position.z), new Vector3(nextPlacementLocationTarget.x, nextPlacementLocationTarget.y - endy, nextPlacementLocationTarget.z), lerpValue);
                    nextPosition.y = currentMousePosition.y;
                }
                else
                {
                    nextPosition = Vector3.Lerp(lastPlacedCorner.transform.position, nextPlacementLocationTarget, lerpValue);
                }

                //Only try to curve the enclosure once the control point (second enclosure piece) has been put down.
                if (curvedMode && createdCurveControlHandle && state == stateEnum.establishingCurve)
                {
                    if (hoveringOverEnclosurePiece)
                    {
                        curvePoints = new Vector3[3] { startOfCurve, currentCurveControlHandle.transform.position, currentRayCastHitObject.transform.position };
                        currentMousePosition = currentRayCastHitObject.transform.position;
                    }
                    else
                    {
                        curvePoints = new Vector3[3] { startOfCurve, currentCurveControlHandle.transform.position, currentMousePosition };
                    }
                    buildCurveTables();
                    float curveLerp = findPositionOnCurve(lerpValue);
                    nextPosition = GetBezierPoint(curveLerp, curvePoints[0], curvePoints[1], curvePoints[2]);
                    //Debug.DrawLine(startOfCurve, curvePoint, Color.red);
                }

                GameObject nextIntermediateEnclosurePiece;
                intermediateEnclosurePieceList.Add(nextIntermediateEnclosurePiece = Instantiate(enclosurePieceType, nextPosition, enclosurePieceType.transform.rotation));

            }

            //If the player is hovering over an existing piece, hide the current piece attached to the mouse cursor.
            if (hoveringOverEnclosurePiece)
            {
                GameObject lastPieceOfCurrentEnclosureSection = intermediateEnclosurePieceList[intermediateEnclosurePieceList.Count - 1];
                lastPieceOfCurrentEnclosureSection.SetActive(false);
            }
        }
    }

    //private float[] lerpTable;
    private float[] lengthTable;

    private Vector3 GetBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        //float x = (((1 - t) * (1 - t)) * start.x) + (2 * t * (1 - t) * control.x) + ((t * t) * end.x);
        //float y = (((1 - t) * (1 - t)) * start.y) + (2 * t * (1 - t) * control.y) + ((t * t) * end.y);
        //float z = (((1 - t) * (1 - t)) * start.z) + (2 * t * (1 - t) * control.z) + ((t * t) * end.z);
        //return new Vector3(x, y, z);

        //http://answers.unity3d.com/questions/990171/curve-between-lerps.html
        float rt = 1 - t;
        return rt * rt * start + 2 * rt * t * control + t * t * end;
    }

    private float findPositionOnCurve(float u)
    {
        //Debug.Log("Looking for " + u);
        if (u <= 0) return 0;
        if (u >= 1) return 1;

        int index = 0;
        int low = 0;
        int high = 100 - 1;
        float target = u * totalCurveLength;
        float found = float.NaN;

        // Binary search to find largest value <= target
        while (low < high)
        {
            index = (low + high) / 2;
            found = lengthTable[index];
            if (found < target)
                low = index + 1;
            else
                high = index;
        }

        // If the value we found is greater than the target value, retreat
        if (found > target)
            index--;

        if (index < 0) return 0;
        if (index >= 100 - 1) return 1;

        // Linear interpolation for index
        float min = lengthTable[index];
        float max = lengthTable[index + 1];
        Debug.Assert(min <= target && max >= target);
        float interp = (target - min) / (max - min);
        Debug.Assert(interp >= 0 && interp <= 1);
        int nSamples = 100;
        float ratio = 1f / nSamples;
        //Debug.Log("Returning: " + ((index + interp + 1) * ratio));
        return ((index + interp + 1) * ratio);
        

        /*
        float t; //Find t for the given u
        float targetArcLength = u * lengthTable[lengthTable.Length-1];

        //Debug.Log("u is: " + u);

        int index = Array.BinarySearch(lengthTable, targetArcLength);

        if (u >=1)
        {
            return 1;
        }

        if (index < 0)
        {
            index = ~index - 1;
            //No exact match found
            float lengthBefore = lengthTable[index];
            return (index + (targetArcLength - lengthBefore) / (lengthTable[index + 1] - lengthBefore)) / lengthTable.Length;
        }
        else
        {
            //Exact match found
            t = index / (float)lengthTable.Length - 1;
            //Debug.Log("Exact match, returning " + t);
            return t;
        }
        */
        
        /*
        var targetLength = u * lengthTable[lengthTable.Length -1];
        var low = 0;
        var high = lengthTable.Length;
        var index = 0;
        while (low < high)
        {
            index = low + (((high - low) / 2) | 0);
            if (lengthTable[index] < targetLength)
            {
                low = index + 1;

            }
            else
            {
                high = index;
            }
        }
        if (lengthTable[index] > targetLength)
        {
            index--;
        }

        var lengthBefore = lengthTable[index];
        if (lengthBefore == targetLength)
        {
            return index / lengthTable.Length;

        }
        else
        {
            return (index + (targetLength - lengthBefore) / (lengthTable[index + 1] - lengthBefore)) /lengthTable.Length;
        }
        */
    }
 
    private void buildCurveTables()
    {
        /*
        int curveSegments = 100;
        lengthTable = new float[curveSegments + 1];

        Vector3 previousPoint = GetBezierPoint(0, curvePoints[0], curvePoints[1], curvePoints[2]);

        float sum = 0;
        lengthTable[0] = 0;

        for (int i = 1; i < lengthTable.Length; i++)
        {
            Vector3 currentPoint = GetBezierPoint(i / (float) curvePoints.Length, curvePoints[0], curvePoints[1], curvePoints[2]);
            sum += Vector3.Distance(previousPoint, currentPoint);
            lengthTable[i] = sum;
            previousPoint = currentPoint;
        }

        totalCurveLength = sum;
        */

        //Debug.Log("Building curve table");
        Vector3 o = GetBezierPoint(0, curvePoints[0], curvePoints[1], curvePoints[2]);
        float ox = o.x;
        float oz = o.z;
        float clen = 0;
        int nSamples = 100;
        float ratio = 1f / nSamples;
        lengthTable = new float[nSamples];

        for (int i=0; i < nSamples; i++)
        {
            float t = (i + 1) * ratio;
            Vector3 p = GetBezierPoint(t, curvePoints[0], curvePoints[1], curvePoints[2]);
            //Debug.DrawLine(o, p, Color.red);
            float dx = ox - p.x;
            float dz = oz - p.z;
            clen += Mathf.Sqrt(dx * dx + dz * dz);
            lengthTable[i] = clen;
            ox = p.x;
            oz = p.z;
        }
        //Debug.Log("Calc curvelength :" + clen);
        totalCurveLength = clen;
        
    }

    //I've seperated this code from the fillEnclosureGap code so that it can be more easier placed into a seperate class and used for other things in the future.
    private void createCircle()
    {
        if (circleEnclosurePieceList.Count > 0)
        {
            for (int i = circleEnclosurePieceList.Count - 1; i >= 0; i--)
            {
                Destroy(circleEnclosurePieceList[i]);
                circleEnclosurePieceList.Remove(circleEnclosurePieceList[i]);
            }
        }

        Debug.Log("Creating circle");
        float radius = Vector3.Distance(circleCentrePoint, currentMousePosition);
        float circumference = 2 * Mathf.PI * radius;
        float size = enclosurePieceType.GetComponent<Renderer>().bounds.size.x;
        float fillCount = Mathf.RoundToInt(circumference / size);

        if (fillCount > 0)
        {
            float angle = 360 / fillCount;
            Vector3 pos;

            for (int i = 0; i < fillCount; i++)
            {
                circleEnclosurePieceList.Add(Instantiate(enclosurePieceType, findPositionOnCircle(circleCentrePoint, radius, i, angle), enclosurePieceType.transform.rotation));
            }
        }

    }

    Vector3 findPositionOnCircle(Vector3 c, float r, int i, float degrees)
    {
        return c + Quaternion.AngleAxis(degrees * i, Vector3.up) * (Vector3.forward * r);
    }

    //Just a simple method for flipping a boolean value, could be used elsewhere
    private bool flipBool(bool whatever)
    {

        return (whatever) ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("State is: " + state);

        //Right clicking will cancel the current placement operation, and revert back to the last placed piece, if there is one.
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Cancel called for current state: " + state);
            createdCurveControlHandle = false;
            if (state == stateEnum.inactive)
            {
                //Nothing to do, might think of something to do later...
            }

            //Just delete the 'nextEnclosurePiece' and revert back to the inactive state
            if (state == stateEnum.choosingStraightOrCurveStartPosition)
            {
                Destroy(nextEnclosurePiece);
                previousState = state;
                state = stateEnum.inactive;
            }

            if (state == stateEnum.establishingCurve)
            {
                Destroy(currentCurveControlHandle);
                createdCurveControlHandle = false;
                previousState = state;
                state = stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition;
               
               
            }

            //A piece of enclosure has been placed, delete all the intermediate pieces and revert back to the last one put down.
            if (state == stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition)
            {

                if (enclosurePieceList.Count == 1 && previousState == stateEnum.establishingCurve)
                {
                    Debug.Log("Sort this");
                }


                else if (enclosurePieceList.Count == 1)
                {
                    //Only a single piece has been placed, so just delete it and tidy up the lists.
                    foreach (GameObject o in intermediateEnclosurePieceList)
                    {
                        Destroy(o);
                    }
                    Destroy(nextEnclosurePiece);
                    Destroy(enclosurePieceList[enclosurePieceList.Count - 1]);
                    previousState = state;
                    state = stateEnum.inactive;
                    intermediateEnclosurePieceList.Clear();
                    enclosurePieceList.Clear();
                    Destroy(currentCurveControlHandle);
                }

                

                else //There are multiple placed pieces of enclosure, delete the last two placed and allow the player to redo the operation.
                {
                    //Change to inactive state so that we can delete objects without upsetting things.
                    previousState = state;
                    state = stateEnum.inactive;



                    //First off, destroy all of the intermediate enclosure pieces, and clear the list.
                    foreach (GameObject o in intermediateEnclosurePieceList)
                    {
                        Destroy(o);
                    }
                    intermediateEnclosurePieceList.Clear();

                    //Destroy the last player placed corner
                    GameObject lastPlacedCorner = enclosurePieceList[enclosurePieceList.Count - 1];
                    enclosurePieceList.Remove(lastPlacedCorner);
                    enclosureCornerPieceList.Remove(lastPlacedCorner);
                    Destroy(lastPlacedCorner);
                    Destroy(currentCurveControlHandle);

                    //Removing from a list whilst iterating through it does not a happy C# make, so you've got to iterate through it backwards.
                    //Go through removing all of the intermediately placed enclosure pieces, until you hit a player placed corner, that will be the new lerp position.
                    for (int i = enclosurePieceList.Count - 1; i >= 0; i--)
                    {
                        if (!enclosurePieceList[i].CompareTag("EnclosurePiecePlacedCorner"))
                        {
                            Destroy(enclosurePieceList[i]);
                            enclosurePieceList.Remove(enclosurePieceList[i]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    previousState = state;
                    state = stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition;
                }
            }

        } //End of rightmouse click cancel operations

        //Restrict enclosure placement along the axis the mouse is moving in (X or Y).
        if (Input.GetKey(KeyCode.LeftShift))
        {
            angleLock = true;
        }

        //Stop Restricting enclosure placement along the axis the mouse is moving in (X or Y).
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            angleLock = false;
        }

        //Switch snapping to the grid on and off
        if (Input.GetKeyDown(KeyCode.G))
        {
            gridLock = flipBool(gridLock);
            Debug.Log("GridLock is: " + gridLock);


        }

        //Constantly update the location of the cursor in world space.
        storeMouseLocation();

        //Once the centre of a circle has been chosen.
        if (state == stateEnum.establishingCircle)
        {
            if (currentMousePosition != lastMousePosition) {
                createCircle();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Finish circle!");
                Destroy(nextEnclosurePiece);
                state = stateEnum.inactive;

                foreach (GameObject circlePiece in circleEnclosurePieceList)
                {
                    GameObject replace = Instantiate(enclosurePieceType, circlePiece.transform.position, enclosurePieceType.transform.rotation);
                    replace.tag = "EnclosurePiecePlaced";
                    enclosurePieceList.Add(replace);
                    Destroy(circlePiece);
                }

            }
        }

        //If the player is creating a circular enclosure.
        if (state == stateEnum.choosingCircleStartPosition)
        {           
            nextEnclosurePiece.transform.position = currentMousePosition;
            lastMousePosition = currentMousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                if (hoveringOverEnclosurePiece)
                {
                    circleCentrePoint = currentRayCastHitObject.transform.position;
                }
                else
                { 
                circleCentrePoint = nextEnclosurePiece.transform.position;
                }
                previousState = state;
                state = stateEnum.establishingCircle;
            }
        }

        if (state != stateEnum.inactive)
        {
            //Hiting the return key completes the current enclosure placement and destroys any unplaced pieces.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                foreach (GameObject o in intermediateEnclosurePieceList)
                {
                    Destroy(o);
                }
                intermediateEnclosurePieceList.Clear();
                Destroy(nextEnclosurePiece);
                previousState = state;
                state = stateEnum.inactive;
            }

            //Check if the player is hovering over an existing enclosure piece, as they might want to snap to it.
            enclosurePieceRayCheck();

            //Handle enclosure placement if player is hovering over an existing piece of enclosure.
            if (hoveringOverEnclosurePiece)
            {
                try
                {
                    nextEnclosurePiece.transform.position = currentRayCastHitObject.transform.position;

                    //Set to false so that any additional raycasts don't cause the new enclosure piece to get stuck at the current position.
                    hoveringOverEnclosurePiece = false;

                    //Hide the next piece of the enclosure so that it allows the existing one to be highlighted.
                    nextEnclosurePiece.SetActive(false);

                    //Need to check what we're currently ray casting to, otherwise we'll be stuck here forever!
                    enclosurePieceRayCheck();
                }
                catch (MissingReferenceException destroyed)
                {
                    Debug.LogError("Player destoryed an enclosure piece while raycast hit it, not to worry.");
                }


            }

            else
            { //Again, the next piece might have been destroyed here (undo/completed), so just catch any errors thrown.
                try
                {
                    nextEnclosurePiece.SetActive(true);
                }
                catch (MissingReferenceException)
                {

                }
            }

        }





        //The player has clicked on 'Build new (straight/curved) freeform enclosure' on the GUI, but hasn't placed down the first piece of the enclosure.
        if (state == stateEnum.choosingStraightOrCurveStartPosition)
        {
            if (lastMousePosition != currentMousePosition)
            {
                if (gridLock)
                {
                    nextEnclosurePiece.transform.position = ClampToTileSize(currentMousePosition);
                }
                else
                {
                    nextEnclosurePiece.transform.position = currentMousePosition;
                }
                lastMousePosition = currentMousePosition;
            }

            //Player clicks to begin the enclosure.
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Beginning new enclosure");
                enclosureStartPoint = currentMousePosition;
                previousState = state;
                state = stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition;
                //When a piece is placed by the player, tag is with 'EnclosurePiecePlacedCorner' to ensure that it can be identified as a piece the player placed,
                //rather than a tag of 'EnclosurePiecePlaced' which was placed during the 'fillEnclosureGap' method.
                nextEnclosurePiece.tag = "EnclosurePiecePlacedCorner";
                enclosurePieceList.Add(nextEnclosurePiece);
                enclosureCornerPieceList.Add(nextEnclosurePiece);
                createdEndEnclosurePiece = false;
            }
        }


        //The first piece of the enclosure has been placed, generate the end enclosure piece, and fill in the gap between them.
        if (state == stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition || state == stateEnum.establishingCurve)
        {
            //Enable/disable curve mode for straight line enclosures.
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                straightEnclosureCurveLock = flipBool(straightEnclosureCurveLock);
                Debug.Log("CurveLock: " + straightEnclosureCurveLock);
            }

            //If both gridlock and anglelock are on, hide the next enclosure piece otherwise it will show up where the mouse cursor is, regardless of where the lerping is taking place.
            if (gridLock && angleLock)
            {
                nextEnclosurePiece.SetActive(false);
            }

            if (!createdEndEnclosurePiece)
            {
                nextEnclosurePiece = Instantiate(enclosurePieceType);
                nextEnclosurePiece.tag = "EnclosurePieceUnplaced";
                createdEndEnclosurePiece = true;
            }
            if (lastMousePosition != currentMousePosition)
            {
                if (gridLock)
                {
                    nextEnclosurePiece.transform.position = ClampToTileSize(currentMousePosition);
                }
                //Check if gridlock isn't active here incase the player turns it on, and then turns it off again, as the next location will need to be updated.
                else
                {
                    nextEnclosurePiece.transform.position = nextPlacementLocationTarget;
                }

                lastMousePosition = currentMousePosition;
                fillEnclosureGap();
            }


            //Once the player has placed down a curved section, destroy the old control point and allow the player to create a new one.
            if (Input.GetMouseButtonDown(0) && currentCurveControlHandle != null)
            {
                Debug.Log("Curved section placed, destroy old curve control handle");
                Destroy(currentCurveControlHandle);
                createdCurveControlHandle = false;
                copyIntermediateEnclosurePiecesToEnclosurePieceList();

            }

            //If the player clicks whilst establishing the curve of the current enclosure run.
            if (Input.GetMouseButtonDown(0) && currentCurveControlHandle && state == stateEnum.establishingCurve)
            {
                previousState = state;
                state = stateEnum.choosingStraightOrCurveNextEnclosurePiecePosition;
            }

                //Player clicks to complete the section of enclosure, or to place down the control point for a curved section of enclosure.
                //Due to 'Input.GetMouseButtonDown' returning true for several frames, check here that some of the intermediate pieces have been created, 
                //and this isn't just the first click from the previous 'choosingStartPosition' state.
                if (Input.GetMouseButtonDown(0) && intermediateEnclosurePieceList.Count > 0)
            {
                Debug.Log("Clicked to complete enclosure section, curved is: " + curvedMode);

                if (curvedMode && !createdCurveControlHandle)
                {
                    Debug.Log("Beginning curved enclosure section");
                    createdCurveControlHandle = true;
                    currentCurveControlHandle = Instantiate(enclosurePieceType);
                    currentCurveControlHandle.transform.position = currentMousePosition;
                    currentCurveControlHandle.tag = "EnclosurePieceCurveControlPoint";
                    previousState = state;
                    state = stateEnum.establishingCurve;

                }
                else
                {
                    copyIntermediateEnclosurePiecesToEnclosurePieceList();
                }

            }



        }
    }
}
