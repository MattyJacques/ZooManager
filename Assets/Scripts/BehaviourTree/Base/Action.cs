// Title        : Action.cs
// Purpose      : Template for an action node for behaviours
// Author       : Matthew Jacques

using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;


namespace Assets.Scripts.BehaviourTree.Base
{
  public delegate ReturnCode ActionDelegate(AIBase theBase);   // The action of the behaviour

  public class Action : BehaveComponent
  {
    private ActionDelegate _action;              // The action of the behaviour

    public Action() { }

    public Action(ActionDelegate action)
    { // Constructor to set up the action of the node
      _action = action;
    } // Action()


    public override ReturnCode Behave(AIBase theBase)
    { // Perform the assigned action, returning the return code of the
      // behaviour
      try
      {
        switch (_action(theBase))
        { // 
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

  } // Action()
}
