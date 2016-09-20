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


    //private Sequence CreateAnimalThirst()
    //{ // Creates a sequence that will allow an animal object to check
    //  // if thirsty, if so will find the nearest trough object and go
    //  // drink

    //  Drink drinkNode;

    //  Sequence thirstSequence = new Sequence;

    //  return thirstSequence;

    //} // CreateThirst()


    private ReturnCode SetTarget(Vector3 target)
    { // Find the closest target with the desired
      ReturnCode returnCode = ReturnCode.Success;
      return returnCode;
    } // SetTarget()


    public Assets.Scripts.BehaviourTree.Base.Behaviour CreateRoot(Selector theSelector)
    { // Create a behaviour tree using the Selector provided, return the
      // created tree

      Assets.Scripts.BehaviourTree.Base.Behaviour newBehaviour = new Assets.Scripts.BehaviourTree.Base.Behaviour(theSelector);
      return newBehaviour;
    } // CreateBehaviour()


    public Selector CreateSelector(BehaveComponent[] theComponents)
    { // Create the selector using the nodes provided, return the
      // created selector

      Selector newSelector = new Selector(theComponents);
      return newSelector;
    } // CreateSelector()


    public Sequence CreateSequence(BehaveComponent[] theComponents)
    { // Create the sequence using the nodes provided, return the
      // created sequence

      Sequence newSequence = new Sequence(theComponents);
      return newSequence;
    } // CreateSequence()


    public Conditional CreateCondition(ConditionalDelegate theDelegate)
    { // Create the conditional node using the delegate provided,
      // return the created conditional node

      Conditional newCondition = new Conditional(theDelegate);
      return newCondition;
    } // CreateCondition()


    public Action CreateAction(ActionDelegate theDelegate)
    { // Create the action node using the action delegate provided,
      // return the created node

      Action newAction = new Action(theDelegate);
      return newAction;
    } // CreateAction()

  } // BehaviourCreator
}
