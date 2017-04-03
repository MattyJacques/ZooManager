// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Characters;
using Assets.Scripts.Managers;
using Assets.Scripts.BehaviourTree;

namespace Assets.Scripts.Characters.Animals
{

  public class AnimalBase : AIBase
  {
    // Template values never get set, they represent the animal's permanent properties (max age, speed, etc)
    public AnimalTemplate Template { get; set; }

    [SerializeField]
    GameClock gameClock;

    public AnimalBase(AnimalTemplate template, GameObject model)
    { // Constructor to set up the template and behaviour tree and model
      Template = template;
      Model = model;
    } // AnimalBase()

    public AnimalBase(Assets.Scripts.Managers.AnimalManager.Animal animal)
    { // Constructor to set up the template and behaviour tree and model
      Template = animal.Template;
      Model = animal.Prefab;   
    } // AnimalBase()

    public void Init()
    { // Initializes the AIBase needs from template (WIP) and gets the animals enclosure
      Hunger = 0;
      Thirst = 100;
      Boredom = 100;
      Health = 100;
      Age = 0;
      Enclosure _enclosure = null;
      if (EnclosureUtilities.IsActiveEnclosure(base.Model.transform.position, ref _enclosure) == false)
        Debug.LogError("AnimalBase is not in a active enclosure!");
      Enclosure = _enclosure;
      Behave = BehaviourCreator.Instance.GetBehaviour("basicAnimal");

      pathfinder = Model.AddComponent<Mover>();
      if (pathfinder == null)
        Debug.LogError("pathfinder not assigned");

      CoroutineSys.Instance.StartCoroutine(Behave.Behave(this));
    } // Init()

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
  } // AnimalBase
} // namespace