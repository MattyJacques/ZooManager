// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Components.Needs;
using UnityEngine;

namespace Assets.Scripts.Tests.Components.Needs
{
    public class MockNeedsComponent 
        : MonoBehaviour
        , INeedsInterface
    { 
        public List<Need> Needs = new List<Need>();

        public void AddNeed(Need inNeed)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Need> GetNeeds()
        {
            return Needs;
        }
    }
}

#endif // UNITY_EDITOR
