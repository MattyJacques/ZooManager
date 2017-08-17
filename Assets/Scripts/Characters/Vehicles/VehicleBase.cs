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
    public string VehicleType { get; set; }
    public Vector3 Destination { get; set; } 
    public float Occupancy { get; set; }
    public string state { get; set; }

    // Template values never get set, they represent the visitor's permanent properties (name, type, etc)
    public VehicleTemplate Template { get; set; }

    [SerializeField]
    GameClock gameClock;

    public VehicleBase(VehicleTemplate template,  GameObject model, Vector3 destination, int occupancy)
    { // Constructor to set up the template and behaviour tree
      Template = template;
      Model = model;
      Destination = destination;
      Occupancy = occupancy;

    } // VisitorBase()

    public VehicleBase(Assets.Scripts.Managers.VehicleManager.Vehicle vehicle)
    { // Constructor to set up the template and behaviour tree
      Template = vehicle.Template;
      Model = vehicle.Prefab;
    } // VisitorBase()

    public void Init(int occupancy, string vehicleType)
    {
      // TODO: Pick vehicle type out of list of vehicles
      // TODO: Assign a fititng number of passengers to the given vehicle type
      // TODO: Have the game only select a given number of vehicles of the ones it spawns to actually go to the Zoo, The other's follow the road
      // TODO: Find the right destination - in this case, the nearest bus stop that has been placed by a player
      // TODO: Go to the nearest - empty? bus stop
      // TODO: Pick up people that are waiting at the bus stop
      VehicleType = vehicleType;
      Occupancy = occupancy;
      // TODO: randomly choose state here? or maybe sooner
      state = "Arriving";
      FindDestination();

      switch (VehicleType)
      {
        case "Bus":
          Behave = BehaviourCreator.Instance.GetBehaviour("bus");
          break;
        case "Car":
          Behave = BehaviourCreator.Instance.GetBehaviour("car");
          break;
        case "Van":
          Behave = BehaviourCreator.Instance.GetBehaviour("car");
          break;
        default:
          Behave = BehaviourCreator.Instance.GetBehaviour("basicVehicle");
          break;
      }

      Debug.Log(VehicleType);

      pathfinder = Model.AddComponent<Mover>();
      pathfinder.Target = Destination;


      CoroutineSys.Instance.StartCoroutine(Behave.Behave(this));

    }

    public void FindDestination()
    {
      switch (state)
      {
        case "Arriving": //The vehicle has spawned and is trying to get ot the Zoo
          if (GameObject.Find("BusStop"))
          {
            Destination = GameObject.Find("BusStop").transform.position;
          }
          else
          {
            Destination = new Vector3(70, Model.transform.position.y, 70);
            state = "Passing"; 
          }
          break;

        case "Leaving": //The vehicle is leaving the Zoo
          //TODO: Find roadexit, go there
          Destination = new Vector3(70, Model.transform.position.y, 70);
          break;
        
        case "Passing": //The vehicle is passing and not entering the zoo
          //TODO: Find opposite roadexit, go there
          Destination = new Vector3(70, Model.transform.position.y, 70);
          break;
        default:
          Destination = new Vector3(70, Model.transform.position.y, 70);
          break;
      }

      Debug.Log(Destination);
    }

    public void SpawnVisitors()
    {
      
      for (int i = 0; i < Occupancy; i++)
      {
        Debug.Log(Occupancy);
        GameObject.Find("Managers").GetComponent<VisitorManager>().CreateRandomVisitor(Model.transform.position);
        Occupancy--;
      }
    }

    public void Update()
    { // Process the needs of the base then process the behaviour for AI
      //TODO: maybe check an area around this? This basically checks whether the vehicle has arrived at the destination
      //      in order to unload the passgeners. Can't really use pathfinder.HasArrvied since that get's set to false
      //      when it arrives
      if (pathfinder.HasArrived == true && state == "Arriving")
      {
        Debug.Log("Bus arrived, spawning visitors");
        SpawnVisitors();
        state = "Leaving";
        Debug.Log("Finding new destination");
        FindDestination();
        pathfinder.Target = Destination;
      }
    }
  } // Update()
 
}
