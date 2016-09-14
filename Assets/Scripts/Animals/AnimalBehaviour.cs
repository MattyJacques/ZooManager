using UnityEngine;
using System.Collections;

public class AnimalBehaviour {
    public float thirst;
    public float boredom;
    public float hunger;
    public Thirst needsWater;
    public Boredom needsFun;
    public Hunger needsFood;

    // Use this for initialization
    void Start () 
    {
        thirst = 100;

    } // Start()

    // Update is called once per frame
    void Update () 
    {
        if (thirst > 0) 
        {
            thirst--;
        }

        if (thirst <= 50)
        {
            needsWater = Thirst.True;
        }
        else 
        {
            needsWater = Thirst.False;
        }
        if (thirst == 0)
        {
            // Call Die Function
        }
        if (boredom > 0) 
        {
            boredom--;
        }

        if (boredom <= 50)
        {
            needsFun = Boredom.True;
        }
        else 
        {
            needsFun = Boredom.False;
        }
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

    /* public IEnumerator IncreaseThirst() 
  {
    yield return new WaitForSeconds(0.1f);
    if (thirst > 0) 
    {
      thirst--;
    }
    Debug.Log (thirst);
    StartCoroutine (IncreaseThirst ());
  } */
} // AnimalThirst

public enum Thirst 
{
    False,
    True,
    Drink
} // Thirst

public enum Boredom
{
    False,
    True,
    Play
} // Boredom

public enum Hunger
{
    False,
    True,
    Eat
} // Hunger

