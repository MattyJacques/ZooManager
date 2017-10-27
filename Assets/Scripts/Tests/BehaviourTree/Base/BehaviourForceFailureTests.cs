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
    public class BehaviourForceFailureTests
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
        public IEnumerator Behave_Fails()
        {
            var behaviour = new BehaviourForceFailure();

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }
    }
}