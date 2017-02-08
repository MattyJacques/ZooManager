using UnityEngine;
using System.Collections;
using System.Threading;
using Assets.Scripts.Managers;
using Pathfinding;

//[RequireComponent(typeof(Seeker))]
//[RequireComponent(typeof(Pathfinding.Mover))]

namespace Assets.Scripts.Characters
{
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
    public Mover pathfinder { get; set; }

    // Behaviour object for AI
    public BehaviourTree.Base.Behaviour Behave { get; set; }


    // Thread variables
    public bool StopBehaviour { get; set; }
    private Thread _behaviourThread; // The behaviour tree thread

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

    public virtual void SetFoodTarget()
    { // Find the nearest suitable food and set the pahtfinding target to this
      pathfinder.Target = BuildingManager.GetClosestOfType(Model.transform.position, BuildingManager.TargetType.Food).transform; // Replace this with enclosure stuff
    } // SetFoodTarget()

    public virtual void AddFun(int amount)
    { // Increase the fun meter
      Boredom += amount;
    } // AddFun()

    public virtual void StartBehaviour()
    { // Start the Behaviour Tree thread

      StopBehaviour = false;
      _behaviourThread = new Thread(() => Behave.Behave(this)); // Create a new thread for behave method, lambda for parameter
      _behaviourThread.Start(); // Start the thread
      while(!_behaviourThread.IsAlive);   // Wait for the thread to be alive
      Debug.Log("Behaviour Thread started!");

    } // StartBehaviour()

    public virtual void AbortBehaviour()
    { // Stop the Behaviour Tree thread

      _behaviourThread.Abort();   // Tell the thread to stop (ungracefully)
      _behaviourThread.Join();    // Wait for the thread to stop
      Debug.Log("Behaviour Thread aborted!");

    } // AbortBehaviour()

    public virtual void Kill()
    { // Kill the character

      Health = 0;

    } // Kill()

  } // AIBase
} // namespace
    