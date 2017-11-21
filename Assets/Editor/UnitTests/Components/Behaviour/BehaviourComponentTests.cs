// Sifaka Game Studios (C) 2017

using Assets.Scripts.Behaviours;
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
            behaviour.SelectedTemplateBehaviour = BehaviourTreeType.Unknown;

            LogAssert.Expect(LogType.Error, "Tried to load behaviour with name " + behaviour.SelectedTemplateBehaviour + " which does not exist!");
            behaviour.TestStart();
        }
    }
}
