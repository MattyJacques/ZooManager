// Title        : GameClock.cs
// Purpose      : Controls the games relative clock and the day / night cycle. Add this script to the directional light (sun).
// Author       : Dan Budworth-Mead
// Date         : 20/08/2016

using UnityEngine;

namespace Assets.Scripts
{
    public class GameClock : MonoBehaviour
    {
        public float TimeMultiplier { get; set; }
        public int Minute { get; private set; }
        public int Hour { get; private set; }

        public float _convertedDeltaTime { get; private set; }
        private float _currentTimeSeconds;
        
        
        // Use this for initialization
        void Start()
        {
            transform.position = Vector3.zero;
            transform.rotation = new Quaternion(0, 0, 180, 1);
            _currentTimeSeconds = PlayerPrefs.GetFloat("currentTimeSeconds");
            TimeMultiplier = 1.0f;
            transform.Rotate(new Vector3(-_currentTimeSeconds / 240, 0, 0));
        }

        // Update is called once per frame
        void Update()
        {
            _convertedDeltaTime = Time.deltaTime / 0.0416666667f * TimeMultiplier;
            _currentTimeSeconds += _convertedDeltaTime;

            Minute = (int)(_currentTimeSeconds / 60);
            Hour = Minute / 24;

            transform.Rotate(new Vector3(-_convertedDeltaTime / 240, 0, 0));
        }

        void OnDestroy()
        {
            PlayerPrefs.SetFloat("currentTimeSeconds", _currentTimeSeconds);
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetFloat("currentTimeSeconds", _currentTimeSeconds);
        }
    }
}