// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using Assets.Scripts.Behaviours.Animal;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Behaviours.General;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class BehaviourCreator
    {
        private Dictionary<string, BehaviourTree> _behaviours;

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

        public BehaviourTree GetBehaviour(string name)
        { // Return the behaviour with the given name

            if (_behaviours.ContainsKey(name))
            {
                Debug.Log("Behaviour \"" + name + "\" found");
                return _behaviours[name];
            }

            Debug.Log("Behaviour \"" + name + "\" not found");
            return null;
        }

        private void CreateBehaviours()
        { // Create all of the behaviours needed, storing them in the list

            _behaviours = new Dictionary<string, BehaviourTree>
            {
                {"basicAnimal", CreateBasicAnimalTree()},
                {"basicVisitor", CreateBasicVisitorTree()}
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
