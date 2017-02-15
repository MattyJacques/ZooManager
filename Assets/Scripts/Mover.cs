using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(RVOController))]
public class Mover : MonoBehaviour 
{
    
    public Seeker Pathfinder;
    public RVOController Controller;
    public float LastRepath;
    public float RepathRate = 0.5f;
    public Transform Target;
    public int CurrentWaypoint;
    public Path path;
    public bool HasArrived = false;
    public float Speed = 2f;
    public float ReachedDistance = 1f;
    public bool CanSearch = true;
    public bool CanMove = false;
    public float WaypointDistance = 1f;

    void Start()
    { // Initializes all needed parts for pathfinding

        Pathfinder = GetComponent<Seeker>();
        Controller = GetComponent<RVOController>();

        Controller.maxSpeed = Speed;

    } // InitPathfinding()

    void FixedUpdate()
    { // Update method, currently only used for path following

        // Repathing if repath time has been reached
        if(CanSearch && Target != null && (Time.time - LastRepath > RepathRate) && Pathfinder.IsDone())
        {
            LastRepath = Time.time + Random.value * RepathRate * 0.5f;

            Pathfinder.StartPath(transform.position, Target.position, OnPathComplete);
        }

        if(CanMove && path != null && CurrentWaypoint < path.vectorPath.Count)
        { // We do have a path and are not at the end, now follow it

            Vector3 dir = (path.vectorPath[CurrentWaypoint] - transform.position).normalized;
            dir *= Speed;

            Controller.Move(dir);

            if(Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]) < WaypointDistance)
            { // Waypoint reached
                CurrentWaypoint++;
            }

        }

        if(Target != null && Vector3.Distance(transform.position, Target.position) < ReachedDistance)
        { // We reached the end of the path
            
            HasArrived = true; // We arrived at the target
            Debug.Log("Arrived at Target");
            Target = null;
            path = null;

        }

        if(!CanMove || Target == null || HasArrived)
        {
            Controller.Move(Vector3.zero);
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