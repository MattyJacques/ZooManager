// Sifaka Game Studios (C) 2017

using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Editor.UnitTests.Characters
{
    public class AIBaseBuilder
    {
        private readonly AIBase _base = new AIBase();

        public void WithHealth(float inHealth)
        {
            _base.Health = inHealth;
        }

        public void WithHunger(float inHunger)
        {
            _base.Hunger = inHunger;
        }

        public void WithThirst(float inThirst)
        {
            _base.Thirst = inThirst;
        }

        public void WithBoredom(float inBoredom)
        {
            _base.Boredom = inBoredom;
        }

        public void WithAge(float inAge)
        {
            _base.Age = inAge;
        }

        public void WithGameObject(GameObject inGameObject)
        {
            _base.Model = inGameObject;
        }

        public AIBase Build()
        {
            return _base;
        }
    }
}
