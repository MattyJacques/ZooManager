// Sifaka Game Studios (C) 2017

using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

namespace Assets.Scripts.Components.Pathfinding
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(RVOController))]
    public class PathfindingComponent 
        : MonoBehaviour
        , IPathfindingInterface
    {
        public float DestinationThresholdDistance = 2.5f;
        public float WaypointThresholdDistance = 1f;
        public float RepathRate = 0.5f;
        public float Speed = 10f;

        private float _timeOnCurrentNode = 0f;
        private Seeker _pathfinder;
        public RVOController Controller;
        private Vector3 _target;
        private int _currentWaypoint;
        private Path _path;

        private void Start()
        { // Initializes all needed parts for pathfinding

            _pathfinder = GetComponent<Seeker>();
            Controller = GetComponent<RVOController>();

            Controller.maxSpeed = Speed;
        }

        private void FixedUpdate()
        { // Update method, currently only used for path following
            if (IsPathing())
            {
                _timeOnCurrentNode += Time.fixedDeltaTime;
                if (ReachedDestination())
                { // We reached the end of the path
                    OnReachedDestination();
                    return;
                }
                
                // Repathing if repath time has been reached
                if (ShouldRepath())
                {
                    Repath();
                }
                
                if (CanTraversePath())
                { // We do have a path and are not at the end, now follow it
                    TraversePath();
                }
            }
        }

        private bool ReachedDestination()
        {
            return Vector3.Distance(transform.position, _target) < DestinationThresholdDistance;
        }

        private void OnReachedDestination()
        {
            Debug.Log("Arrived at _target");
            _path = null;
        }

        private bool ShouldRepath()
        {
            return (_timeOnCurrentNode > RepathRate) && _pathfinder.IsDone();
        }

        private void Repath()
        {
            _timeOnCurrentNode = 0f;

            StartPathfinding(_target);
        }

        private bool CanTraversePath()
        {
            return _path != null && _currentWaypoint < _path.vectorPath.Count;
        }

        private void TraversePath()
        {
            Vector3 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            gameObject.transform.localRotation = Quaternion.Euler(dir);
            dir *= Speed;

            Controller.Move(dir);

            if (Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]) < WaypointThresholdDistance)
            { // Waypoint reached
                _currentWaypoint++;
                _timeOnCurrentNode = 0f;
            }
        }

        private void OnPathSelectionComplete(Path p)
        { // Is called when the Seeker has finished finding the wanted path
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 1; // 1 because 0 is the start point
                Controller.locked = false;
            }
        }

        // IPathfindingInterface
        public void StartPathfinding(Vector3 inTarget)
        {
            _target = inTarget;
            _pathfinder.StartPath(transform.position, _target, OnPathSelectionComplete);
        }

        public bool IsPathing()
        {
            return _path != null;
        }
        // ~IPathfindingInterface
    }
}