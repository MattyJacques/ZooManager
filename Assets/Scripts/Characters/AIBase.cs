﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;


namespace Assets.Scripts.Characters
{

  //[RequireComponent(typeof(Seeker))]
  //[RequireComponent(typeof(Pathfinding.Mover))]
  public class AIBase
  {
    public enum FeedType { Food, Water }          // Enum for feed() to tell which stat to increase

    // The animal's temporary properties change over the life of the animal
    public float Health { get; set; }    // How healthy the animal is, low is dying
    public float Hunger { get; set; }    // How hungry the animal is, low is hungry
    public float Thirst { get; set; }    // How thirsty the animal is, low is thirsty
    public float Boredom { get; set; }   // How bored the animal is, low is bored
    public float Age { get; set; }       // Animal Age, increases with time

    public GameObject Model { get; set; } // Model of the object, used to render the object

    // Target / Path members
    public BuildingManager.TargetType NextTarget { get; set; }  // Type of target, example: food or water
    public Transform Target { get; set; }                      // Target of current behaviour
    //public GameObject Path { get; set; }                        // Path to current behaviour
    public bool HasArrived { get; set; }
    public Pathfinding.Mover pathfinder { get; set; }

    // Behaviour object for AI
    public BehaviourTree.Base.Behaviour Behave { get; set; }

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
     // Pathfinding.Mover mover = new Pathfinding.Mover(ref this);
     Model.AddComponent<Pathfinding.Mover>();
    } // Feed()

    public virtual void CheckNeeds()
    { // Perform the behaviour for this base

      Behave.Behave(this);

    } // CheckNeeds()

    public virtual void Kill()
    { // Kill the character

      Health = 0;

    } // Kill()

  } // AIBase
} // namespace
