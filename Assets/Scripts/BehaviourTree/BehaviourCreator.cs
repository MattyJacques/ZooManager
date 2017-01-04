using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Characters;
using Assets.Scripts.Managers;


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
      animalComponents[0] = CreateAnimalThirst();
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
      findWaterComp[0] = new Action(SetTarget);       // Find nearest water source
      findWaterComp[1] = CreateMoveToTarget();
      findWaterComp[2] = new Action(DrinkWater);               // Refill thirst

      // Set thirst components
      thirstCheckComp[0] = new Conditional(IsThirsty);
      thirstCheckComp[1] = new Sequence(findWaterComp);

      // Create and return the finished thirst sequence
      return new Sequence(thirstCheckComp);

    } // CreateThirst()

    private Sequence CreateMoveToTarget()
    { // Create the sequence of behaviours that will allow an object to
      // follow to path to a target

      BehaveComponent[] moveToTargetComp = new BehaveComponent[4];
      BehaveComponent[] pathCheckComp = new BehaveComponent[2];

      pathCheckComp[0] = new Conditional(HasPath);
      pathCheckComp[1] = new Action(GetPath);

      moveToTargetComp[0] = new Conditional(HasTarget);
      moveToTargetComp[1] = new Selector(pathCheckComp);
      moveToTargetComp[2] = new Action(FollowPath);
      moveToTargetComp[3] = new Conditional(HasArrived);

      return new Sequence(moveToTargetComp);

    } // CreateMoveToTarget()

    private ReturnCode SetTarget(AIBase theBase)
    { // Calculate the cloest source matching next target type

      theBase.Target = BuildingManager.GetClosestOfType(theBase.Model.transform.position
                                                        , theBase.NextTarget);

      Debug.Log("SetTarget(), returning success");
      return ReturnCode.Success;
    } // SetCurrTarget()

    private ReturnCode DrinkWater(AIBase theBase)
    { // Handle the drinking of the water
      Debug.Log("DrinkWater(), returning success");
      theBase.Feed(AIBase.FeedType.Food, 100);
      return ReturnCode.Success;
    } // DrinkWater()

    private ReturnCode GetPath(AIBase theBase)
    { // Call to get the path to the current target
      Debug.Log("GetPath(), returning success");

      // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ AMBERITE
      // Insert call to get the path here
      // theBase.Target is a gameobject you can get the position of

      return ReturnCode.Success;
    } // GetPath()

    private ReturnCode FollowPath(AIBase theBase)
    { // Handle the following of the current path to the target
      Debug.Log("FollowPath(), returning success");

      // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ AMBERITE
      // Here we need something to check if we are still following that path
      // Not entirely sure how but if there is a method in the Seeker I think it was that we can check
      // of maybe have bool in AIBase that is like _FollowingPath that we can check for false 

      return ReturnCode.Success;
    } // FollowPath()

    private bool IsThirsty(AIBase theBase)
    { // Check if the animal base's thirst is at a level we class as thirsty
      // If so set the next target of the base to water and return true

      Debug.Log("IsThirsty(), returning " + (theBase.Thirst < 50));

      bool isThirst = theBase.Thirst < 50;

      if (isThirst)
      {
        theBase.NextTarget = BuildingManager.TargetType.Water;
      }

      return isThirst;
    } // CheckThirst()

    private bool HasPath(AIBase theBase)
    { // Check if the base has a set path already
      Debug.Log("HasPath(), returning " + (theBase.Path != null));
      return theBase.Path != null;
    } // HasPath()

    private bool HasTarget(AIBase theBase)
    { // Return true if the base currently has a target
      Debug.Log("HasTarget(), returning " + (theBase.Target != null));
      return theBase.Target != null;
    } // HasTarget()

    private bool HasArrived(AIBase theBase)
    { // Return true if we have arrived at target
      Debug.Log("HasArrived(), returning " + (theBase.Model.transform.position == theBase.Target.transform.position));

      // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ AMBERITE
      // Here check if we are near wherever we need to be, pick whatever radius seems best

      return theBase.Model.transform.position == theBase.Target.transform.position;
    } // HasArrived()

  } // BehaviourCreator
}
