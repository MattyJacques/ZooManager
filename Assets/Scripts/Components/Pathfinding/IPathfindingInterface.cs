// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.Pathfinding
{
    public interface IPathfindingInterface
    {
        void StartPathfinding(Vector3 target);
        bool IsPathing();
    }
}
