using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BehaviourTree.Base;


namespace Assets.Scripts.BehaviourTree
{
  public class BehaviourCreator
  {
    private List<Assets.Scripts.BehaviourTree.Base.Behaviour> _behaviours;

    public void CreateBehaviours()
    { // Create all of the behaviours needed, storing them in the list

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


    private Sequence CreateMoveToTarget()
    { // Create the sequence of behaviours that will allow an object to
      // follow to path to a target

      BehaveComponent[] moveToTargetComp = new BehaveComponent[4];
      BehaveComponent[] pathCheckComp = new BehaveComponent[2];

      pathCheckComp[0] = new HasPath();
      pathCheckComp[1] = new GetPath();

      moveToTargetComp[0] = new HasTarget();
      moveToTargetComp[1] = new Selector(pathCheckComp);

      return new Sequence(moveToTargetComp);
      
    } // CreateMoveToTarget()

  } // BehaviourCreator
}
