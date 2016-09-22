using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class BehaviourCreator
  {
    private List<Base.Behaviour> _behaviours;


    public Base.Behaviour GetBehaviour(int index)
    { // Return the behaviour at the given index

      return _behaviours[index];
    } // GetBehaviour()


    public void CreateBehaviours()
    { // Create all of the behaviours needed, storing them in the list

      _behaviours = new List<Base.Behaviour>();

      BehaveComponent[] animalComponents = new BehaveComponent[1];
      animalComponents[0] = CreateAnimalThirst2();
      Selector animalSelector = new Selector(animalComponents);
      _behaviours.Add(new Base.Behaviour(animalSelector));

    } // CreateBehaviours()


    private Sequence CreateAnimalThirst()
    { // Creates a sequence that will allow an animal object to check
      // if thirsty, if so will find the nearest trough object and go
      // drink

      // Create Arrays to hold sequence components
      BehaveComponent[] findWaterComp = new BehaveComponent[3];
      BehaveComponent[] thirstCheckComp = new BehaveComponent[2];

      // Set FindWater components
      findWaterComp[0] = new SetTarget();       // Find nearest water source
      findWaterComp[1] = CreateMoveToTarget();
      findWaterComp[2] = new Drink();               // Refill thirst

      // Set thirst components
      thirstCheckComp[0] = new IsThirsty();
      thirstCheckComp[1] = new Sequence(findWaterComp);

      // Create and return the finished thirst sequence
      return new Sequence(thirstCheckComp);

    } // CreateThirst()

    private Sequence CreateAnimalThirst2()
    { // Creates a sequence that will allow an animal object to check
      // if thirsty, if so will find the nearest trough object and go
      // drink

      // Create Arrays to hold sequence components
      BehaveComponent[] findWaterComp = new BehaveComponent[3];
      BehaveComponent[] thirstCheckComp = new BehaveComponent[2];

      // Set FindWater components
      findWaterComp[0] = new Action(SetCurrTarget);       // Find nearest water source
      findWaterComp[1] = CreateMoveToTarget2();
      findWaterComp[2] = new Action(DrinkWater);               // Refill thirst

      // Set thirst components
      thirstCheckComp[0] = new Conditional(CheckThirst);
      thirstCheckComp[1] = new Sequence(findWaterComp);

      // Create and return the finished thirst sequence
      return new Sequence(thirstCheckComp);

    } // CreateThirst()

    private Sequence CreateMoveToTarget()
    { // Create the sequence of behaviours that will allow an object to
      // follow to path to a target

      BehaveComponent[] moveToTargetComp = new BehaveComponent[4];
      BehaveComponent[] pathCheckComp = new BehaveComponent[2];

      pathCheckComp[0] = new HasPath();
      pathCheckComp[1] = new GetPath();

      moveToTargetComp[0] = new HasTarget();
      moveToTargetComp[1] = new Selector(pathCheckComp);
      moveToTargetComp[2] = new FollowPath();
      moveToTargetComp[3] = new HasArrived();

      return new Sequence(moveToTargetComp);
      
    } // CreateMoveToTarget()

    private Sequence CreateMoveToTarget2()
    { // Create the sequence of behaviours that will allow an object to
      // follow to path to a target

      BehaveComponent[] moveToTargetComp = new BehaveComponent[4];
      BehaveComponent[] pathCheckComp = new BehaveComponent[2];

      pathCheckComp[0] = new Conditional(HasCurrPath);
      pathCheckComp[1] = new Action(GetPathToTarget);

      moveToTargetComp[0] = new Conditional(HasCurrTarget);
      moveToTargetComp[1] = new Selector(pathCheckComp);
      moveToTargetComp[2] = new Action(FollowPathToTarget);
      moveToTargetComp[3] = new Conditional(HasArrivedAtTarget);

      return new Sequence(moveToTargetComp);

    } // CreateMoveToTarget()

    private ReturnCode SetCurrTarget(AnimalBase theBase)
    { // Calculate the cloest source of water

      return ReturnCode.Success;
    } // SetCurrTarget()

    private ReturnCode DrinkWater(AnimalBase theBase)
    { // Handle the drinking of the water

      theBase.Thirst = 100;
      return ReturnCode.Success;
    } // DrinkWater()

    private ReturnCode GetPathToTarget(AnimalBase theBase)
    { // Call to get the path to the current target
      return ReturnCode.Success;
    } // GetPath()

    private ReturnCode FollowPathToTarget(AnimalBase theBase)
    { // Handle the following of the current path to the target
      return ReturnCode.Success;
    } // FollowPath()

    private bool CheckThirst(AnimalBase theBase)
    { // Check if the animal base's thirst is at a level we class as thirsty
      return theBase.Thirst > 50;
    } // CheckThirst()

    private bool HasCurrPath(AnimalBase theBase)
    { // Check if the base has a set path already
      return theBase.Path != null;
    } // HasPath()

    private bool HasCurrTarget(AnimalBase theBase)
    { // Return true if the base currently has a target
      return theBase.Path != null;
    } // HasTarget()

    private bool HasArrivedAtTarget(AnimalBase theBase)
    { // Return true if we have arrived at target
      return theBase.UnityTransform.position == theBase.Target.position;
    } // HasArrived()

  } // BehaviourCreator
}
