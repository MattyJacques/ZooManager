using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;


namespace Assets.Scripts.BehaviourTree.Base
{

  public enum ReturnCode
  { // The code for the current status of the behaviour nodes
    Failure,                 // Needs attention
    Success,                 // No attention, continue
    Running                  // Currently active
  }

  public delegate ReturnCode BehaveReturn();     // INSERT WHAT THIS DOES

  public class Behaviour
  {
    private Selector _root;                // Root selector for running behaviour
    private ReturnCode _returnCode;        // Current return code of behaviour

    public ReturnCode returnCode { get; set; }

    public Behaviour(Selector root)
    { // Constructor to set the root selector of the tree
      _root = root;
    } // Behaviour()


    public IEnumerator Behave(AIBase theBase)
    { // Process the behaviour of the current selector

            while(true) // Do it forever since we are in a thread
            {
                yield return null;
                ReturnCode result;
                Debug.Log("Start BT");
                yield return CoroutineSys.Instance.StartCoroutine(_root.Behave(theBase, val => result = val)); // Start the root selector as coroutine
                Debug.Log("End BT");
                yield return new WaitForSeconds(5f);
            }
       
    } // Behave()
  } // Behaviour
}
