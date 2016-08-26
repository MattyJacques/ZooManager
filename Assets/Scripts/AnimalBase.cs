// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class AnimalBase : MonoBehaviour
    {
        //Protected member variables
        protected float Health { get; set; }
        protected float HealthRate { get; set; }
        protected float Hunger { get; set; }
        protected float HungerRate { get; set; }
        protected float Thirst { get; set; }
        protected float ThirstRate { get; set; }
        protected float Age { get; set; }
        protected float AgeMax { get; set; }
        protected float Boredom { get; set; }

        [SerializeField] private GameClock _gameClock;

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            float deltaTime = Time.deltaTime;
            if (Health > 0)
            {
                //Handles thirst
                if (Thirst > 0)
                {
                    Thirst -= ThirstRate*deltaTime;
                }
                Thirst = Mathf.Clamp(Thirst, 0, 100);

                //Handles hunger
                if (Hunger > 0)
                {
                    Hunger -= HungerRate*deltaTime;
                }
                Hunger = Mathf.Clamp(Hunger, 0, 100);

                //Handles health
                if (Hunger > 50 && Health > 50)
                {
                    Health += HealthRate*deltaTime;
                }
                if (Hunger == 0 || Thirst == 0)
                {
                    Health -= HealthRate*deltaTime;
                }

                //Handles age
                Age += deltaTime;
                if (Age < AgeMax)
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
                // TODO: Do dying stuff
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