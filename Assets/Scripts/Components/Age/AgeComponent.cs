// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;
using Assets.Scripts.GameSettings;
using UnityEngine;

namespace Assets.Scripts.Components.Age
{
    public class AgeComponent 
        : MonoBehaviour
        , AgeInterface
    {
        public string SettingsPath = GameSettingsConstants.GameSettingsPath;
        public int MaxAge = 700;

        private float SecondsPerDay { get; set; }
        private float SecondsPassed { get; set; }
        private int CurrentAge { get; set; }

        protected void Awake()
        {
            SecondsPerDay = ZooManagerGameSettingsFunctions.LoadSettings(SettingsPath).Get().SecondsPerDay;

            CurrentAge = 0;
            SecondsPassed = 0.0f;
        }

        protected void Update()
        {
            if (!HasPassedMaxAge())
            {
                var deltaTime = GetDeltaTime();

                SecondsPassed += deltaTime;

                if (SecondsPassed >= SecondsPerDay)
                {
                    CurrentAge++;

                    if (HasPassedMaxAge())
                    {
                        SendAgingCompleteEvent();
                    }

                    SecondsPassed = 0;
                }
            }
        }

        private void SendAgingCompleteEvent()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new AgingCompleteMessage());
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // AgeInterface
        public int GetCurrentAge()
        {
            return CurrentAge;
        }

        public bool HasPassedMaxAge()
        {
            return MaxAge < CurrentAge;
        }
        // ~AgeInterface
    }
}
