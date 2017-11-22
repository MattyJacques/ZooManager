// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using Assets.Scripts.Services.PointsOfInterest;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.PointsOfInterest
{
    [TestFixture]
    public class PointsOfInterestServiceTestFixture
    {
        [Test]
        public void GetRandomPointOfInterest_NoPoints_ErrorAndInvalidVector()
        {
		    LogAssert.Expect(LogType.Error, "No points of interest registered!");

            var receivedPoint = new PointsOfInterestService().GetRandomPointOfInterest();

            Assert.AreEqual(PointsOfInterestConstants.InvalidPointOfInterest, receivedPoint);
        }

        [Test]
        public void GetRandomPointOfInterest_Points_RetrievesOne()
        {
            var pointSelection = new List<Vector3> {new Vector3(1f, 2f, 3f), new Vector3(2f, 3f, 4f)};

            var poiService = new PointsOfInterestService();

            foreach (var currentPoint in pointSelection)
            {
                poiService.AddPointOfInterest(currentPoint);
            }

            var receivedPoint = poiService.GetRandomPointOfInterest();

            Assert.IsTrue(pointSelection.Contains(receivedPoint));
        }

        [Test]
        public void GetNearestPointOfInterest_NoPoints_ErrorAndInvalidVector()
        {
            LogAssert.Expect(LogType.Error, "No points of interest registered!");

            var receivedPoint = new PointsOfInterestService().GetNearestPointOfInterest(new Vector3());

            Assert.AreEqual(PointsOfInterestConstants.InvalidPointOfInterest, receivedPoint);
        }

        [Test]
        public void GetNearestPointOfInterest_Points_RetrievesNearestOne()
        {
            var nearPoint = new Vector3(1f, 2f, 3f);
            var farPoint = new Vector3(100f, 300f, 200f);

            var poiService = new PointsOfInterestService();

            poiService.AddPointOfInterest(nearPoint);
            poiService.AddPointOfInterest(farPoint);

            var receivedPoint = poiService.GetRandomPointOfInterest();

            Assert.AreEqual(nearPoint, receivedPoint);
        }

        [Test]
        public void AddPointOfInterest_AlreadyAdded_Error()
        {
            var expectedPoint = new Vector3(1f, 2f, 3f);
            LogAssert.Expect(LogType.Error, "Already contained point of interest " + expectedPoint);

            var poiService = new PointsOfInterestService();

            poiService.AddPointOfInterest(expectedPoint);
            poiService.AddPointOfInterest(expectedPoint);
        }

        [Test]
        public void RemovePointOfInterest_NotAlreadyAdded_Error()
        {
            var expectedPoint = new Vector3(1f, 2f, 3f);
            LogAssert.Expect(LogType.Error, "Could not find point of interest " + expectedPoint);

            new PointsOfInterestService().RemovePointOfInterest(expectedPoint);
        }

        [Test]
        public void RemovePointOfInterest_RemovesPointOfInterest()
        {
            var nearPoint = new Vector3(1f, 2f, 3f);
            var farPoint = new Vector3(100f, 300f, 200f);

            var poiService = new PointsOfInterestService();

            poiService.AddPointOfInterest(nearPoint);
            poiService.AddPointOfInterest(farPoint);

            poiService.RemovePointOfInterest(nearPoint);

            var receivedPoint = poiService.GetRandomPointOfInterest();

            Assert.AreEqual(farPoint, receivedPoint);
        }
    }
}
