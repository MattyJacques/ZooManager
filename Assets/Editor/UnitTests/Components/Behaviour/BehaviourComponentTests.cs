// Sifaka Game Studios (C) 2017

using Assets.Scripts.Tests.Components.Behaviour;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Behaviour
{
    [TestFixture]
    public class BehaviourComponentTestFixture
    {
        [Test]
        public void Start_NoValidBehaviour_ThrowsError()
        {
            var behaviour = new GameObject().AddComponent<TestBehaviourComponent>();
            behaviour.BehaviourName = "Terrible test behaviour name that will never exist";

            LogAssert.Expect(LogType.Error, "Tried to load behaviour with name " + behaviour.BehaviourName + " which does not exist!");
            behaviour.TestStart();
        }
    }
}
