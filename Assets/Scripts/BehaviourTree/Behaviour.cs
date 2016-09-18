using UnityEngine;
using System.Collections;
using System;


namespace Assets.Scripts.BehaviourTree
{

  public enum ReturnCode
  { // The code for the current status of the behaviour nodes
    Failure,                 // Needs attention
    Success,                 // No attention, continue
    Running                  // Currently active
  }

  public delegate ReturnCode Return();     // INSERT WHAT THIS DOES

  public class Behaviour
  {
    private Selector _root;                // Root selector for running behaviour
    private ReturnCode _returnCode;        // Current return code of behaviour

    public ReturnCode returnCode { get; set; }

    public Behaviour(Selector root)
    { // Constructor to set the root selector of the tree
      _root = root;
    } // Behaviour()


    public ReturnCode Behave()
    { // Process the behaviour of the current selector

      try
      {
        switch (_root.Behave())
        { // Process behaviour and return the correct return code
          case ReturnCode.Failure:
            _returnCode = ReturnCode.Failure;
            return _returnCode;
          case ReturnCode.Success:
            _returnCode = ReturnCode.Success;
            return _returnCode;
          case ReturnCode.Running:
            _returnCode = ReturnCode.Running;
            return _returnCode;
          default:
            _returnCode = ReturnCode.Running;
            return _returnCode;
        }
      } // try
      catch (Exception excep)
      { // Print out the exception for debugging purposes and continue with
        // behaviour checking

        Debug.Log(excep.ToString());
        _returnCode = ReturnCode.Failure;
        return _returnCode;
     
      } // catch
    } // Behave()
  } // Behaviour
}
