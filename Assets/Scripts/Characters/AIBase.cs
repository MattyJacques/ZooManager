using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using Pathfinding;


namespace Assets.Scripts.Characters
{

  [RequireComponent(typeof(Seeker))]
  [RequireComponent(typeof(SimpleSmoothModifier))]
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
    public Transform Target { get; set; }                       // Target of current behaviour
    public Path path { get; set; }                              // Current path
    public int CurrentWaypoint { get; set; }                    // The current waypoint to move to
    public float RepathRate { get; set; }                       // After how much seconds the path should be recalculated
    public float LastRepath { get; set; }                       // The time when the last repath happened
    public bool HasArrived { get; set; }                        // Has the AI arrived to target
    public Seeker Pathfinder { get; set; }                      // The seeker script (pathfinder)
    public CharacterController Controller { get; set; }         // The character controller of the gameobject
    public float Speed { get; set; }                            // The speed of the AI

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
     //Model.AddComponent<Pathfinding.Mover>();
    } // Feed()

    public virtual void AddFun(int amount)
    { // Increase the fun meter
      Boredom += amount;
    } // AddFun()

    public virtual void CheckNeeds()
    { // Perform the behaviour for this base

      Behave.Behave(this);

    } // CheckNeeds()

    public virtual void Kill()
    { // Kill the character

      Health = 0;

    } // Kill()

    public virtual void InitPathfinding()
    { // Initializes all needed parts for pathfinding

        Pathfinder = Model.GetComponent<Seeker>();
        Controller = Model.GetComponent<CharacterController>();

        // Setup the path smoothing modifier
        SimpleSmoothModifier ssm = Model.GetComponent<SimpleSmoothModifier>();
        ssm.iterations = 2;
        ssm.maxSegmentLength = 2;
        ssm.strength = 0.5f;

    } // InitPathfinding()

    public virtual void Update()
    { // Update method, currently only used for path following

        // Repathing if repath time has been reached
        if(Time.time - LastRepath > RepathRate && Pathfinder.IsDone())
        {
            LastRepath = Time.time + Random.value * RepathRate * 0.5f;

            Pathfinder.StartPath(Model.transform.position, Target.position, OnPathComplete);
        }

        if(path != null && CurrentWaypoint < path.vectorPath.Count)
        { // We do have a path and are not at the end, now follow it

            Vector3 dir = (path.vectorPath[CurrentWaypoint] - Model.transform.position).normalized;
            dir *= Speed;

            Controller.SimpleMove(dir);

            if(Model.transform.position == path.vectorPath[CurrentWaypoint])
            { // Waypoint reached
                CurrentWaypoint++;
            }

        }
        else if(CurrentWaypoint == path.vectorPath.Count)
        { // We reached the end of the path

            CurrentWaypoint++; // So we don't go into the if anymore
            HasArrived = true; // We arrived at the target

        }
        
    } // Update()

    public virtual void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            CurrentWaypoint = 1; // 1 because 0 is the start point
        }
    }

  } // AIBase
} // namespace
