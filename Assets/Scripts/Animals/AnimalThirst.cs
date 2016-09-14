using UnityEngine;
using System.Collections;

public class AnimalThirst {
    public float thirst;
    public Thirst needsWater;

	// Use this for initialization
    void Start () 
    {
        thirst = 100;
    } // Start()
	
	// Update is called once per frame
	void Update () {
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
  } // Update()
} // AnimalThirst

public enum Thirst 
{
    False,
    True,
    Drink
} // Thirst