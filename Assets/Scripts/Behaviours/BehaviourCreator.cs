// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using Assets.Scripts.Behaviours.Animal;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Behaviours.General;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public enum BehaviourTreeType
    {
        BasicAnimal,
        BasicVisitor,
        Unknown
    }

    public class BehaviourCreator
    {
        
        private Dictionary<BehaviourTreeType, BehaviourTree> _behaviours;

        private static BehaviourCreator _instance;

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

        public BehaviourTree GetBehaviour(BehaviourTreeType inTreeType)
        { // Return the behaviour with the given name

            if (_behaviours.ContainsKey(inTreeType))
            {
                Debug.Log("Behaviour \"" + inTreeType + "\" found");
                return _behaviours[inTreeType];
            }

            Debug.Log("Behaviour \"" + inTreeType + "\" not found");
            return null;
        }

        private void CreateBehaviours()
        { // Create all of the behaviours needed, storing them in the list

            _behaviours = new Dictionary<BehaviourTreeType, BehaviourTree>
            {
                {BehaviourTreeType.BasicAnimal, CreateBasicAnimalTree()},
                {BehaviourTreeType.BasicVisitor, CreateBasicVisitorTree()}
            };

        }

        private static BehaviourTree CreateBasicAnimalTree()
        {
            return new BehaviourTreeBuilder()
                .AddSelector()
                    .AddSequence()
                        .AddConditional(AnimalBehaviours.TryFindNeedToImprove)
                        .AddAction(PathfindingBehaviours.MoveToTarget)
                        .AddAction(AnimalBehaviours.ImproveNeed)
                    .AddSequence()
                        .AddAction(PathfindingBehaviours.GetRandomPointInsideEnclosure)
                        .AddAction(PathfindingBehaviours.MoveToTarget)
                .Build();
        }

        private static BehaviourTree CreateBasicVisitorTree()
        {
            return new BehaviourTreeBuilder()
                .AddSelector()
                    .AddSequence()
                        .AddAction(PathfindingBehaviours.GetRandomInterestPoint)
                        .AddAction(PathfindingBehaviours.MoveToTarget)
                .Build();
        }
    } 
}
