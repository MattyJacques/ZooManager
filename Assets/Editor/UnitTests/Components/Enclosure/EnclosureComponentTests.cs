// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Tests.Components.Enclosure;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Enclosure
{
    [TestFixture]
    public class EnclosureComponentTestFixture
    {
        private TestEnclosureComponent _enclosureComponent;
        private EnclosureResidentComponent _expectedResident;

        [SetUp]
        public void BeforeTest()
        {
            _enclosureComponent = new GameObject().AddComponent<BoxCollider>().gameObject.AddComponent<MeshCollider>()
                .gameObject.AddComponent<TestEnclosureComponent>();

            _expectedResident = new GameObject().AddComponent<EnclosureResidentComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _expectedResident = null;
            _enclosureComponent = null;
        }

        [Test]
        public void RegisterEnclosureResident_UpdatesResident()
        {
            _enclosureComponent.RegisterEnclosureResident(_expectedResident);

            Assert.AreSame(_enclosureComponent, _expectedResident.RegisteredEnclosure);
        }

        [Test]
        public void UnregisterEnclosureResident_UpdatesResident()
        {
            _enclosureComponent.RegisterEnclosureResident(_expectedResident);
            _enclosureComponent.UnregisterEnclosureResident(_expectedResident);

            Assert.IsNull(_expectedResident.RegisteredEnclosure);
        }

        [Test]
        public void UnregisterEnclosureResident_NotRegistered_ErrorThrown()
        {
            LogAssert.Expect(LogType.Error, "Tried to remove a resident that didn't exist!");
            _enclosureComponent.UnregisterEnclosureResident(_expectedResident);
        }
    }
}
