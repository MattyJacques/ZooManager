// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Tests.Components.UnityEvent;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.UnityEvent
{
    [TestFixture]
    public class UnityMessageEventDispatcherComponentTestFixture {

        [Test]
        public void Awake_GetMessageEventDispatcher_ReturnsValidDispatcher()
        {
            var gameObject = new GameObject();

            var unityMessageEventDispatcherComponent =
                gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>();

            unityMessageEventDispatcherComponent.TestAwake();

            Assert.NotNull(unityMessageEventDispatcherComponent.GetUnityMessageEventDispatcher());
        }
    }
}

#endif
