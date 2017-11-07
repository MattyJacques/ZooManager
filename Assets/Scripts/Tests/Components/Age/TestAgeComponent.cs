// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Age;

namespace Assets.Scripts.Tests.Components.Age
{
    public class TestAgeComponent 
        : AgeComponent
    {
        private float _deltaTime;

        public void TestAwake()
        {
            Awake();
        }

        public void TestUpdate(float inDeltaTime)
        {
            _deltaTime = inDeltaTime;

            Update();
        }

        protected override float GetDeltaTime()
        {
            return _deltaTime;
        }
    }
}
