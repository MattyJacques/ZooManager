// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Services.PointsOfInterest
{
    public static class PointsOfInterestConstants
    {
        public static readonly Vector3 InvalidPointOfInterest = Vector3.positiveInfinity;
    }

    public interface IPointsOfInterestService
    {
        Vector3 GetRandomPointOfInterest();
        Vector3 GetNearestPointOfInterest(Vector3 inLocation);

        void AddPointOfInterest(Vector3 inPointOfInterest);
        void RemovePointOfInterest(Vector3 inPointOfInterest);
    }
}
