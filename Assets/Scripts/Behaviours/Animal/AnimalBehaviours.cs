// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Needs;
using Assets.Scripts.Components.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Behaviours.Animal
{
    public static class AnimalBehaviours
    {
        public const string PathfindingTargetLocationKey = "PathfindingTarget";
        public const string PathfindingTargetTypeKey = "PathfindingTargetType";

        public const int MinNeedThreshold = 50;
        public const int NeedImproveAmount = 100;

        public static IEnumerator TryFindNeedToImprove(Blackboard inBlackboard, System.Action<bool> conditionResult)
        {
            conditionResult(false);
            var gameObject = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>();

            if (gameObject == null)
            {
                yield break;
            }

            var needsInterface = gameObject.GetComponent<INeedsInterface>();
            var enclosureResident = gameObject.GetComponent<EnclosureResidentComponent>();

            if (needsInterface != null && enclosureResident != null)
            {
                var lowestNeed = FindLowestApplicableNeed(needsInterface, enclosureResident);

                if (lowestNeed != null)
                {
                    inBlackboard.InstanceBlackboard.Add
                    (
                        PathfindingTargetLocationKey,
                        new BlackboardItem
                        (
                            enclosureResident.RegisteredEnclosure.GetClosestInteriorItemTransform
                            (
                                gameObject.transform.position, lowestNeed.GetNeedType()
                            ).position
                        )
                    );

                    inBlackboard.InstanceBlackboard.Add
                    (
                        PathfindingTargetTypeKey,
                        new BlackboardItem(lowestNeed)
                    );

                    conditionResult(true);
                }
            }
        }

        private static Need FindLowestApplicableNeed(INeedsInterface inNeedsInterface, EnclosureResidentComponent inEnclosureResident)
        {
            var needs = inNeedsInterface.GetNeeds();
            Need lowestNeed = null;
            foreach (var need in needs)
            {
                if (need.CurrentValue < MinNeedThreshold)
                {
                    if (lowestNeed == null || need.CurrentValue < lowestNeed.CurrentValue)
                    {
                        if (CanFindItemOfType(inEnclosureResident,
                            need.GetNeedType()))
                        {
                            lowestNeed = need;
                        }
                    }
                }
            }

            return lowestNeed;
        }

        private static bool CanFindItemOfType(EnclosureResidentComponent inEnclosureResident, NeedType inType)
        {
            if (inEnclosureResident != null)
            {
                if (inEnclosureResident.RegisteredEnclosure != null)
                {
                    if (inEnclosureResident.RegisteredEnclosure.GetClosestInteriorItemTransform(
                            inEnclosureResident.gameObject.transform.position, inType) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static IEnumerator ImproveNeed(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        {
            var needImproved = inBlackboard.InstanceBlackboard[PathfindingTargetTypeKey].GetCurrentItem<Need>();

            var gameObject = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>();
            var enclosureResident = gameObject.GetComponent<EnclosureResidentComponent>();

            var itemFound = enclosureResident.RegisteredEnclosure.GetClosestInteriorItemTransform(gameObject.transform.position, needImproved.GetNeedType()).gameObject;

            if (needImproved.GetNeedType() == NeedType.Hunger)
            {
                enclosureResident.RegisteredEnclosure.UnregisterInteriorItem(itemFound);
            }

            needImproved.AdjustNeed(NeedImproveAmount);

            inBlackboard.InstanceBlackboard.Remove(PathfindingTargetTypeKey);

            returnCode(ReturnCode.Success);

            yield break;
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

        public static IEnumerator GetRandomInterestPoint(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        { // Set's the AIBase pathfinder target to a random interest
            // point gotten from the IPManager
            Debug.Log("Getting random Interest Point");

            var ipvec = IPManager.Instance.GetRandomIP();

            if (ipvec == Vector3.zero)
            { // no interest points
                returnCode(ReturnCode.Failure);
                yield break;
            }

            inBlackboard.InstanceBlackboard.Add(PathfindingTargetLocationKey, new BlackboardItem(ipvec));

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
