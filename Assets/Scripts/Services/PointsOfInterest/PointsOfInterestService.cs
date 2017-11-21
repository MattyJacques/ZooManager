// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.PointsOfInterest
{
    public class PointsOfInterestService : IPointsOfInterestService
    {
        private readonly List<Vector3> _pointsOfInterest = new List<Vector3>();

        // IPointsOfInterestService
        public Vector3 GetRandomPointOfInterest()
        {
            if (HasPointsOfInterest())
            {
                return _pointsOfInterest[Random.Range(0, _pointsOfInterest.Count - 1)];
            }

            Debug.LogError("No points of interest registered!");

            return Vector3.zero;
        }

        public Vector3 GetNearestPointOfInterest(Vector3 inLocation)
        {
            var currentNearest = Vector3.positiveInfinity;

            if (HasPointsOfInterest())
            {
                foreach (var pointOfInterest in _pointsOfInterest)
                {
                    if ((pointOfInterest - inLocation).sqrMagnitude < (currentNearest - inLocation).sqrMagnitude)
                    {
                        currentNearest = pointOfInterest;
                    }
                }

                return currentNearest;
            }

            Debug.LogError("No points of interest registered!");

            return currentNearest;
        }

        private bool HasPointsOfInterest()
        {
            return _pointsOfInterest.Count > 0;
        }

        public void AddPointOfInterest(Vector3 inPointOfInterest)
        {
            if (!_pointsOfInterest.Contains(inPointOfInterest))
            {
                _pointsOfInterest.Add(inPointOfInterest);
            }
            else
            {
                Debug.LogError("Already contained point of interest " + inPointOfInterest);
            }
        }

        public void RemovePointOfInterest(Vector3 inPointOfInterest)
        {
            if (!_pointsOfInterest.Remove(inPointOfInterest))
            {
                Debug.LogError("Could not find point of interest " + inPointOfInterest);
            }
        }
        // ~IPointsOfInterestService
    }
}
