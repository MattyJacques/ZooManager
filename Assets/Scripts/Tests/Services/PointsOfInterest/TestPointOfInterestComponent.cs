// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Services.PointsOfInterest;

namespace Assets.Scripts.Tests.Services.PointsOfInterest
{
    public class TestPointOfInterestComponent 
        : PointOfInterestComponent
    {
        public void TestStart ()
        {
		    Start();
        }

        public void TestOnDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
