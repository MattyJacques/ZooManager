// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviours.Base;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class BehaviourTreeBuilder
    {
        private delegate bool CompositeBuilderFailureDelegate();

        public enum BehaviourType
        {
            Sequence,
            Selector,
        }

        public class IntermediateBehaviour
        {
            public readonly BehaviourType HeadBehaviourType;
            private IList< BehaviourBase > ChildBehaviours { get; set; }

            public IntermediateBehaviour(BehaviourType inBehaviourType)
            {
                HeadBehaviourType = inBehaviourType;
                ChildBehaviours = new List<BehaviourBase>();
            }

            public void AddBehaviour(BehaviourBase inBehaviour)
            {
                ChildBehaviours.Add(inBehaviour);
            }

            public BehaviourBase AssembleCompositeBehaviour()
            {
                BehaviourBase createdBehaviour = null;
                switch (HeadBehaviourType)
                {
                    case BehaviourType.Selector:
                        createdBehaviour = new BehaviourSelector(ChildBehaviours.ToArray());
                        break;
                    case BehaviourType.Sequence:
                        createdBehaviour = new BehaviourSequence(ChildBehaviours.ToArray());
                        break;
                    default:
                        Debug.LogError("Attemped to build a behaviour type that did not exist!");
                        break;
                }

                return createdBehaviour;
            }
        }

        private BehaviourTree CurrentTree { get; set; }
        private BehaviourBase Root { get; set; }
        private IList<IntermediateBehaviour> IntermediateBehaviours { get; set; }

        public BehaviourTreeBuilder()
        {
            CurrentTree = null;
            IntermediateBehaviours = new List<IntermediateBehaviour>();
        }

        // Selector must be a root
        public BehaviourTreeBuilder AddSelector()
        {
            return AddCompositeBehaviour(BehaviourType.Selector, () => IntermediateBehaviours.Count > 0);
        }

        // Sequence must be the root or have selector as a parent
        public BehaviourTreeBuilder AddSequence()
        {
            return AddCompositeBehaviour(BehaviourType.Sequence, () => IntermediateBehaviours.Count != 0 &&
                                                                IntermediateBehaviours.First().HeadBehaviourType != BehaviourType.Selector);
        }

        private BehaviourTreeBuilder AddCompositeBehaviour(BehaviourType inBehaviourType, CompositeBuilderFailureDelegate inDelegate)
        {
            if (Root != null)
            {
                Debug.LogError("Primitive behaviour already root!");
                return this;
            }

            if (inDelegate())
            {
                Debug.LogError("Composite behaviour could not be constructed!");
                return this;
            }

            IntermediateBehaviours.Add(new IntermediateBehaviour(inBehaviourType));

            return this;
        }

        public BehaviourTreeBuilder AddConditional(ConditionalDelegate inDelegate)
        {
            return AddPrimitiveBehaviour(new BehaviourConditional(inDelegate));
        }

        public BehaviourTreeBuilder AddForceFailure()
        {
            return AddPrimitiveBehaviour(new BehaviourForceFailure());
        }

        public BehaviourTreeBuilder AddAction(ActionDelegate inAction)
        {
            return AddPrimitiveBehaviour(new BehaviourAction(inAction));
        }

        // Primitives must either be the root or have a sequence as a parent
        private BehaviourTreeBuilder AddPrimitiveBehaviour(BehaviourBase inPrimitive)
        {
            if (IntermediateBehaviours.Count == 0)
            {
                Root = inPrimitive;
                return this;
            }

            if (IntermediateBehaviours.Last().HeadBehaviourType != BehaviourType.Sequence)
            {
                Debug.LogError("Conditional needs a sequence as a parent.");
                return this;
            }

            IntermediateBehaviours.Last().AddBehaviour(inPrimitive);
            return this;
        }

        public BehaviourTree Build()
        {
            AssembleCompositeBehaviours(new List<BehaviourBase>());
            CurrentTree = new BehaviourTree(Root);
            return CurrentTree;
        }

        // Recursively build the tree
        private void AssembleCompositeBehaviours(IList<BehaviourBase> assembledBehaviours)
        {
            if (assembledBehaviours != null)
            {
                if (IntermediateBehaviours.Count > 1)
                {
                    assembledBehaviours.Add(IntermediateBehaviours.Last().AssembleCompositeBehaviour());
                    IntermediateBehaviours.RemoveAt(IntermediateBehaviours.Count - 1);
                    AssembleCompositeBehaviours(assembledBehaviours);
                }
                else if (IntermediateBehaviours.Count == 1)
                {
                    foreach (var assembledBehaviour in assembledBehaviours)
                    {
                        IntermediateBehaviours.First().AddBehaviour(assembledBehaviour);
                    }

                    Root = IntermediateBehaviours.First().AssembleCompositeBehaviour();
                }
            }
        }
    }
}
