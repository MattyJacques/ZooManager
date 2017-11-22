// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Services.PointsOfInterest
{
    public class PointOfInterestComponent 
        : MonoBehaviour
    {
        protected void Start ()
        {
            GameServiceProvider.CurrentInstance.GetService<IPointsOfInterestService>().AddPointOfInterest(gameObject.transform.position);		
        }

        protected void OnDestroy()
        {
            GameServiceProvider.CurrentInstance.GetService<IPointsOfInterestService>().RemovePointOfInterest(gameObject.transform.position);
        }
    }
}
