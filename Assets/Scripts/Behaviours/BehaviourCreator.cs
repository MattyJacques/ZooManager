// Sifaka Game Studios (C) 2017

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Needs;
using Assets.Scripts.Components.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class BehaviourCreator
    {
        private Dictionary<string, BehaviourTree> _behaviours;

        private static BehaviourCreator _instance;

        private static string PathfindingTargetLocationKey = "PathfindingTarget";
        private static string PathfindingTargetTypeKey = "PathfindingTargetType";

        private static int minNeedThreshold = 50;

        public static BehaviourCreator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BehaviourCreator();
                    _instance.CreateBehaviours();
                }

                return _instance;
            }
        }

        public BehaviourTree GetBehaviour(string name)
        { // Return the behaviour with the given name

            if (_behaviours.ContainsKey(name))
            {
                Debug.Log("Behaviour \"" + name + "\" found");
                return _behaviours[name];
            }
            else
            {
                Debug.Log("Behaviour \"" + name + "\" not found");
                return null;
            }

        }

        private void CreateBehaviours()
        { // Create all of the behaviours needed, storing them in the list

            _behaviours = new Dictionary<string, BehaviourTree>();

            var tree = new BehaviourTreeBuilder()
                      .AddSelector()
                          .AddSequence()
                              .AddConditional(TryFindNeedToImprove)
                              .AddAction(MoveToTarget)
                              .AddAction(ImproveNeed)
                          .AddSequence()
                              .AddAction(GetRandomInterestPoint)
                              .AddAction(MoveToTarget)
                      .Build();

            _behaviours.Add("basicAnimal", tree);

        }

        private static IEnumerator TryFindNeedToImprove(Blackboard inBlackboard, System.Action<bool> conditionResult)
        {
            var gameObject = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>();
            var needsInterface = gameObject.GetComponent<INeedsInterface>();

            var enclosureResident = gameObject.GetComponent<EnclosureResidentComponent>();

            if (needsInterface != null)
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
                            )
                         )
                    );

                    inBlackboard.InstanceBlackboard.Add
                    (
                        PathfindingTargetTypeKey,
                        new BlackboardItem(lowestNeed.GetNeedType())
                    );

                    conditionResult(true);
                    yield return null;
                }
            }

            conditionResult(false);
        }

        private static Need FindLowestApplicableNeed(INeedsInterface inNeedsInterface, EnclosureResidentComponent inEnclosureResident)
        {
            var needs = inNeedsInterface.GetNeeds();
            Need lowestNeed = null;
            foreach (var need in needs)
            {
                if (need.CurrentValue < minNeedThreshold)
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

        private static IEnumerator ImproveNeed(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        {
            var needImproved = inBlackboard.InstanceBlackboard[PathfindingTargetTypeKey].GetCurrentItem<Need>();

            var gameObject = inBlackboard.InstanceBlackboard[BehaviourTree.GameObjectKey].GetCurrentItem<GameObject>();
            var enclosureResident = gameObject.GetComponent<EnclosureResidentComponent>();

            var itemFound = enclosureResident.RegisteredEnclosure.GetClosestInteriorItemTransform(gameObject.transform.position, needImproved.GetNeedType()).gameObject;

            if (needImproved.GetNeedType() == NeedType.Hunger)
            {
                enclosureResident.RegisteredEnclosure.UnregisterInteriorItem(itemFound);
            }

            needImproved.AdjustNeed(100);

            inBlackboard.InstanceBlackboard.Remove(PathfindingTargetTypeKey);
            

            yield break;
        }

        private static IEnumerator MoveToTarget(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
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

        private static IEnumerator GetRandomInterestPoint(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
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

        private static IEnumerator GetRandomPointInsideEnclosure(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
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
