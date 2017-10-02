// Title        : VisitorBase.cs
// Purpose      : Base class which all visitors which instantiate from
// Author       : Christos Alatzidis & Alexander Falk
// Date         : 11/11/2016

using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Characters;
using Assets.Scripts.BehaviourTree;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Characters.Visitors
{

  public class VisitorBase : AIBase
  { 
    // Template values never get set, they represent the visitor's permanent properties (name, type, etc)
    public VisitorTemplate Template { get; set; }

    public string favouriteAnimal { get; set; }

    [SerializeField]
    GameClock gameClock;

    public VisitorBase(VisitorTemplate template,  GameObject model)
    { // Constructor to set up the template and behaviour tree
      Template = template;
	    Model = model;
    } // VisitorBase()

    public VisitorBase(Assets.Scripts.Managers.VisitorManager.Visitor visitor)
    {
      Template = visitor.Template;
      Model = visitor.Prefab;
    }

    public void init()
    {
      Hunger = Random.Range(0,100);
      Thirst = Random.Range(0,100);
      Boredom = Random.Range(0,100);
      Age = Random.Range(1,100);

      AnimalManager animalManager = GameObject.Find("Managers").GetComponent<AnimalManager>();

      int random = Random.Range(0,animalManager._animalCollection.Count);
      favouriteAnimal = animalManager._animalCollection[random].ID;
      // TODO: Select 3 "favourite" animals out of the game's animal list

      pathfinder = Model.AddComponent<Mover>();
      pathfinder.NeighbourDist = 2f;

      if (GameObject.Find("TicketBooth"))
      {
        Transform ticketBooth = GameObject.Find("TicketBooth").transform;
        pathfinder.Target = ticketBooth.position + 3*ticketBooth.forward;
      }
      else
      {
        pathfinder.Target = new Vector3(80, 10, 80);//GameObject.Find("Busstop").transform.position;
      }

      Behave = BehaviourCreator.Instance.GetBehaviour("basicVisitor");

      CoroutineSys.Instance.StartCoroutine(Behave.Behave(this));
    }

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
        if (Age < Template.lifespan) { Kill(); }
      }
      else if (Health < 0) { Health = 0; }
      else if (Health == 0)
      {
        //Do dying stuff
      }
    } // Update()



    

  }
}
