using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class Drink : Assets.Scripts.BehaviourTree.Base.Action
  {
    public ReturnCode Behave(AnimalBase theBase)
    { // Perform the assigned action, returning the return code of the
      // behaviour
      try
      {
        switch (DrinkWater(theBase))
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


    private ReturnCode DrinkWater(AnimalBase theBase)
    { // Handle the drinking of the water

      theBase.Thirst = 100;
      return ReturnCode.Success;
    } // DrinkWater()

  } // Drink
}
