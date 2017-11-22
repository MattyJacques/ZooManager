// Sifaka Game Studios (C) 2017

using Assets.Scripts.Services;
using Assets.Scripts.Services.PointsOfInterest;
using Assets.Scripts.Tests.Services;
using Assets.Scripts.Tests.Services.PointsOfInterest;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.PointsOfInterest
{
    [TestFixture]
    public class PointOfInterestComponentTestFixture
    {
        private TestPointOfInterestComponent _poiComponent;
        private MockPointsOfInterestService _poiService;

        [SetUp]
        public void BeforeTest()
        {
            _poiComponent = new GameObject().AddComponent<TestPointOfInterestComponent>();
            _poiComponent.gameObject.transform.position = new Vector3(300f, 900f, 800f);

            _poiComponent.gameObject.AddComponent<TestGameServiceProvider>().TestAwake();
            _poiService = new MockPointsOfInterestService();

            GameServiceProvider.CurrentInstance.AddService<IPointsOfInterestService>(_poiService);
        }

        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();
        }

        [Test]
        public void Start_AddsPointsOfInterest()
        {
            _poiComponent.TestStart();

            Assert.AreEqual(_poiComponent.gameObject.transform.position, _poiService.AddedPointOfInterest);
        }

        [Test]
        public void OnDestroy_RemovesPointsOfInterest()
        {
            _poiComponent.TestOnDestroy();

            Assert.AreEqual(_poiComponent.gameObject.transform.position, _poiService.RemovedPointOfInterest);
        }
    }
}
