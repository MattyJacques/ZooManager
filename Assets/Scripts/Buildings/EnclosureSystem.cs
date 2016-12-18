// Title        : EnclosureSystem.cs
// Purpose      : 
// Author       : Eivind Andreassen
// Date         : 18/12/2016
using System.Collections.Generic;
using UnityEngine;

public class EnclosureSystem : MonoBehaviour
{
    public LayerMask _layerMask; //Layermask used to detect what surfaces enclosures can be built on, leave on default or everything until terrain has a specific layer
    private Fence[] _fences;
    [SerializeField]    //Debug setting, do not modify in editor, remove whenever
    private State _state;   //The EnclosureSystem's current state
    private Fence _selectedFence;   //The currently selected fence
    private Vector3 _currentFenceLineStart; //The position where the current line of fences that is being drawn starts
    private List<Fence> _fencesBeingDrawn = new List<Fence> (); //TODO: Use object pool to get fences, rotate them in place

    private enum State
    {   //The different states the EnclosureSystem can be in
        Idle,               //Idle, is waiting for a call to SelectFence()
        Waiting_For_Input,  //Waits for the player to left-click
        Drawing_Line_Of_Fences,  //Is drawing the enclosure from the clicked position to the player's mouse cursor
    }

    public struct Fence
    {   //The struct for the fences
        public Vector3 beginningPostOffset;
        public Vector3 endPostOffset;
        public GameObject gameObject;
        
        /// <param name="beginningPostOffset">The beginning post's offset from the fences pivot point</param>
        /// <param name="endPostOffset">The ending post's offset from the fences pivot point</param>
        /// <param name="gameObject">The fence GameObject itself</param>
        public Fence(Vector3 beginningPostOffset, Vector3 endPostOffset, GameObject gameObject)
        {
            this.beginningPostOffset = beginningPostOffset;
            this.endPostOffset = endPostOffset;
            this.gameObject = gameObject;
        }
    }

    private void Start()
    {
        _fences = LoadFences();
    }

    private void Update()
    {
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
    }

    private void OnGUI()
    {   //Draws the buttons that calls SelectFence
        float width = Screen.width / _fences.Length;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, 50));
        for (int i = 0; i < _fences.Length; i++)
        {
            if (GUI.Button(new Rect(width * i, 5, width, 40), _fences[i].gameObject.name))
            {
                SelectFence (_fences[i]);
            }
        }
        GUI.EndGroup();
    }

    private Fence[] LoadFences()
    {   //Returns an array with all of the fence objects to be used by the enclosure system
        //TODO: Load this from a JSON or XML file instead
        //TODO: Adjust offsets for all fences
        return new Fence[5] 
        {
            new Fence(new Vector3(0f, 0f, -0.9f), new Vector3(0f, 0f, 0.9f), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(Broken)") as GameObject),
            new Fence(new Vector3(0f, 0f, -0.9f), new Vector3(0f, 0f, 0.9f), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(End)") as GameObject),
            new Fence(new Vector3(0f, 0f, -0.9f), new Vector3(0f, 0f, 0.9f), Resources.Load("Buildings/Prefabs/Fences/LowWoodFence(Straight)") as GameObject),
            new Fence(new Vector3(0f, 0f, -0.9f), new Vector3(0f, 0f, 0.9f), Resources.Load("Buildings/Prefabs/Fences/LowWoodGate(Bar)") as GameObject),
            new Fence(new Vector3(0f, 0f, -0.9f), new Vector3(0f, 0f, 0.9f), Resources.Load("Buildings/Prefabs/Fences/LowWoodGate(Board)") as GameObject)
        };
    }   //LoadFences()



    //Can be called with different fences
    //When active, wait for click
    //Draw line from clicked pos to where cursor is
    //'Show' fences within the length of the line
    //Left click 'places' fences, right click cancels mode

    //Able to detect fences and "snap" on to them?

    private void SelectFence(Fence fence)
    {   //sets _selectedFence to fence and changes _state to Waiting_For_Input
        Debug.Log (fence.gameObject.name);
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
            GameObject g = GameObject.CreatePrimitive (PrimitiveType.Sphere);
            g.transform.position = _currentFenceLineStart;
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

        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit = new RaycastHit ();
        if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 currentFenceLineEnd = rayHit.point;
            Debug.DrawLine (_currentFenceLineStart, currentFenceLineEnd, Color.black);
        }
    }   //DrawLineOfFences()

    private void CancelDrawingLineOfFences()
    {   //Cancels the process, and reverts back to State.Idle
        _state = State.Idle;
    }   //CancelDrawingLineOfFences()

    private void FinalizeDrawingLineOfFences()
    {   //Finalizes the line of fences, and places the GameObjects into the scene
        _state = State.Idle;
    }   //FinalizeDrawingLineOfFences()
}