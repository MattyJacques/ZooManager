// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.ActionStateMachine;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class NullActionStateTestFixture
    {
        [Test]
        public void NullActionState_Initialised_HasNullId()
        {
            Assert.AreEqual(new NullActionState().ActionStateId, EActionStateId.Null);
        }
    }
}
