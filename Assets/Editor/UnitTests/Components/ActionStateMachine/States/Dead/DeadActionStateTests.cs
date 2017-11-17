// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Editor.UnitTests.Components.UnityEvent;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.UnityEvent;
using Assets.Scripts.Tests.Components.UnityEvent;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Dead
{
    [TestFixture]
    public class DeadActionStateTestFixture
    {
        private GameObject _owner;

        [SetUp]
        public void BeforeTest()
        {
            _owner = new GameObject();
            _owner.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _owner = null;
        }

        [Test]
        public void GetActionStateId_Dead()
        {
            var deadActionState = new DeadActionState(new ActionStateInfo());

            Assert.AreEqual(EActionStateId.Dead, deadActionState.ActionStateId);
        }

        [Test]
        public void Start_EnterDeadActionStateMessageFired()
        {
            var messageCapture = new UnityTestMessageHandleResponseObject<EnteredDeadActionStateMessage>();
            var handle = UnityMessageEventFunctions.
                RegisterActionWithDispatcher<EnteredDeadActionStateMessage>(_owner, messageCapture.OnResponse);
            var deadActionState = new DeadActionState(new ActionStateInfo(_owner));

            deadActionState.Start();

            Assert.IsTrue(messageCapture.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_owner, handle);
        }

        [Test]
        public void End_LeftDeadActionStateMessageFired()
        {
            var messageCapture = new UnityTestMessageHandleResponseObject<LeftDeadActionStateMessage>();
            var handle = UnityMessageEventFunctions.
                RegisterActionWithDispatcher<LeftDeadActionStateMessage>(_owner, messageCapture.OnResponse);
            var deadActionState = new DeadActionState(new ActionStateInfo(_owner));

            deadActionState.Start();
            Assert.IsFalse(messageCapture.ActionCalled);

            deadActionState.End();
            Assert.IsTrue(messageCapture.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_owner, handle);
        }
    }
}

#endif
