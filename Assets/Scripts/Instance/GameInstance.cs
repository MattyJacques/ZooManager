// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;
using UnityEngine;

namespace Assets.Scripts.Instance
{
    public class GameInstance
        : MonoBehaviour
    {
        private static GameInstance _currentInstance;
        public static GameInstance CurrentInstance
        {
            get { return _currentInstance; }
            private set
            {
                if (_currentInstance == null || value == null)
                {
                    _currentInstance = value;
                }
                else
                {
                    Debug.LogError("Found existing GameInstance!");
                }
            }
        }

        private UnityMessageEventDispatcher _uiDispatcher;

        public static void ClearGameInstance()
        {
            CurrentInstance = null;
        }

        protected virtual void Awake()
        {
            CurrentInstance = this;
            _uiDispatcher = new UnityMessageEventDispatcher();
        }

        public UnityMessageEventDispatcher GetUIMessageDispatcher()
        {
            return _uiDispatcher;
        }
    }
}
