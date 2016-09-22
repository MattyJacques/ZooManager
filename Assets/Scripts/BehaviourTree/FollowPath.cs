﻿using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Animals;


namespace Assets.Scripts.BehaviourTree
{
  public class FollowPath : Base.Action
  {
    public override ReturnCode Behave(AnimalBase theBase)
    { // Perform the assigned action, returning the return code of the
      // behaviour
      try
      {
        switch (FollowPathToTarget(theBase))
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


    private ReturnCode FollowPathToTarget(AnimalBase theBase)
    { // Handle the following of the current path to the target


      return ReturnCode.Success;
    } // DrinkWater()

  } // Drink
}