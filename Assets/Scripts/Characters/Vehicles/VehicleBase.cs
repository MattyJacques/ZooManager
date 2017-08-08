// Title        : VehicleBase.cs
// Purpose      : Base class which all visitors which instantiate from
// Author       : Alexander Falk
// Date         : 11/11/2016

using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Characters;
using Assets.Scripts.BehaviourTree;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Characters.Vehicles
{

  public class VehicleBase : AIBase
  { 
    public string vehicleType { get; set; }
    public Vector3 destination { get; set; } 
    public float occupancy { get; set; }


    // Template values never get set, they represent the visitor's permanent properties (name, type, etc)
    public VehicleTemplate Template { get; set; }

    [SerializeField]
    GameClock gameClock;

    public VehicleBase(VehicleTemplate template,  GameObject model)
    { // Constructor to set up the template and behaviour tree
      Template = template;
      Model = model;
    } // VisitorBase()

    public VehicleBase(Assets.Scripts.Managers.VehicleManager.Vehicle vehicle)
    { // Constructor to set up the template and behaviour tree
      Template = vehicle.Template;
      Model = vehicle.Prefab;
    } // VisitorBase()

    public void Init()
    {
      // TODO: Pick vehicle type out of list of vehicles
      // TODO: Assign a fititng number of passengers to the given vehicle type
      // TODO: Have the game only select a given number of vehicles of the ones it spawns to actually go to the Zoo, The other's follow the road
      // TODO: Find the right destination - in this case, the nearest bus stop that has been placed by a player
      vehicleType = "Bus";
      destination = new Vector3(50,10,50);
      occupancy = 10;
      Behave = BehaviourCreator.Instance.GetBehaviour("basicVehicle");
      CoroutineSys.Instance.StartCoroutine(Behave.Behave(this));

    }

    protected void Update()
    { // Process the needs of the base then process the behaviour for AI

      float deltaTime = Time.deltaTime;
      //TODO: Move towards destination - this is either the parking lot/ bus stop or other end of the road
    }
  } // Update()
 
}


