// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours
{
    public class BehaviourTreeBuilderTestFixture
    {
        private bool _actionCompleted;
        private bool _otherActionCompleted;
        private Blackboard _blackboard;

        [SetUp]
        public void BeforeTest()
        {
            _blackboard = new Blackboard();
            _actionCompleted = false;
            _otherActionCompleted = false;
        }

        [TearDown]
        public void AfterTest()
        {
            _blackboard = null;
        }

        [Test]
        public void NoNodes_RootIsNull()
        {
            var tree = new BehaviourTreeBuilder().Build();

            Assert.Null(tree.Root);
        }

        [UnityTest]
        public IEnumerator AddPrimitiveBehaviour_RootIsAction()
        {
            var tree = new BehaviourTreeBuilder().AddAction(ActivateTestDelegate).Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        [UnityTest]
        public IEnumerator AddPrimitiveBehaviour_AlreadyPrimitiveAsRoot_ThrowsErrorAndDoesNotAdd()
        {
            LogAssert.Expect(LogType.Error, "Primitive already added as root!");
            var tree = new BehaviourTreeBuilder().AddAction(ActivateTestDelegate).AddAction(OtherActivateTestDelegate).Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
            Assert.IsFalse(_otherActionCompleted);
        }

        [UnityTest]
        public IEnumerator AddPrimitiveBehaviour_ParentIsSelector_ReturnsErrorAndNotAdded()
        {
            LogAssert.Expect(LogType.Error, "Primitive needs a sequence as a parent.");
            var tree = new BehaviourTreeBuilder().AddSelector().AddAction(ActivateTestDelegate).Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsFalse(_actionCompleted);
        }

        [UnityTest]
        public IEnumerator AddPrimitiveBehaviour_ParentIsSequence_AddedAsPartOfSequence()
        {
            var tree = new BehaviourTreeBuilder().AddSequence().AddAction(ActivateTestDelegate).Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        [Test]
        public void AddSequence_ParentIsSequence_ThrowsError()
        {
            LogAssert.Expect(LogType.Error, "Composite behaviour could not be added!");

            new BehaviourTreeBuilder().AddSequence().AddSequence().Build();
        }

        [Test]
        public void AddSelector_ParentIsSequence_ThrowsError()
        {
            LogAssert.Expect(LogType.Error, "Composite behaviour could not be added!");

            new BehaviourTreeBuilder().AddSequence().AddSelector().Build();
        }

        [UnityTest]
        public IEnumerator AddSequence_ParentIsPrimitive_ThrowsErrorAndIsNotAdded()
        {
            LogAssert.Expect(LogType.Error, "Primitive behaviour already root!");

            var tree = new BehaviourTreeBuilder().AddAction(ActivateTestDelegate).AddSequence().Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        [UnityTest]
        public IEnumerator AddSelector_ParentIsPrimitive_ThrowsErrorAndIsNotAdded()
        {
            LogAssert.Expect(LogType.Error, "Primitive behaviour already root!");

            var tree = new BehaviourTreeBuilder().AddAction(ActivateTestDelegate).AddSelector().Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        [Test]
        public void AddSelector_ParentIsSelector_ThrowsError()
        {
            LogAssert.Expect(LogType.Error, "Composite behaviour could not be added!");

            new BehaviourTreeBuilder().AddSelector().AddSelector().Build();
        }

        [UnityTest]
        public IEnumerator AddSequence_NoRoot_ConstructsAsExpected()
        {
            var tree =
                new BehaviourTreeBuilder().
                    AddSequence()
                        .AddAction(ActivateTestDelegate)
                        .AddAction(OtherActivateTestDelegate)
                    .Build();

            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
            Assert.IsTrue(_otherActionCompleted);
        }

        [UnityTest]
        public IEnumerator AddSequence_ParentIsSelector_ConstructsAsExpected()
        {
            var tree = 
                new BehaviourTreeBuilder().
                    AddSelector()
                        .AddSequence()
                            .AddConditional(FailConditional)
                            .AddAction(ActivateTestDelegate)
                        .AddSequence()
                            .AddAction(OtherActivateTestDelegate)
                    .Build();

            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsFalse(_actionCompleted);
            Assert.IsTrue(_otherActionCompleted);
        }

        [UnityTest]
        public IEnumerator AddSequence_ParentIsSelector_OrderIsCorrect()
        {
            var tree =
                new BehaviourTreeBuilder().
                    AddSelector()
                    .AddSequence()
                        .AddAction(AssertFirstTestDelegate)
                        .AddForceFailure()
                    .AddSequence()
                        .AddAction(ActivateTestDelegate)
                    .Build();

            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_blackboard, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        private IEnumerator ActivateTestDelegate(Blackboard inBlackboard, Action<ReturnCode> returnFunc )
        {
            _actionCompleted = true;
            returnFunc(ReturnCode.Success);
            yield return null;
        }

        private IEnumerator OtherActivateTestDelegate(Blackboard inBlackboard, Action<ReturnCode> returnFunc)
        {
            _otherActionCompleted = true;
            returnFunc(ReturnCode.Success);
            yield return null;
        }

        private IEnumerator AssertFirstTestDelegate(Blackboard inBlackboard, Action<ReturnCode> returnFunc)
        {
            Assert.IsFalse(_otherActionCompleted);
            Assert.IsFalse(_actionCompleted);
            returnFunc(ReturnCode.Success);
            yield return null;
        }

        private IEnumerator FailConditional(Blackboard inBlackboard, Action<bool> returnFunc)
        {
            returnFunc(false);
            yield return null;
        }
    }
}
