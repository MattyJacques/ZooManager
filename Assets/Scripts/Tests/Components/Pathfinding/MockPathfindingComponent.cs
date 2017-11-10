// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Tests.Components.Pathfinding
{
    public class MockPathfindingComponent 
        : MonoBehaviour
        , IPathfindingInterface
    {
        public bool IsPathingResult = false;

        public bool StartPathfindingCalled = false;
        public Vector3 StartPathfindingTargetVector { get; set; }

        public void StartPathfinding(Vector3 target)
        {
            StartPathfindingCalled = true;
            StartPathfindingTargetVector = target;
        }

        public bool IsPathing()
        {
            return IsPathingResult;
        }
    }
}
