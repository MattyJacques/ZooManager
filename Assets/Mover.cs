using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
    Transform target;
    Seeker seeker;

	void Start () {

        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
	}
	
    public void OnPathComplete (Pathfinding // here, pos 22:29 in vid
        
}
