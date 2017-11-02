// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.Base
{
    [TestFixture]
    public class BehaviourForceFailureTests
    {
        private Blackboard _blackboard;

        [SetUp]
        public void BeforeTest()
        {
            _blackboard = new Blackboard();
        }

        [TearDown]
        public void AfterTest()
        {
            _blackboard = null;
        }

        [UnityTest]
        public IEnumerator Behave_Fails()
        {
            var behaviour = new BehaviourForceFailure();

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_blackboard, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }
    }
}