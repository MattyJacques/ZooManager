// Title        : VehicleBase.cs
// Purpose      : Base class which all visitors which instantiate from
// Author       : Alexander Falk
// Date         : 11/11/2016

using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Characters.Visitors
{

  public class VehicleBase : AIBase
  { 
    // Template values never get set, they represent the visitor's permanent properties (name, type, etc)
    public VisitorTemplate Template { get; set; }

    [SerializeField]
    GameClock gameClock;

    public VehicleBase(VisitorTemplate template,  GameObject model)
    { // Constructor to set up the template and behaviour tree
      Template = template;
      Model = model;
    } // VisitorBase()

    public void init()
    {
      // TODO: PIck vehicle type out of list of vehicles
      // TODO: Assign a fititng number of passengers to the given vehicle type
      // TODO: Have the game only select a given number of vehicles of the ones it spawns to actually go to the Zoo, The other's follow the road
      vehicleType = "Bus";
      occupancy = 10;
      Behave = behaviourCreator.Instance.GetBehaviour("basicVehicle");
    }

    protected void Update()
    { // Process the needs of the base then process the behaviour for AI

      float deltaTime = Time.deltaTime;
      //TODO: Move towards destination - this is either the parking lot/ bus stop or other end of the road
      }
    } // Update()

    

  }
}
