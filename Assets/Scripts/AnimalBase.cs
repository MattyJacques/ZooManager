// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;

public class AnimalBase : MonoBehaviour
{
    //Protected member variables
    protected float health;
    protected float healthRate;
    protected float hunger;
    protected float hungerRate;
    protected float thirst;
    protected float thirstRate;
    protected float age;
    protected float ageMax;
    protected float boredom;

    //Public accessors
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float HealthRate
    {
        get { return healthRate; }
        set { healthRate = value; }
    }
    public float Hunger
    {
        get { return hunger; }
        set { hunger = value; }
    }
    public float HungerRate
    {
        get { return hungerRate; }
        set { hungerRate = value; }
    }
    public float Thirst
    {
        get { return thirst; }
        set { thirst = value; }
    }
    public float ThirstRate
    {
        get { return thirstRate; }
        set { thirstRate = value; }
    }
    public float Age
    {
        get { return age; }
    }
    public float Boredom
    {
        get { return boredom; }
        set { boredom = value; }
    }

    [SerializeField]
    GameClock gameClock;

    // Use this for initialization
    protected virtual void Start()
    {
	    
	}

    // Update is called once per frame
    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;
        if (health > 0)
        {
            //Handles thirst
            if (thirst > 0) { thirst -= thirstRate * deltaTime; }
            thirst = Mathf.Clamp(thirst, 0, 100);

            //Handles hunger
            if (hunger > 0) { hunger -= hungerRate * deltaTime; }
            hunger = Mathf.Clamp(hunger, 0, 100);

            //Handles health
            if (hunger > 50 && health > 50) { health += healthRate * deltaTime; }
            if (hunger == 0 || thirst == 0) { health -= healthRate * deltaTime; }

            //Handles age
            age += deltaTime;
            if (age < ageMax) { Cull(); }
        }
        else if (health < 0) { health = 0; }
        else if (health == 0)
        {
            //Do dying stuff
        }
	}

    public virtual void Feed(float food, float water)
    {
        hunger += food;
        thirst += water;
    }

    public virtual void Cull()
    {
        health = 0;
    }
}
