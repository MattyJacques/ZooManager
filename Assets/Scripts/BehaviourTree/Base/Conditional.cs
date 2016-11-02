using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public delegate bool ConditionalDelegate(AIBase theBase);

  public class Conditional : BehaveComponent
  {
    private ConditionalDelegate _testCondition;

    public Conditional() { }

    public Conditional(ConditionalDelegate testCondition)
    { // Constructor to set up the test for the condition
      _testCondition = testCondition;
    } // Conditional


    public override ReturnCode Behave(AIBase theBase)
    { // Perform the given behaviour by testing the condition given in
      // the constructor

      try
      {
        switch (_testCondition(theBase))
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

  } // Conditional
}
