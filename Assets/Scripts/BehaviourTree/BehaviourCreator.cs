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
      
      // Animal Behaviour (index 0)
      BehaveComponent[] animalComponents = new BehaveComponent[1];
      animalComponents[0] = CreateAnimalHunger();
      Selector animalSelector = new Selector(animalComponents);
      _behaviours.Add(new Base.Behaviour(animalSelector));

    } // CreateBehaviours()

    #region Sequences

    private Sequence CreateAnimalHunger()
    { // Creates a sequence that will allow an animal object to check
      // if hungry, if so will find the nearest suitable food object
      // and go eat it

      // Create Array to hold sequence components
      BehaveComponent[] animalHunger = new BehaveComponent[6];

      // Set components
      animalHunger[0] = new Conditional(IsHungry);    // Check if animal is hungry
      animalHunger[1] = new Conditional(HasMyFood);   // Check if enclosure has animals preferred food
      animalHunger[2] = new Action(GetFood);          // Get the position of the food
      animalHunger[3] = new Action(MoveToTarget);     // Move to target
      animalHunger[4] = new Action(EatFood);          // Eat food
      animalHunger[5] = new ForceFailure();           // Force Sequence to fail, thus Selector will run next branch

      // Create and return the finished hunger sequence
      return new Sequence(animalHunger);
        
    } // CreateAnimalHunger()

    #endregion

    #region Actions

    private IEnumerator EatFood(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Handle the eating of food
        Debug.Log("EatFood(), returning success");
        theBase.Feed(AIBase.FeedType.Food, 100);
        returnCode(ReturnCode.Success);
        yield break;
    } // EatFood()

    private IEnumerator GetFood(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Get food target
        Debug.Log("GetFood(), returning success");
        theBase.SetFoodTarget();
        returnCode(ReturnCode.Success);
        yield break;
    } // GetFood()

    #endregion

    #region Conditionals

    private IEnumerator IsHungry(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the animal base's hunger is at a level we class as hungry
        // If so set the next target of the base to suitable food and return true

        Debug.Log("IsHungry(), returning " + (theBase.Hunger < 50));

        bool isHunger = theBase.Hunger < 50;

        if(isHunger)
        {
            theBase.NextTarget = BuildingManager.TargetType.Food; // TODO: get a suitable food for animal... might be enclosure code stuff
        }

        conditionResult(isHunger);
        yield break;
    } // IsHungry()

    private IEnumerator HasMyFood(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the enclosure has the preffered food type

        Debug.Log("HasMyFood(), returning true");
        
        conditionResult(true);
        yield break;
    } // HasMyFood()


    #endregion

    #region GeneralActions

    private IEnumerator MoveToTarget(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Move to the target

        /*theBase.pathfinder.CanMove = true;  // Start movement

        while(theBase.pathfinder.HasArrived == false)  // Wait for reaching target
            yield return null; // yield coroutine

        theBase.pathfinder.CanMove = false; // Prevent further movement

        Debug.Log("MoveToTarget(), returning success");*/
        
        returnCode(ReturnCode.Success);
        yield break;
    } // MoveToTarget()

    #endregion

  } // BehaviourCreator
}