// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Pathfinding;
using Assets.Scripts.Services;
using Assets.Scripts.Services.PointsOfInterest;
using UnityEngine;

namespace Assets.Scripts.Behaviours.General
{
    public static class PathfindingBehaviours
    {
        public const string PathfindingTargetLocationKey = "PathfindingTarget";
        public const string PathfindingTargetTypeKey = "PathfindingTargetType";

        public static IEnumerator GetRandomInterestPoint(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        { // Set's the AIBase pathfinder target to a random interest
            // point gotten from the IPManager
            Debug.Log("Getting random Interest Point");

            var ipvec = GameServiceProvider.CurrentInstance.GetService<IPointsOfInterestService>()
                .GetRandomPointOfInterest();

            if (ipvec.Equals(PointsOfInterestConstants.InvalidPointOfInterest))
            { // no interest points
                returnCode(ReturnCode.Failure);
                yield break;
            }

            if (inBlackboard.InstanceBlackboard.ContainsKey(PathfindingTargetLocationKey))
            {
                inBlackboard.InstanceBlackboard.Remove(PathfindingTargetLocationKey);
            }

            inBlackboard.InstanceBlackboard.Add(PathfindingTargetLocationKey, new BlackboardItem(ipvec));

            returnCode(ReturnCode.Success);
        }

        public static IEnumerator MoveToTarget(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        { // Move to the target

            Debug.Log("Starting MoveToTarget()");

            var gameObject = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>();
            var pathfinder = gameObject.GetComponent<IPathfindingInterface>();

            pathfinder.StartPathfinding(inBlackboard.InstanceBlackboard[PathfindingTargetLocationKey].GetCurrentItem<Vector3>());

            while (pathfinder.IsPathing())
            {
                yield return null;
            }

            Debug.Log("MoveToTarget(), returning success");

            inBlackboard.InstanceBlackboard.Remove(PathfindingTargetLocationKey);

            returnCode(ReturnCode.Success);
        }

        public static IEnumerator GetRandomPointInsideEnclosure(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        {
            Debug.Log("Getting random point inside Enclosure");

            var enclosureResident = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>().GetComponent<EnclosureResidentComponent>();

            inBlackboard.InstanceBlackboard.Add(PathfindingTargetLocationKey,
                new BlackboardItem(enclosureResident.RegisteredEnclosure.GetRandomPointOnTheGround()));

            returnCode(ReturnCode.Success);
            yield break;
        }
    }
}
