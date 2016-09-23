// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

namespace Assets.Scripts.Animals
{

  public class AnimalBase
  {

    public enum FeedType { Food, Water }     // Enum for feed() to tell which stat to increase

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
    public GameObject Target { get; set; }
    public GameObject Path { get; set; }      // Change to path object
    public BehaviourTree.Base.Behaviour Behave { get; set; }

    [SerializeField]
    GameClock gameClock;

    public AnimalBase(AnimalTemplate template)
    { // Constructor to set up the template and behaviour tree
      Template = template;
    } // AnimalBase()

    protected void Update()
    { // Process the needs of the base then process the behaviour for AI

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
    } // Update()

    public virtual void Feed(FeedType type, int amount)
    { // Increase the hunger or thirst meter, depending on type
      if (type == FeedType.Food)
      {
        Hunger += amount;
      }
      else
      {
        Thirst += amount;
      }
    } // Feed()

    public virtual void Cull()
    { // Kill the animal
      Health = 0;
    } // Cull()

	  public virtual void CheckNeeds() 
	  { // Perform the behaviour for this base
      Behave.Behave(this);

    //  if (Health < 50) 
		  //{
			 // Debug.Log ("feeling sick!");
		  //}
		  //if (Hunger < 50) 
		  //{
			 // Debug.Log ("feeling hungry!");
		  //}
		  //if (Thirst < 50) 
		  //{
			 // Debug.Log ("feeling thirsty!");
		  //}
		  //if (Boredom < 50) 
		  //{
			 // Debug.Log ("feeling bored!");
		  //}
	  } // CheckNeeds()
  }
}
