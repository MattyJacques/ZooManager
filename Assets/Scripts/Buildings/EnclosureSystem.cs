// Title        : EnclosureSystem.cs
// Purpose      : Draws a line of fences based on user input
// Author       : Eivind Andreassen
// Date         : 18/12/2016
using System.Collections.Generic;
using UnityEngine;

public class EnclosureSystem : MonoBehaviour
{
    public LayerMask _layerMask; //Layermask used to detect what surfaces enclosures can be built on, leave on default or everything until terrain has a specific layer
    public int _maximumFencesPerLine = 20;
    public Vector3 _objectPoolHideawayPosition = new Vector3 (-5000f, -5000f, -5000f);   //This is the position that fences get moved to when they're not in use

    private Fence[] _fences;
    [SerializeField]    //Debug setting, do not modify in editor, remove whenever
    private State _state;   //The EnclosureSystem's current state
    private Fence _selectedFence;   //The currently selected fence
    private Vector3 _currentFenceLineStart; //The position where the current line of fences that is being drawn starts
    private List<Transform> _fenceObjectPool = new List<Transform> ();  //TODO: Replace this with list of transforms, only keep fence as reference
    private int _amountOfFencesInCurrentLine = 0;

    private enum State
    {   //The different states the EnclosureSystem can be in
        Idle,               //Idle, is waiting for a call to SelectFence()
        Waiting_For_Input,  //Recieved call to SelectFence(), is waiting for the player to define where the line starts (left click)
        Drawing_Line_Of_Fences,  //Is drawing the line of fences from the clicked position to the player's mouse cursor
    }

    public class Fence
    {   //The struct for the fences

        public Vector3 beginningPostOffset; //TODO: just have these be floats?
        public Vector3 endPostOffset;
        public GameObject gameObject;
        public float length;
        public Quaternion originalRotation;
        public float margin;    //The space between this fence and others in a line
        
        /// <param name="beginningPostOffset">The beginning post's offset from the fences pivot point</param>
        /// <param name="endPostOffset">The ending post's offset from the fences pivot point</param>
        /// <param name="gameObject">The fence GameObject itself</param>
        /// <param name="margin">The space between this fence and others in a line</param>
        public Fence(Vector3 beginningPostOffset, Vector3 endPostOffset, GameObject gameObject, float margin)
        {
            this.beginningPostOffset = beginningPostOffset;
            this.endPostOffset = endPostOffset;
            this.length = Vector3.Distance (beginningPostOffset, endPostOffset) + margin;
            this.gameObject = gameObject;
            this.margin = margin;
            this.originalRotation = gameObject.transform.rotation;
        }

        /// <param name="beginningPostOffset">The beginning post's offset from the fences pivot point</param>
        /// <param name="endPostOffset">The ending post's offset from the fences pivot point</param>
        /// <param name="gameObject">The fence GameObject itself</param>
        /// <param name="margin">The space between this fence and others in a line</param>
        /// <param name="modifiedRotation">This is the offset used when rotating the model. This parameter should be omitted unless you specifically do not want to use the object's original rotation.</param>
        public Fence(Vector3 beginningPostOffset, Vector3 endPostOffset, GameObject gameObject, float margin, Quaternion modifiedRotation)
        {
            this.beginningPostOffset = beginningPostOffset;
            this.endPostOffset = endPostOffset;
            this.length = Vector3.Distance (beginningPostOffset, endPostOffset) + margin;
            this.gameObject = gameObject;
            this.margin = margin;
            this.originalRotation = modifiedRotation;
        }
        
        
        public static Fence Copy(Fence fence)
        {
            Fence newFence = new Fence (fence.beginningPostOffset, fence.endPostOffset, Instantiate (fence.gameObject), fence.margin, fence.originalRotation);
            newFence.gameObject.name = fence.gameObject.name;   //Omitt the "(clone)" string that gets added to cloned objects else we can't compare them based on name
            return newFence;
        }
    }   //Fence

    private void Start()
    {   //Initializes
        _fences = LoadFences();
    }   //Start()

    private void Update()
    {   //Gets player input based on what state the EnclosureSystem is in

        switch (_state) {
            case State.Waiting_For_Input:
                if (Input.GetMouseButtonDown (0))   //left click
                {
                    BeginDrawingLineOfFences ();
                }
                break;

            case State.Drawing_Line_Of_Fences:
                DrawLineOfFences ();

                if (Input.GetMouseButtonDown (0))   //Left click
                {
                    FinalizeDrawingLineOfFences ();
                }

                if (Input.GetMouseButtonDown (1))   //Right click
                {
                    CancelDrawingLineOfFences ();
                }
                break;
        }
    }   //Update()

    private void OnGUI()
    {   //Draws the buttons that calls SelectFence

        float width = Screen.width / _fences.Length;
        GUI.BeginGroup(new Rect(0, Screen.height - 50, Screen.width, 50));
        for (int i = 0; i < _fences.Length; i++)
        {
            if (GUI.Button(new Rect(width * i, 5, width, 40), _fences[i].gameObject.name))
            {
                SelectFenceToUse (_fences[i]);
            }
        }
        GUI.EndGroup();
    }

    private Fence[] LoadFences()
    {   //Returns an array with all of the fence objects to be used by the enclosure system

        //TODO: Load this from a JSON or XML file instead
        return new Fence[5] 
        {
            new Fence(new Vector3(0f, 0f, -1), new Vector3(0f, 0f, 1), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(Broken)") as GameObject, 0.2f),
            new Fence(new Vector3(0f, 0f, -1), new Vector3(0f, 0f, 1), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(End)") as GameObject, 0.2f),
            new Fence(new Vector3(0f, 0f, -1), new Vector3(0f, 0f, 1), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(Straight)") as GameObject, 0.2f),
            new Fence(new Vector3(0f, 0f, -1), new Vector3(0f, 0f, 1), Resources.Load("Buildings/Prefabs/Fences/LowWoodGate(Bar)") as GameObject, 0.2f),
            new Fence(new Vector3(0f, 0f, -1), new Vector3(0f, 0f, 1), Resources.Load("Buildings/Prefabs/Fences/LowWoodGate(Board)") as GameObject, 0.2f)
        };
    }   //LoadFences()

    private void SelectFenceToUse(Fence fence)
    {   //sets _selectedFence to fence and changes _state to Waiting_For_Input

        _selectedFence = fence;
        _state = State.Waiting_For_Input;

    }   //SelectFence()

    private void BeginDrawingLineOfFences()
    {   //Gets the cursor current position and begins to draw a new line of fences originating from that position

        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit = new RaycastHit ();

        if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
        {
            _currentFenceLineStart = rayHit.point;
            _state = State.Drawing_Line_Of_Fences;
        }
    }   //BeginDrawingLineOfFences()

    private void DrawLineOfFences()
    {   //Draws the line of fences so that the player can visualize where it's going to end up

        if (_currentFenceLineStart == null)
        {
            Debug.LogError ("Line of fences has no start, it needs a starting point!");
            return;
        }

        //Get where the cursor is. Don't do anything unless it's hovering over something.
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit = new RaycastHit ();
        if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 currentFenceLineEnd = rayHit.point;
            float lineDistance = Vector3.Distance (currentFenceLineEnd, _currentFenceLineStart);
            _amountOfFencesInCurrentLine = (int) (lineDistance / _selectedFence.length);
            _amountOfFencesInCurrentLine = Mathf.Clamp (_amountOfFencesInCurrentLine, 0, _maximumFencesPerLine);

            //Draw a line of fences going from _currentFenceLineStart to where the ray from the player's mouse hit
            for (int i = 0; i < _amountOfFencesInCurrentLine; i++)
            {
                Transform fenceTransform = GetFenceTransformFromPool (i);
                Vector3 dir = (currentFenceLineEnd - _currentFenceLineStart).normalized;
                Vector3 pos = _currentFenceLineStart + dir * (_selectedFence.length * i);
                pos += dir * (_selectedFence.length / 2);
                fenceTransform.position = pos;
                fenceTransform.rotation = Quaternion.LookRotation (dir) * _selectedFence.originalRotation;
            }

            //Hide any fences that aren't being used
            if (_amountOfFencesInCurrentLine < _fenceObjectPool.Count)
            {
                for (int i = _amountOfFencesInCurrentLine; i < _fenceObjectPool.Count; i++)
                {
                    _fenceObjectPool[i].position = _objectPoolHideawayPosition;
                }
            }
        }
    }   //DrawLineOfFences()

    private void CancelDrawingLineOfFences()
    {   //Sets state to Idle, and hides all fences in the pool

        _state = State.Idle;
        DestroyAllObjectInObjectPool ();

    }   //CancelDrawingLineOfFences()

    private void FinalizeDrawingLineOfFences()
    {   //Finalizes the line of fences, and places the GameObjects into the scene

        //TODO: particle effect here?
        _state = State.Idle;

        //Remove the fences we're currently using from the object pool, and destroy the remainders
        _fenceObjectPool.RemoveRange (0, _amountOfFencesInCurrentLine);
        DestroyAllObjectInObjectPool ();

    }   //FinalizeDrawingLineOfFences()
    
    /// <param name="numberFromTheCenter">What number the fence is in a line of fences from the beginning of the line. The first fence is 0.</param>
    private Transform GetFenceTransformFromPool(int numberFromTheCenter)
    {   //Returns the transform of a fence from the object pool based on the distance //TODO: fix comment

        if (_fenceObjectPool.Count <= numberFromTheCenter)
        {
            Fence newFence = Fence.Copy (_selectedFence);
            _fenceObjectPool.Add (newFence.gameObject.transform);
            return newFence.gameObject.transform;
        }

        return _fenceObjectPool[numberFromTheCenter];
    }   //GetFenceFromPool()

    private void DestroyAllObjectInObjectPool()
    {   //Destroys all the objects in the object pool and clears the list

        foreach (Transform t in _fenceObjectPool)
        {
            Destroy (t.gameObject);
        }
        _fenceObjectPool.Clear ();

    }   //DestroyObjectPool()
}