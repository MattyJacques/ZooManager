using UnityEngine;
using System.Collections;

public class AnimalFun {
    public float boredom;
    public Boredom needsFun;

    // Use this for initialization
    void Start () 
    {
        boredom = 100;
    } // Start()

    // Update is called once per frame
    void Update () {
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
    } // Update()
} // AnimalFun

public enum Boredom
{
    False,
    True,
    Play
} // Boredom
