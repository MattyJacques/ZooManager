// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Behaviours;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Characters;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours
{
    public class BehaviourTreeBuilderTestFixture
    {
        private bool _actionCompleted;
        private AIBase _aiBase;

        [SetUp]
        public void BeforeTest()
        {
            _aiBase = new AIBase();
            _actionCompleted = false;
        }

        [TearDown]
        public void AfterTest()
        {
            _aiBase = null;
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
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_aiBase, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        [UnityTest]
        public IEnumerator AddPrimitiveBehaviour_ParentIsSelector_ReturnsErrorAndNotAdded()
        {
            LogAssert.Expect(LogType.Error, "Conditional needs a sequence as a parent.");
            var tree = new BehaviourTreeBuilder().AddSelector().AddAction(ActivateTestDelegate).Build();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Root.Behave(_aiBase, code => { }));

            Assert.IsTrue(_actionCompleted);
        }

        private IEnumerator ActivateTestDelegate(AIBase inAIBase, Action<ReturnCode> returnFunc )
        {
            _actionCompleted = true;
            yield return null;
        }
    }
}
