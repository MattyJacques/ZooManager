// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Characters;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.Base
{
    public class BehaviourTreeTestFixture
    {
        [Test]
        public void ConstructedWithBehaviour_AddedAsRoot()
        {
            var baseBehaviour = new TestBehaviour(ReturnCode.Success);
            var tree = new BehaviourTree(baseBehaviour);

            Assert.AreSame(tree.Root, baseBehaviour);
        }

        [UnityTest]
        public IEnumerator ConstructedWithNullBehaviour_BehaveCalled_ErrorThrown()
        {
            var tree = new BehaviourTree(null);

            LogAssert.Expect(LogType.Error, "Root is null!");

            var aiBase = new AIBase();
            yield return CoroutineSys.Instance.StartCoroutine(tree.Behave(aiBase));
        }
    }
}
