using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class HasTarget : Conditional
  {
    public ReturnCode Behave(AnimalBase theBase)
    { // Perform a check to see if the base has a target, returning
      // corrosponding return code

      try
      {
        switch (theBase.Path != null)
        { // Run the test condition, setting the return code appropriately
          case true:
            _returnCode = ReturnCode.Success;
            return _returnCode;
          case false:
            _returnCode = ReturnCode.Failure;
            return _returnCode;
          default:
            _returnCode = ReturnCode.Failure;
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

  } // HasTarget
}
