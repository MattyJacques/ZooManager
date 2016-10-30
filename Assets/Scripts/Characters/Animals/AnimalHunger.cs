using UnityEngine;
using System.Collections;

public class AnimalHunger {
    public float hunger;
    public Hunger needsFood;

    // Use this for initialization
    void Start () 
    {
        hunger = 100;
    } // Start()

    // Update is called once per frame
    void Update () {
        if (hunger > 0) 
        {
            hunger--;
        }

        if (hunger <= 50)
        {
            needsFood = Hunger.True;
        }
        else 
        {
            needsFood = Hunger.False;
        }
        if (hunger == 0)
        {
            // Call Die Function
        }
    } // Update()
} // AnimalHunger

public enum Hunger
{
    False,
    True,
    Eat
} // Hunger