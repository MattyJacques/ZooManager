using UnityEngine;
using System.Collections;
using Pathfinding;

public class Mover : MonoBehaviour {


    public Transform target;
    public float maxWaypointDistance = 2;
    public float moveSpeed = 10;

    Seeker seeker;
    Path path;
    int currentWaypoint;
    CharacterController characterController;

    void Start () {
        seeker = GetComponent<Seeker>();
        // Tell Seeker to generate path
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        characterController = GetComponent<CharacterController>();

    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        { path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.LogError(p.error);
        }
    }

	void FixedUpdate () {
	if (path == null)
        {
            return;
        }
    if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized * moveSpeed;
        characterController.SimpleMove(dir);
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < maxWaypointDistance)
        {
            currentWaypoint++;
        }
	}
}
