// Title        : FreeFormEnclosureBuilder.cs
// Purpose      : Allows for the creation of free-form enclosures
// Author       : Tony Haggar
// Date         : 29/07/2017

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FreeFormPathBuilder : MonoBehaviour
{
    //The current state of the pathbuilding system.
    private stateEnum state = stateEnum.inactive;

    private enum stateEnum {inactive, choosingPathStartPosition, choosingPathEndPosition, choosingCurveControlPoint, establishingCurve};

    //The prefab that contains the procgen mesh creation code.
    public Transform procGenMeshPrefab;

    //The instantiated procgen object that contains the path mesh.
    private Transform procGenPath;

    //At present the path supports two submesh materials, but this can be altered to give the path different materials/colours.
    public Material pathMaterial1;
    
    //Used to control the bezier curves for smooth corners.
    private float totalCurveLength;
    private Vector3[] curvePoints;

    LineRenderer line;
    GameObject lineDrawer;

    Vector3 currentMousePosition;

    //The line only has two vertex positions; the start and end.
    int lineVertexCount = 2;

    //How wide is the path?
    float pathWidth = 1f;

    List<Path> pathList = new List<Path>();
    Path currentPath = null;

    //This boolean controls whether or not the player is creating a path.
    private bool buildingPath = false;

    //If the player is creating a curved path.
    private bool curvedMode = false;

    //The Vector3 used to curve the path around.
    private Vector3 currentCurveControlHandle;

    //Used a temporary on screen indicator;
    GameObject temp = null;

    //Each new piece of path is a 'Path' object.
    public class Path
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public List<Vector3[]> sectionPoints;
        public bool CapStart { get; set; }
        public bool CapEnd { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 CornerIntersection { get; set; }
        public List<Vector3> cornerCurvePositions;
        public int SubMesh { get; set; }
        public Vector3 CurveStartTangent { get; set; }
        public Vector3 CurveEndTangent { get; set; }
    }

    public void BuildNewFreeFormPath(bool curvedMode)
    {
        Debug.Log("Building new free form path");
        lineDrawer = new GameObject("Line");
        line = lineDrawer.AddComponent<LineRenderer>();
        line.startWidth = 3f;
        line.endWidth = 3f;
        line.numCornerVertices = 5;
        line.numCapVertices = 5;
        line.positionCount = lineVertexCount;
        line.enabled = false;
        line.material = pathMaterial1;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        buildingPath = !buildingPath;
        this.curvedMode = curvedMode;
        state = stateEnum.choosingPathStartPosition;

    }

    public void BuildNewCirclePath()
    {
        Debug.Log("Building new circular path");
    }

    private void CreateNewPath()
    {
        line.positionCount = 2;
        currentPath = new Path();
        currentPath.SubMesh = 0;
        currentPath.sectionPoints = new List<Vector3[]>();
        currentPath.Start = currentMousePosition;
        //Debug.Log("Path starting at: " + currentPath.Start);
        currentPath.CapStart = true;
        if (pathList.Count > 0)
        {
            //This isn't the first path, so start it from the end of the last path placed.
            line.SetPosition(0, pathList[pathList.Count-1].End);
        }
        else
        {
            line.SetPosition(0, currentMousePosition);
        }
        
    }

    public int CalculateCornerIntersectionPoint(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, out Vector3 Pout)
    {
        float mua, mub;
        float denom, numera, numerb;
        const float eps = 0.000000000001f;
        Pout = Vector3.zero;

        denom = (P4.z - P3.z) * (P2.x - P1.x) - (P4.x - P3.x) * (P2.z - P1.z);
        numera = (P4.x - P3.x) * (P1.z - P3.z) - (P4.z - P3.z) * (P1.x - P3.x);
        numerb = (P2.x - P1.x) * (P1.z - P3.z) - (P2.z - P1.z) * (P1.x - P3.x);

        if ((-eps < numera && numera < eps) &&
                 (-eps < numerb && numerb < eps) &&
                 (-eps < denom && denom < eps))
        {
            Pout.x = (P1.x + P2.x) * 0.5f;
            Pout.z = (P1.z + P2.z) * 0.5f;
            return 2; //meaning the lines coincide
        }

        if (-eps < denom && denom < eps)
        {
            Pout.x = 0;
            Pout.z = 0;
            return 0; //meaning lines are parallel
        }

        mua = numera / denom;
        mub = numerb / denom;
        Pout.x = P1.x + mua * (P2.x - P1.x);
        Pout.z = P1.z + mua * (P2.z - P1.z);
        bool out1 = mua < 0 || mua > 1;
        bool out2 = mub < 0 || mub > 1;

        if (out1 & out2)
        {
            return 5; //the intersection lies outside both segments
        }
        else if (out1)
        {
            return 3; //the intersection lies outside segment 1
        }
        else if (out2)
        {
            return 4; //the intersection lies outside segment 2
        }
        else
        {
            return 1; //the intersection lies inside both segments
        }
    }

    //Once two paths have been placed, this method builds the intersecting corner.  Either square or rounded.
    private void BuildPathIntersectionCorner()
    {
        //Debug.Log("Two paths placed, check intersection!");
        Path path1 = pathList[pathList.Count - 2];
        Path path2 = pathList[pathList.Count - 1];
        path1.cornerCurvePositions = new List<Vector3>();

        Vector3 interP = Vector3.zero;

        float angle = 0;

        angle = CalculateAngle(path1.Direction, path2.Direction);

        if (curvedMode)
        {
            angle = CalculateAngle(path1.CurveEndTangent, path2.CurveStartTangent);
        }

        Debug.Log("Angle is: " + angle);

        Vector3 t0;
        Vector3 p0;
        Vector3 p1;
        Vector3 t2;
        Vector3 p2;
        
        if (angle > 0) //Right turn.
        {
            if (curvedMode)
            {
                t0 = path1.sectionPoints[path1.sectionPoints.Count -2][0];
                p0 = path1.sectionPoints[path1.sectionPoints.Count - 2][1];
                p1 = path1.sectionPoints[path1.sectionPoints.Count - 1][1];
                t2 = path2.sectionPoints[1][0];
                p2 = path2.sectionPoints[1][1];
            }
            else
            {
                t0 = path1.sectionPoints[0][0];
                p0 = path1.sectionPoints[0][1];
                p1 = path1.sectionPoints[path1.sectionPoints.Count - 1][1];
                t2 = path2.sectionPoints[path2.sectionPoints.Count - 1][0];
                p2 = path2.sectionPoints[path2.sectionPoints.Count - 1][1];
            }
        }
        else //Left turn.
        {
            if (curvedMode)
            {
                t0 = path1.sectionPoints[path1.sectionPoints.Count-2][2];
                p0 = path1.sectionPoints[path1.sectionPoints.Count-2][1];
                p1 = path1.sectionPoints[path1.sectionPoints.Count - 1][1];
                t2 = path2.sectionPoints[1][2];
                p2 = path2.sectionPoints[1][1];
            }
            else
            {
                t0 = path1.sectionPoints[0][2];
                p0 = path1.sectionPoints[0][1];
                p1 = path1.sectionPoints[path1.sectionPoints.Count - 1][1];
                t2 = path2.sectionPoints[path2.sectionPoints.Count - 1][2];
                p2 = path2.sectionPoints[path2.sectionPoints.Count - 1][1];
            }
        }

        int intersect = CalculateCornerIntersectionPoint(t0 + p0, t0 + p1, t2 + p2, t2 + p1, out interP);
        Debug.Log("Intersect is: " + intersect);
        Vector3 vp = interP - p1;

        //Set the intersection y axis to be the same as the top of the path.
        vp.y = p0.y;

        path1.CornerIntersection = vp;
        path2.CornerIntersection = vp;

        Debug.DrawLine(path1.End, vp, Color.cyan, 60f);

        //-----------------------------------------------------------------------------------------
        //Create the corners for the path intersection
        int cornerFillCount = 8; //The higher the number the smoother the corner.
        float distance = 0;

        if (angle > 0) //Path is turning right.
        {
            Debug.Log("Right turn");
            //procGenPath.GetComponent<ProcGenMesh>().BuildIntersectionCorner(path1.sectionPoints[path1.sectionPoints.Count - 1], path1.CornerIntersection, true, 1);
            //procGenPath.GetComponent<ProcGenMesh>().BuildIntersectionCorner(path2.sectionPoints[0], path2.CornerIntersection, true, 2);
            curvePoints = new Vector3[3] { path1.sectionPoints[path1.sectionPoints.Count - 1][0], vp, path2.sectionPoints[0][0] };
            path1.cornerCurvePositions.Add(path1.sectionPoints[path1.sectionPoints.Count - 1][0]);



        }
        else //Path is turning left.
        {
            Debug.Log("Left turn");
            //procGenPath.GetComponent<ProcGenMesh>().BuildIntersectionCorner(path1.sectionPoints[path1.sectionPoints.Count - 1], path1.CornerIntersection, false, 1);
            //procGenPath.GetComponent<ProcGenMesh>().BuildIntersectionCorner(path2.sectionPoints[0], path2.CornerIntersection, false, 2);
            curvePoints = new Vector3[3] { path1.sectionPoints[path1.sectionPoints.Count - 1][2], vp, path2.sectionPoints[0][2] };
            path1.cornerCurvePositions.Add(path1.sectionPoints[path1.sectionPoints.Count - 1][2]);
        }

        BuildCurveTables();
        distance = 1.0f / cornerFillCount;
        float lerpValue = 0;

        for (int i = 0; i < cornerFillCount; i++)
        {
            lerpValue += distance;
            float curveLerp = FindPositionOnCurve(lerpValue);
            Vector3 nextPosition = GetBezierPoint(curveLerp, curvePoints[0], curvePoints[1], curvePoints[2]);
            //Debug.Log("Next position for corner curve: " + nextPosition);
            //GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //test.transform.localScale = Vector3.one / 5.0f;
            //test.transform.position = nextPosition;
            path1.cornerCurvePositions.Add(nextPosition);
        }

        //Build the curved intersection corner.
        if (angle > 0)
        {
            procGenPath.GetComponent<ProcGenMesh>().BuildCornerCurve(path1.cornerCurvePositions, path1.sectionPoints[path1.sectionPoints.Count - 1][1], true, 1);
        }
        else
        {
            procGenPath.GetComponent<ProcGenMesh>().BuildCornerCurve(path1.cornerCurvePositions, path1.sectionPoints[path1.sectionPoints.Count - 1][1], false, 1);
        }

        //End of path corner intersection.
        //-----------------------------------------------------------------------------------------
    }

    private void CancelPathOperation()
    {
        Debug.Log("Cancel called for state: " + state);
        switch (state)
        {
            case stateEnum.inactive: //Nothing to cancel, might want to add something
                break;
            case stateEnum.choosingPathStartPosition:
                Destroy(temp);
                line.enabled = false;
                state = stateEnum.inactive;
                break;

        }
    }


    private void Update()
    { 
        StoreMouseLocation();

        //Debug.Log("State is: " + state);

        if (buildingPath)
        {
            //--------------------------------------------------------------
            //------------------HANDLE MOUSE INPUT--------------------------
            //--------------------------------------------------------------
            if (Input.GetMouseButtonDown(0)) //Left click.
            {
                switch (state)
                {
                    case stateEnum.inactive:
                        break;
                    case stateEnum.choosingPathStartPosition:
                        //Debug.Log("Start of path!");
                        line.enabled = true;
                        CreateNewPath();
                        state = (curvedMode) ? stateEnum.choosingCurveControlPoint : stateEnum.choosingPathEndPosition;
                        break;
                    case stateEnum.choosingPathEndPosition:
                        //Debug.Log("End of path!");
                        currentPath.End = currentMousePosition;
                        currentPath.Direction = currentPath.End - currentPath.Start;
                        pathList.Add(currentPath);
                        ConstructPathPoints();
                        CreateNewPath();
                        if (pathList.Count >=2)
                        {
                            BuildPathIntersectionCorner();
                        }
                        break;
                    case stateEnum.choosingCurveControlPoint:
                        //Debug.Log("Curve control point goes here!");
                        currentCurveControlHandle = currentMousePosition;
                        state = stateEnum.establishingCurve;
                        break;
                    case stateEnum.establishingCurve:
                        currentPath.End = currentMousePosition;
                        pathList.Add(currentPath);
                        ConstructPathPoints();
                        currentPath.Direction = currentPath.End - currentPath.Start;
                        CreateNewPath();
                        if (pathList.Count >= 2)
                        {
                            BuildPathIntersectionCorner();
                        }
                        state = stateEnum.choosingCurveControlPoint;
                        break;
                }
                
            }
            //If the player right clicks whilst building a path.
            if (Input.GetMouseButtonDown(1))
            {
                CancelPathOperation();
            }
            //---------------------------------------------------------------------
            //------------------END OF HANDLE MOUSE INPUT--------------------------
            //---------------------------------------------------------------------



            //-------------------------------------------------------------------------
            //----------------------------STATE CONTROL HANDLING-----------------------
            //-------------------------------------------------------------------------

            switch (state)
            {
                case stateEnum.inactive:
                    break;
                case stateEnum.choosingPathStartPosition:
                    if (temp == null)
                    {
                        temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        temp.GetComponent<Collider>().enabled = false;
                        temp.name = "PathStartIndicator";
                    }
                    temp.transform.position = currentMousePosition;
                    break;
                case stateEnum.choosingCurveControlPoint:
                case stateEnum.choosingPathEndPosition:
                    line.SetPosition(1, currentMousePosition);
                    temp.transform.position = currentMousePosition;
                    break;
                case stateEnum.establishingCurve:
                    curvePoints = new Vector3[3] { currentPath.Start, currentCurveControlHandle, currentMousePosition};
                    BuildCurveTables();
                    CurveLine();
                    break;
            }

            //--------------------------------------------------------------------------------
            //----------------------------END OF STATE CONTROL HANDLING-----------------------
            //--------------------------------------------------------------------------------



        }

    }

    //Used to curve the line so the player can see how the curve will look, prior to constructing the geometry.
    private void CurveLine()
    {
        //Divide the line up so that it curves somewhat evenly.
        int sectionCount = Mathf.RoundToInt(Vector3.Distance(currentPath.Start, currentMousePosition) / pathWidth);
        float distance = 1.0f / sectionCount;
        float lerpValue = 0;
        Vector3 nextPosition;
        line.positionCount = sectionCount;
        for (int i = 0; i < line.positionCount; i++)
        {
            lerpValue += (i == 0) ? 0 : distance;
            float curveLerp = FindPositionOnCurve(lerpValue);
            nextPosition = GetBezierPoint(curveLerp, curvePoints[0], curvePoints[1], curvePoints[2]);
            line.SetPosition(i, nextPosition);
        }

    }

    //https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Quadratic_B.C3.A9zier_curves
    private Vector3 GetTangent(float t)
    {
        Vector3 tangent = (2f * (1.0f - t)) * (curvePoints[1] - curvePoints[0]) + (2 * t * (curvePoints[2] - curvePoints[1]));

        return tangent;
    }

    private Vector3 GetNormal3D(float t)
    {
        Vector3 tangent = GetTangent(t);
        Vector3 binormal = Vector3.Cross(Vector3.up, tangent).normalized;
        return Vector3.Cross(tangent, binormal);
    }

    private Quaternion GetOrientation(float t)
    {
        Vector3 tangent = GetTangent(t);
        Vector3 normal = GetNormal3D(t);
        //Debug.Log("Look rotation: " + Quaternion.LookRotation(tangent, normal));
        return Quaternion.LookRotation(tangent, normal);
    }

    private void ConstructPathPoints()
    {
        //Debug.Log("Building points for path");

        Vector3 cross = Vector3.Cross(currentPath.Direction, Vector3.up);
        cross.Normalize();

        //Place sections evenly along the path
        int sectionCount = Mathf.RoundToInt(Vector3.Distance(currentPath.Start, currentPath.End) / pathWidth);
        if (curvedMode)
        {
            sectionCount = Mathf.RoundToInt(totalCurveLength / pathWidth);
        }
        float distance = 1.0f / sectionCount;
        float lerpValue = 0;
        Vector3 nextPosition;
        for (int i = 0; i < sectionCount + 1; i++)
        {
            lerpValue += (i == 0) ? 0 : distance;
            nextPosition = Vector3.Lerp(currentPath.Start, currentPath.End, lerpValue);

            if (curvedMode)
            {
                float curveLerp = FindPositionOnCurve(lerpValue);
                nextPosition = GetBezierPoint(curveLerp, curvePoints[0], curvePoints[1], curvePoints[2]);

                Vector3 tangent = GetTangent(curveLerp).normalized;
                if (curvedMode)
                {
                    if (i==0)
                    {
                        currentPath.CurveStartTangent = tangent;
                    }
                    if (i == (sectionCount +1) -1)
                    {
                        currentPath.CurveEndTangent = tangent;
                    }
                }
                Vector3 normal = GetNormal3D(curveLerp).normalized;
                cross = Vector3.Cross(tangent, Vector3.up);
                cross.Normalize();

                Debug.DrawLine(nextPosition, nextPosition + tangent, Color.red, 60f);
                Debug.DrawLine(nextPosition, nextPosition + normal, Color.green, 60f);
            }
            
            Vector3 leftVert = pathWidth * cross + nextPosition;
            Vector3 rightVert = -pathWidth * cross + nextPosition;
            Vector3[] vertexPoints = new Vector3[3];
            vertexPoints[0] = leftVert;
            vertexPoints[1] = nextPosition;
            vertexPoints[2] = rightVert;
            currentPath.sectionPoints.Add(vertexPoints);
            //Debug.Log("Test: " + currentPath.sectionPoints[0][0]);

        }

        //At this point the path has been evenly divided up, so now it's time to construct the mesh for the path.

        procGenPath = Instantiate(procGenMeshPrefab);
        procGenPath.GetComponent<ProcGenMesh>().SetHeightOffset((pathList.Count % 2 == 0) ? 0.0001f : -0.0001f);
        for (int i = 0; i < currentPath.sectionPoints.Count; i++)
        {
            foreach (Vector3 v in currentPath.sectionPoints[i])
            {
                //GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //test.transform.localScale = Vector3.one / 5.0f;
                //test.transform.position = v;
                //test.GetComponent<MeshRenderer>().material = pathMaterial1;

            }
        }

        //Create the mesh for the path from the generated vectors.
        procGenPath.GetComponent<ProcGenMesh>().BuildProceduralPath(currentPath.sectionPoints, currentPath.SubMesh);
    }

    //private float[] lerpTable;
    private float[] lengthTable;

    private Vector3 GetBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        float x = (((1 - t) * (1 - t)) * start.x) + (2 * t * (1 - t) * control.x) + ((t * t) * end.x);
        float y = (((1 - t) * (1 - t)) * start.y) + (2 * t * (1 - t) * control.y) + ((t * t) * end.y);
        float z = (((1 - t) * (1 - t)) * start.z) + (2 * t * (1 - t) * control.z) + ((t * t) * end.z);
        return new Vector3(x, y, z);

        //http://answers.unity3d.com/questions/990171/curve-between-lerps.html
        //float rt = 1 - t;
        //return rt * rt * start + 2 * rt * t * control + t * t * end;
    }

    private float FindPositionOnCurve(float u)
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
    }

    private void BuildCurveTables()
    {
        //Debug.Log("Building curve table");
        Vector3 o = GetBezierPoint(0, curvePoints[0], curvePoints[1], curvePoints[2]);
        float ox = o.x;
        float oy = o.y;
        float oz = o.z;
        float clen = 0;
        int nSamples = 100;
        float ratio = 1f / nSamples;
        lengthTable = new float[nSamples];

        for (int i = 0; i < nSamples; i++)
        {
            float t = (i + 1) * ratio;
            Vector3 p = GetBezierPoint(t, curvePoints[0], curvePoints[1], curvePoints[2]);
            //Debug.DrawLine(o, p, Color.red);
            float dx = ox - p.x;
            float dy = oy - p.y;
            float dz = oz - p.z;
            clen += Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
            lengthTable[i] = clen;
            ox = p.x;
            oy = p.y;
            oz = p.z;
        }
        //Debug.Log("Calc curvelength :" + clen);
        totalCurveLength = clen;



    }

    //Determine if the player has made a new path to the left, or the right of the last path.
    public float CalculateAngle(Vector3 fromDir, Vector3 toDir)
    {
        float angle = Quaternion.FromToRotation(fromDir, toDir).eulerAngles.y;
        if (angle > 180) { return angle - 360f; }
        return angle;
    }

    public void StoreMouseLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            currentMousePosition = hit.point;
            //currentMouseDistance = hit.distance;
            //Debug.Log("Mouse pos: " + currentMousePosition);
        }
    }
}
