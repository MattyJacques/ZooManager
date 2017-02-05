using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour 
{
    
    public Seeker Pathfinder;
    public CharacterController Controller;
    public float LastRepath;
    public float RepathRate;
    public Transform Target;
    public int CurrentWaypoint;
    public Path path;
    public bool HasArrived;
    public float Speed;
    public float ReachedDistance;
    public bool CanSearch;
    public bool CanMove;

    void Start()
    { // Initializes all needed parts for pathfinding

        Pathfinder = GetComponent<Seeker>();
        Controller = GetComponent<CharacterController>();

    } // InitPathfinding()

    void FixedUpdate()
    { // Update method, currently only used for path following

        // Repathing if repath time has been reached
        if(CanSearch && (Time.time - LastRepath > RepathRate) && Pathfinder.IsDone())
        {
            LastRepath = Time.time + Random.value * RepathRate * 0.5f;

            Pathfinder.StartPath(transform.position, Target.position, OnPathComplete);
        }

        if(CanMove && path != null && CurrentWaypoint < path.vectorPath.Count)
        { // We do have a path and are not at the end, now follow it

            Vector3 dir = (path.vectorPath[CurrentWaypoint] - transform.position).normalized;
            dir *= Speed;

            Controller.SimpleMove(dir);

            if(transform.position == path.vectorPath[CurrentWaypoint])
            { // Waypoint reached
                CurrentWaypoint++;
            }

        }
        else if(Vector3.Distance(transform.position, Target.position) < ReachedDistance)
        { // We reached the end of the path
            
            HasArrived = true; // We arrived at the target

        }

    } // Update()

    public void OnPathComplete(Path p)
    { // Is called when the Seeker has finished pathfinding
        if(!p.error)
        {
            path = p;
            CurrentWaypoint = 1; // 1 because 0 is the start point
        }
    } // OnPathComplete()
}