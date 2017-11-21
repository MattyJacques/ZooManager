// Sifaka Game Studios (C) 2017

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class GameServiceProvider 
        : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _gameServices = new Dictionary<Type, object>();

        private static GameServiceProvider _currentInstance;
        public static GameServiceProvider CurrentInstance
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
                    Debug.LogError("Found existing instance of GameServiceProvider!");
                }
            } 
        }

        public static void ClearGameServiceProvider()
        {
            CurrentInstance = null;
        }

        protected void Awake()
        {
            CurrentInstance = this;
        }

        public void AddService<TServiceType>(TServiceType inService)
        {
            _gameServices.Add(typeof(TServiceType), inService);   
        }

        public TServiceType GetService<TServiceType>()
        {
            var serviceType = typeof(TServiceType);
            if (_gameServices.ContainsKey(serviceType))
            {
                return (TServiceType)_gameServices[serviceType];
            }
            else
            {
                Debug.LogError("Could not find service of type" + serviceType);
                return default(TServiceType);
            }
        }
    }
}
