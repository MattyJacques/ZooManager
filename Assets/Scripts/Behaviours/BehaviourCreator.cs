// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using Assets.Scripts.Behaviours.Animal;
using Assets.Scripts.Behaviours.Base;
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

            _behaviours = new Dictionary<string, BehaviourTree>();

            var tree = new BehaviourTreeBuilder()
                      .AddSelector()
                          .AddSequence()
                              .AddConditional(AnimalBehaviours.TryFindNeedToImprove)
                              .AddAction(AnimalBehaviours.MoveToTarget)
                              .AddAction(AnimalBehaviours.ImproveNeed)
                          .AddSequence()
                              .AddAction(AnimalBehaviours.GetRandomInterestPoint)
                              .AddAction(AnimalBehaviours.MoveToTarget)
                      .Build();

            _behaviours.Add("basicAnimal", tree);
        }
    } 
}
