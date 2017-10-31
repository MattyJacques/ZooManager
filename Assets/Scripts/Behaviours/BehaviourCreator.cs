// Sifaka Game Studios (C) 2017

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
  public class BehaviourCreator
  {
    private Dictionary<string, BehaviourTree> _behaviours;

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

    public Base.BehaviourTree GetBehaviour(string name)
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

    }

    private void CreateBehaviours()
    { // Create all of the behaviours needed, storing them in the list

      _behaviours = new Dictionary<string, BehaviourTree>();
      
      var tree = new BehaviourTreeBuilder()
                .AddSelector()
                    .AddSequence()
                        .AddConditional(IsHungry)
                        .AddConditional(HasMyFood)
                        .AddAction(GetFood)
                        .AddAction(MoveToTarget)
                        .AddAction(EatFood)
                    .AddSequence()
                        .AddConditional(IsThirsty)
                        .AddConditional(HasWater)
                        .AddAction(GetWater)
                        .AddAction(MoveToTarget)
                        .AddAction(DrinkWater)
                    .AddSequence()
                        .AddConditional(IsBored)
                        .AddConditional(HasFun)
                        .AddAction(GetFun)
                        .AddAction(MoveToTarget)
                        .AddAction(HaveFun)
                    .AddSequence()
                        .AddAction(GetRandomInterestPoint)
                        .AddAction(MoveToTarget)
                .Build();

      _behaviours.Add("basicAnimal", tree);

    } 

    #region Actions

    private IEnumerator EatFood(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { 
      Debug.Log("EatFood(), returning success");

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemFound = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food).gameObject;

      aiBase.Enclosure.RemoveInteriorItem(itemFound);
      MonoBehaviour.Destroy(itemFound);

      aiBase.Feed(AIBase.FeedType.Food, 100);
      returnCode(ReturnCode.Success);
      yield break;
    } 

    private IEnumerator GetFood(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Get food target

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food);

      if (itemTransform != null)
      {
        Debug.Log("GetFood(), returning success");
        aiBase.pathfinder.Target = itemTransform.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetFood(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;
    } 

    private IEnumerator DrinkWater(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { 
        Debug.Log("DrinkWater(), returning success");
        var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
        aiBase.Feed(AIBase.FeedType.Water, 100);
        returnCode(ReturnCode.Success);
        yield break;
    } 

    private IEnumerator GetWater(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Get water target

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Water);

      if (itemTransform != null)
      {
        Debug.Log("GetWater(), returning success");
        aiBase.pathfinder.Target = itemTransform.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetWater(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;      
    }

    private IEnumerator HaveFun(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Handle the playing with fun object
      Debug.Log("HaveFun(), returning success");
      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      aiBase.AddFun(100);
      returnCode(ReturnCode.Success);
      yield break;      
    } 

    private IEnumerator GetFun(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Get Fun target

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Fun);

      if (itemTransform != null)
      {
        Debug.Log("GetFun(), returning success");
        aiBase.pathfinder.Target = itemTransform.position;
        returnCode(ReturnCode.Success);
      }
      else
      {
        Debug.Log("GetFun(), returning failure");
        returnCode(ReturnCode.Failure);
      }

      yield break;      
    } 

    #endregion

    #region Conditionals

    private IEnumerator IsHungry(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the animal base's hunger is at a level we class as hungry
      // If so set the next target of the base to suitable food and return true

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      Debug.Log("IsHungry(), returning " + (aiBase.Hunger < 50));

      var hungry = aiBase.Hunger < 50;

      if(hungry)
      {
        aiBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Food;
      }

      conditionResult(hungry);
      yield break;
    }

    private IEnumerator IsThirsty(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the animal base's thirst is at a level we class as thirsty
      // If so set the next target of the base to water and return true

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      Debug.Log("IsThirsty(), returning " + (aiBase.Thirst < 50));

      var thirsty = aiBase.Thirst < 50;

      if(thirsty)
      {
        aiBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Water;
      }

      conditionResult(thirsty);
      yield break;
    }

    private IEnumerator IsBored(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the animal base's boredom is at a level we class as bored
      // If so set the next target of the base to fun and return true

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      Debug.Log("IsBored(), returning " + (aiBase.Boredom < 50));

      var bored = aiBase.Boredom < 50;

      if(bored)
      {
        aiBase.NextTarget = EnclosureInteriorItem.InteriorItemType.Fun;
      }

      conditionResult(bored);
      yield break;      
    } 

    private IEnumerator HasMyFood(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the enclosure has the preffered food type

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Food);

      if (itemTransform != null)
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
    } 

    private IEnumerator HasWater(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the enclosure has a water source

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Water);

      if (itemTransform != null)
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
    } 

    private IEnumerator HasFun(Blackboard inBlackboard, System.Action<bool> conditionResult)
    { // Check if the enclosure has a fun source

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();
      var itemTransform = aiBase.Enclosure.GetClosestInteriorItemTransform(aiBase.Model.transform.position, EnclosureInteriorItem.InteriorItemType.Fun);

      if (itemTransform != null)
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
    }

    #endregion

    #region GeneralActions

    private IEnumerator MoveToTarget(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Move to the target

      Debug.Log("Starting MoveToTarget()");

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();

      aiBase.pathfinder.HasArrived = false;
      aiBase.pathfinder.CanMove = true;  // Start movement
      aiBase.pathfinder.CanSearch = true;

      while (aiBase.pathfinder.HasArrived == false)
      {
        yield return null;
      }

      aiBase.pathfinder.CanMove = false; // Prevent further movement
      aiBase.pathfinder.CanSearch = false;
      aiBase.pathfinder.HasArrived = false;

      Debug.Log("MoveToTarget(), returning success");
        
      returnCode(ReturnCode.Success);
    }

    #endregion

    #region GeneralVisitorActions

    private IEnumerator GetRandomInterestPoint(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Set's the AIBase pathfinder target to a random interest
      // point gotten from the IPManager
      Debug.Log("Getting random Interest Point");

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();

      var ipvec = IPManager.Instance.GetRandomIP();

      if(ipvec == Vector3.zero)
      { // no interest points
        returnCode(ReturnCode.Failure);
        yield break;
      }

      aiBase.pathfinder.Target = ipvec;
      aiBase.pathfinder.CanSearch = true;

      returnCode(ReturnCode.Success);
    } 

    #endregion

    #region GeneralAnimalActions

    private IEnumerator GetRandomPointInsideEnclosure(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
    { // Set's the AIBase pathfinder target to a random point
      // inside the enclosure

      Debug.Log("Getting random point inside Enclosure");

      var aiBase = inBlackboard.InstanceBlackboard[BehaviourTree.AIBaseKey].GetCurrentItem<AIBase>();

      aiBase.pathfinder.Target = aiBase.Enclosure.GetRandomPointOnTheGround();

      returnCode(ReturnCode.Success);
      yield break;

    }

    #endregion

  } // BehaviourCreator
}
