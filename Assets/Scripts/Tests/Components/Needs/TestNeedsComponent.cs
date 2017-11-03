// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.Needs;

namespace Assets.Scripts.Tests.Components.Needs
{
    public class TestNeedsComponent 
        : NeedsComponent
    {
        private float _deltaTime = 0.0f;

        public void TestStart()
        {
            Start();
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

#endif
