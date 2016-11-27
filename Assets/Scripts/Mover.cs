using UnityEngine;
using System.Collections;
using Pathfinding; // must use pathfinding!

// movement requires the GameObject to have the seeker script and character controller, can remove these lines but just added them to help :)
[RequireComponent (typeof (Seeker))]
[RequireComponent(typeof(CharacterController))]

public class Mover : MonoBehaviour {
    public Transform target; //destination, I think behaviour tree will need to pass this in?
    public float moveSpeed = 10;
    public float maxWaypointDistance =2f;
    Seeker seeker;
    Path path;
    int CurrentWaypoint;
    CharacterController characterController;
    
    void Start () {

        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        characterController = GetComponent<CharacterController>();
    } // Start

    public void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            CurrentWaypoint = 0;
        }
        else
        {
            Debug.LogError(p.error);
        }
    } // OnPathComplete

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (CurrentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector3 dir = (path.vectorPath[CurrentWaypoint] - transform.position).normalized * moveSpeed; // dir we need to move in * speed, we don't need * Time because char controller uses it in simpleMove
        characterController.SimpleMove(dir);
        if (Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]) < maxWaypointDistance)
        {
            CurrentWaypoint++;
        }
    } // FixedUpdate
}
