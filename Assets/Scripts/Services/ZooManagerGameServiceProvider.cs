// Sifaka Game Studios (C) 2017

using Assets.Scripts.Services.PointsOfInterest;

namespace Assets.Scripts.Services
{
    public class ZooManagerGameServiceProvider 
        : GameServiceProvider
    {
        protected override void Awake()
        {
            base.Awake();

            AddService<IPointsOfInterestService>(new PointsOfInterestService());
        }
    }
}
