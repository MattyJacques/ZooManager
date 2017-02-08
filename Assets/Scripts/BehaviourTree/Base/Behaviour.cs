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


    public ReturnCode Behave(AIBase theBase)
    { // Process the behaviour of the current selector

            while(theBase.StopBehaviour == false) // Do it forever since we are in a thread
            {
                try
                {
                    _root.Behave(theBase);
                }
                catch(Exception excep)
                {
                    Debug.Log(excep.ToString());
                    break;
                }
            }

            return ReturnCode.Failure;
       
    } // Behave()
  } // Behaviour
}
