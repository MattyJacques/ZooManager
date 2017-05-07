using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Characters;
using Assets.Scripts.Managers;
using Assets.Scripts.Characters.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class BehaviourCreator
  {
    private Dictionary<string, Base.Behaviour> _behaviours;

    private static BehaviourCreator _instance;

    public static BehaviourCreator Instance 
    {
      get 
      { 
        if(_instance == null)
        {
          _instance = new BehaviourCreator();
          _instance.CreateBehaviours();
        }

        return _instance; 
      }
    }

    public Base.Behaviour GetBehaviour(string name)
    { // Return the behaviour with the given name

      if(_behaviours.ContainsKey(name))
      {
          Debug.Log("Behaviour \"" + name + "\" found");
          return _behaviours[name];
      }
      else
      {
          Debug.Log("Behaviour \"" + name + "\" not found");
          return null;
      }

    } // GetBehaviour()

    private void CreateBehaviours()
    { // Create all of the behaviours needed, storing them in the list

      _behaviours = new Dictionary<string, Base.Behaviour>();
      
      // Animal Behaviour (basicAnimal)
      BehaveComponent[] animalComponents = new BehaveComponent[4];
      animalComponents[0] = CreateAnimalHunger();
      animalComponents[1] = CreateAnimalThirst();
      animalComponents[2] = CreateAnimalFun();
      animalComponents[3] = CreateAnimalWanderRandom();
      Selector animalSelector = new Selector(animalComponents);
      _behaviours.Add("basicAnimal", new Base.Behaviour(animalSelector));

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


    private Sequence CreateAnimalThirst()
    { // Creates a sequence that will allow an animal object to check
      // if thirsty, if so will find the nearest water source
      // and go drink out of it

      // Create Array to hold sequence components
      BehaveComponent[] animalThirst = new BehaveComponent[6];

      // Set components
      animalThirst[0] = new Conditional(IsThirsty);    // Check if animal is thirsty
      animalThirst[1] = new Conditional(HasWater);   // Check if enclosure has a water source
      animalThirst[2] = new Action(GetWater);          // Get the position of the water source
      animalThirst[3] = new Action(MoveToTarget);     // Move to target
      animalThirst[4] = new Action(DrinkWater);          // Drink water
      animalThirst[5] = new ForceFailure();           // Force Sequence to fail, thus Selector will run next branch

      // Create and return the finished thirst sequence
      return new Sequence(animalThirst);

    } // CreateAnimalThirst()

    private Sequence CreateAnimalFun()
    { // Creates a sequence that will allow an animal object to check
      // if bored, if so will find the nearest fun source
      // and go play with it

      // Create Array to hold sequence components
      BehaveComponent[] animalFun = new BehaveComponent[6];

      // Set components
      animalFun[0] = new Conditional(IsBored);    // Check if animal is bored
      animalFun[1] = new Conditional(HasFun);   // Check if enclosure has a fund source
      animalFun[2] = new Action(GetFun);          // Get the position of the fun source
      animalFun[3] = new Action(MoveToTarget);     // Move to target
      animalFun[4] = new Action(HaveFun);          // Have fun
      animalFun[5] = new ForceFailure();           // Force Sequence to fail, thus Selector will run next branch

      // Create and return the finished thirst sequence
      return new Sequence(animalFun);

    } // CreateAnimalThirst()

    private Sequence CreateAnimalWander() // UNFINISHED AND FOR TESTING PURPOSES ONLY
    { // Creates a sequence that will allow an animal to wander to
      // a random interest point in his enclosure

      BehaveComponent[] animalWander = new BehaveComponent[2];

      animalWander[0] = new Action(GetRandomInterestPoint);
      animalWander[1] = new Action(MoveToTarget);

      return new Sequence(animalWander);
      
    } // CreateAnimalWander()

    private Sequence CreateAnimalWanderRandom()
    { // Creates a sequence that will allow an animal to wander to a random point inside
      // his enclosure

      BehaveComponent[] animalWander = new BehaveComponent[2];

      animalWander[0] = new Action(GetRandomPointInsideEnclosure);
      animalWander[1] = new Action(MoveToTarget);

      return new Sequence(animalWander);

    } // CreateAnimalWanderRandom()

    #endregion

    #region Actions

    private IEnumerator EatFood(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Handle the eating of food
      Debug.Log("EatFood(), returning success");

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food);

      theBase.Enclosure.RemoveInteriorItem(item.gameObject);
      MonoBehaviour.Destroy(item.gameObject);

      theBase.Feed(AIBase.FeedType.Food, 100);
      returnCode(ReturnCode.Success);
      yield break;
    } // EatFood()

    private IEnumerator GetFood(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Get food target

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food);

      if (item != null)
      {
        Debug.Log("GetFood(), returning success");
        theBase.pathfinder.Target = item.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetFood(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;
    } // GetFood()

    private IEnumerator DrinkWater(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Handle the drinking of water
        Debug.Log("DrinkWater(), returning success");
        theBase.Feed(AIBase.FeedType.Water, 100);
        returnCode(ReturnCode.Success);
        yield break;
    } // DrinkWater()

    private IEnumerator GetWater(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Get water target

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Water);

      if (item != null)
      {
        Debug.Log("GetWater(), returning success");
        theBase.pathfinder.Target = item.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetWater(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;      
    }

    private IEnumerator HaveFun(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Handle the playing with fun object
      Debug.Log("HaveFun(), returning success");
      theBase.AddFun(100);
      returnCode(ReturnCode.Success);
      yield break;      
    } // HaveFun()

    private IEnumerator GetFun(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Get Fun target

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Fun);

      if (item != null)
      {
        Debug.Log("GetFun(), returning success");
        theBase.pathfinder.Target = item.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetFun(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;      
    } // GetFun()

    #endregion

    #region Conditionals

    private IEnumerator IsHungry(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the animal base's hunger is at a level we class as hungry
      // If so set the next target of the base to suitable food and return true

      Debug.Log("IsHungry(), returning " + (theBase.Hunger < 50));

      bool isHunger = theBase.Hunger < 50;

      if(isHunger)
      {
        theBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Food;
      }

      conditionResult(isHunger);
      yield break;
    } // IsHungry()

    private IEnumerator IsThirsty(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the animal base's thirst is at a level we class as thirsty
      // If so set the next target of the base to water and return true
      Debug.Log("IsThirsty(), returning " + (theBase.Thirst < 50));

      bool isThirst = theBase.Thirst < 50;

      if(isThirst)
      {
        theBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Water;
      }

      conditionResult(isThirst);
      yield break;
    } // IsThirsty()

    private IEnumerator IsBored(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the animal base's boredom is at a level we class as bored
      // If so set the next target of the base to fun and return true
      Debug.Log("IsBored(), returning " + (theBase.Boredom < 50));

      bool isBore = theBase.Boredom < 50;

      if(isBore)
      {
        theBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Fun;
      }

      conditionResult(isBore);
      yield break;      
    } // IsBored()

    private IEnumerator HasMyFood(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the enclosure has the preffered food type
      
      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food);

      if (item != null)
      {
        Debug.Log("HasMyFood(), returning true");
        conditionResult(true);
      }
      else
      {
        Debug.Log("HasMyFood(), returning false");
        conditionResult(false);
      }

      yield break;
    } // HasMyFood()

    private IEnumerator HasWater(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the enclosure has a water source

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Water);

      if (item != null)
      {
        Debug.Log("HasWater(), returning true");
        conditionResult(true);
      }
      else
      {
        Debug.Log("HasWater(), returning false");
        conditionResult(false);
      }

      yield break;
    } // HasWater()

    private IEnumerator HasFun(AIBase theBase, System.Action<bool> conditionResult)
    { // Check if the enclosure has a fun source

      Transform item = theBase.Enclosure.GetClosestInteriorItemTransform(theBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Fun);

      if (item != null)
      {
        Debug.Log("HasFun(), returning true");
        conditionResult(true);
      }
      else
      {
        Debug.Log("HasFun(), returning false");
        conditionResult(false);
      }

      yield break;
    } // HasFun()

    #endregion

    #region GeneralActions

    private IEnumerator MoveToTarget(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Move to the target

      Debug.Log("Starting MoveToTarget()");

        theBase.pathfinder.HasArrived = false;
        theBase.pathfinder.CanMove = true;  // Start movement
        theBase.pathfinder.CanSearch = true;

        while(theBase.pathfinder.HasArrived == false)  // Wait for reaching target
            yield return null; // yield coroutine

        theBase.pathfinder.CanMove = false; // Prevent further movement
        theBase.pathfinder.CanSearch = false;
        theBase.pathfinder.HasArrived = false;

        Debug.Log("MoveToTarget(), returning success");
        
        returnCode(ReturnCode.Success);
        yield break;
    } // MoveToTarget()

    #endregion

    #region GeneralVisitorActions

    private IEnumerator GetRandomInterestPoint(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Set's the AIBase pathfinder target to a random interest
      // point gotten from the IPManager
      Debug.Log("Getting random Interest Point");

      Vector3 ipvec = IPManager.Instance.GetRandomIP();

      if(ipvec == Vector3.zero)
      { // no interest points
        returnCode(ReturnCode.Failure);
        yield break;
      }

      theBase.pathfinder.Target = ipvec;
      theBase.pathfinder.CanSearch = true;

      returnCode(ReturnCode.Success);
      yield break;
    } // GetRandomInterestPoint()

    #endregion

    #region GeneralAnimalActions

    private IEnumerator GetRandomPointInsideEnclosure(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Set's the AIBase pathfinder target to a random point
      // inside the enclosure

      Debug.Log("Getting random point inside Enclosure");

      theBase.pathfinder.Target = theBase.Enclosure.GetRandomPointOnTheGround();

      returnCode(ReturnCode.Success);
      yield break;

    }

    #endregion

  } // BehaviourCreator
}