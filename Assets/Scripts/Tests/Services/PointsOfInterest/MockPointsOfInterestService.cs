﻿// Sifaka Game Studios (C) 2017

using Assets.Scripts.Services.PointsOfInterest;
using UnityEngine;

namespace Assets.Scripts.Tests.Services.PointsOfInterest
{
    public class MockPointsOfInterestService 
        : IPointsOfInterestService
    {
        public Vector3 GetRandomPointOfInterestResult = Vector3.zero;

        public Vector3 GetRandomPointOfInterest()
        {
            return GetRandomPointOfInterestResult;
        }

        public Vector3 GetNearestPointOfInterest(Vector3 inLocation)
        {
            throw new System.NotImplementedException();
        }

        public void AddPointOfInterest(Vector3 inPointOfInterest)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePointOfInterest(Vector3 inPointOfInterest)
        {
            throw new System.NotImplementedException();
        }
    }
}
