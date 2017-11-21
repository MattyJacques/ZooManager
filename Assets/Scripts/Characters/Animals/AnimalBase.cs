﻿// Title        : AnimalBase.cs
// Purpose      : Base class which all animals which instantiate from
// Author       : Dan Budworth-Mead
// Date         : 22/08/2016

using UnityEngine;
using Assets.Scripts.Behaviours;
using Assets.Scripts.UI;

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
      Behave = BehaviourCreator.Instance.GetBehaviour(BehaviourTreeType.BasicAnimal);

      CoroutineSys.Instance.StartCoroutine(Behave.Behave(Model));
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