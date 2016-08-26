// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Animals
{
    public class AnimalBase : MonoBehaviour
    {
        //Protected member variables
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<string> Food { get; set; }
        public List<string> Fun { get; set; }
        public List<string> Tags { get; set; }
        public float Lifespan { get; set; }
        public float Age { get; set; }
        public float Health { get; set; }
        public float HealthRate { get; set; }
        public float Hunger { get; set; }
        public float HungerRate { get; set; }
        public float Thirst { get; set; }
        public float ThirstRate { get; set; }
        public float Boredom { get; set; }

        [SerializeField] public GameClock gameClock;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;
            if (Health > 0)
            {
                //Handles Thirst
                if (Thirst > 0)
                {
                    Thirst -= ThirstRate*deltaTime;
                }
                Thirst = Mathf.Clamp(Thirst, 0, 100);

                //Handles Hunger
                if (Hunger > 0)
                {
                    Hunger -= HungerRate*deltaTime;
                }
                Hunger = Mathf.Clamp(Hunger, 0, 100);

                //Handles Health
                if (Hunger > 50 && Health > 50)
                {
                    Health += HealthRate*deltaTime;
                }
                if (Hunger == 0 || Thirst == 0)
                {
                    Health -= HealthRate*deltaTime;
                }

                //Handles Age
                Age += deltaTime;
                if (Age < Lifespan)
                {
                    Cull();
                }
            }
            else if (Health < 0)
            {
                Health = 0;
            }
            else if (Health == 0)
            {
                //Do dying stuff
            }
        }

        public virtual void Feed(float food, float water)
        {
            Hunger += food;
            Thirst += water;
        }

        public virtual void Cull()
        {
            Health = 0;
        }
    }
}
