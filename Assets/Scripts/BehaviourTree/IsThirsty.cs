using UnityEngine;
using System.Collections;
using Assets.Scripts.BehaviourTree.Base;
using System;
using Assets.Scripts.Animals;

namespace Assets.Scripts.BehaviourTree
{
  public class IsThirsty : Conditional
  {

    public override ReturnCode Behave(AnimalBase theBase)
    { // Perform the check to see if the base's thirst is below the
      // desired level, returning the appropriate return code

      try
      {
        switch (theBase.Thirst > 50)
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

  } // IsThirsty
}
