// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;

public class AnimalBase
{
    // This reference can be used to move the animal around without needing to make AnimalBase a MonoBehaviour
    public Transform UnityTransform { get; set; }

    // Template values never get set, they represent the animal's permanent properties (max age, speed, etc)
    public AnimalTemplate Template { get; set; }

    // The animal's temporary properties change over the life of the animal
    public float Health { get; set; }
    public float Hunger { get; set; }
    public float Thirst { get; set; }
    public float Age { get; set; }
    public float Boredom { get; set; }

    [SerializeField]
    GameClock gameClock;

    public AnimalBase(AnimalTemplate template)
    {
        Template = template;
    }

    // Update is called once per frame
    protected void Update()
    {
        float deltaTime = Time.deltaTime;
        if (Health > 0)
        {
            //Handles thirst
            if (Thirst > 0) { Thirst -= Template.thirstRate * deltaTime; }
            Thirst = Mathf.Clamp(Thirst, 0, 100);

            //Handles hunger
            if (Hunger > 0) { Hunger -= Template.hungerRate * deltaTime; }
            Hunger = Mathf.Clamp(Hunger, 0, 100);

            //Handles health
            if (Hunger > 50 && Health > 50) { Health += Template.healthRate * deltaTime; }
            if (Hunger == 0 || Thirst == 0) { Health -= Template.healthRate * deltaTime; }

            //Handles age
            Age += deltaTime;
            if (Age < Template.lifespan) { Cull(); }
        }
        else if (Health < 0) { Health = 0; }
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
