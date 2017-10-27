// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Characters;
using Assets.Scripts.Tests.Characters;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.BehaviourTree.Base
{
    [TestFixture]
    public class BehaviourSequenceTests
    {
        private AIBase _aiBase;

        [SetUp]
        public void BeforeTest()
        {
            _aiBase = new AIBaseBuilder().Build();
        }

        [TearDown]
        public void AfterTest()
        {
            _aiBase = null;
        }

        [UnityTest]
        public IEnumerator FirstBehaviourSucceeds_ExecutesUntilFailure()
        {
            var behaviours = new TestBehaviour[3];
            behaviours[0] = new TestBehaviour(ReturnCode.Success);
            behaviours[1] = new TestBehaviour(ReturnCode.Failure);
            behaviours[2] = new TestBehaviour(ReturnCode.Success);

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code => { }));

            Assert.IsTrue(behaviours[0].BehaveCalled);
            Assert.IsTrue(behaviours[1].BehaveCalled);
            Assert.IsFalse(behaviours[2].BehaveCalled);
        }

        [UnityTest]
        public IEnumerator FirstBehaviourSucceeds_ReturnsFailureIfOtherFails()
        {
            var behaviours = new TestBehaviour[2];
            behaviours[0] = new TestBehaviour(ReturnCode.Success);
            behaviours[1] = new TestBehaviour(ReturnCode.Failure);

            var actualReturnCode = ReturnCode.Success;

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code =>
            {
                actualReturnCode = code;
            }));

            Assert.AreEqual(ReturnCode.Failure, actualReturnCode);
        }

        [UnityTest]
        public IEnumerator AllBehavioursSucceed_ExecutesAllBehaviours()
        {
            var behaviours = new TestBehaviour[3];
            behaviours[0] = new TestBehaviour(ReturnCode.Success);
            behaviours[1] = new TestBehaviour(ReturnCode.Success);
            behaviours[2] = new TestBehaviour(ReturnCode.Success);

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code => { }));

            Assert.IsTrue(behaviours[0].BehaveCalled);
            Assert.IsTrue(behaviours[1].BehaveCalled);
            Assert.IsTrue(behaviours[2].BehaveCalled);
        }

        [UnityTest]
        public IEnumerator AllBehavioursSucceed_ReturnsSuccess()
        {
            var behaviours = new TestBehaviour[3];
            behaviours[0] = new TestBehaviour(ReturnCode.Success);
            behaviours[1] = new TestBehaviour(ReturnCode.Success);
            behaviours[2] = new TestBehaviour(ReturnCode.Success);

            var actualReturnCode = ReturnCode.Failure;

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code =>
            {
                actualReturnCode = code;
            }));

            Assert.AreEqual(ReturnCode.Success, actualReturnCode);
        }

        [UnityTest]
        public IEnumerator BehaviourRunning_Fails()
        {
            var behaviours = new TestBehaviour[1];
            behaviours[0] = new TestBehaviour(ReturnCode.Running);

            var actualReturnCode = ReturnCode.Success;

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code =>
            {
                actualReturnCode = code;
            }));

            Assert.AreEqual(ReturnCode.Failure, actualReturnCode);
        }

        [UnityTest]
        public IEnumerator BehaviourRunning_EarlyOuts()
        {
            var behaviours = new TestBehaviour[2];
            behaviours[0] = new TestBehaviour(ReturnCode.Running);
            behaviours[1] = new TestBehaviour(ReturnCode.Success);

            var behaviour = new BehaviourSequence(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code => { }));

            Assert.IsTrue(behaviours[0].BehaveCalled);
            Assert.IsFalse(behaviours[1].BehaveCalled);
        }

        [UnityTest]
        public IEnumerator NoBehaviours_Fails()
        {
            var behaviours = new TestBehaviour[0];

            var actualReturnCode = ReturnCode.Success;

            var behaviour = new BehaviourSelector(behaviours);

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, code =>
            {
                actualReturnCode = code;
            }));

            Assert.AreEqual(ReturnCode.Failure, actualReturnCode);
        }
    }
}
