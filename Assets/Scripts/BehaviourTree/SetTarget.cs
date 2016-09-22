using UnityEngine;
using System.Collections;
using Assets.Scripts.BehaviourTree.Base;
using System;
using Assets.Scripts.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class SetTarget : Base.Action
  {

    public ReturnCode Behave(AnimalBase theBase)
    { // Perform the assigned action, returning the return code of the
      // behaviour
      try
      {
        switch (SetCurrTarget(theBase))
        { // Act out drinking of water on the base provided, return
          // the appropriate return code
          case ReturnCode.Success:
            _returnCode = ReturnCode.Success;
            return _returnCode;
          case ReturnCode.Failure:
            _returnCode = ReturnCode.Failure;
            return _returnCode;
          case ReturnCode.Running:
            _returnCode = ReturnCode.Running;
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


    private ReturnCode SetCurrTarget(AnimalBase theBase)
    { // Calculate the cloest source of water

      return ReturnCode.Success;

    } // SetCurrTarget()

  } // SetTarget()
}
