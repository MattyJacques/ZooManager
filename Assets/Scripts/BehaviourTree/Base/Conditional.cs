using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public delegate IEnumerator ConditionalDelegate(AIBase theBase, System.Action<bool> conditionResult);

  public class Conditional : BehaviourBase
  {
    private ConditionalDelegate _testCondition;

    public Conditional() { }

    public Conditional(ConditionalDelegate testCondition)
    { // Constructor to set up the test for the condition
      _testCondition = testCondition;
    } // Conditional


    public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Perform the given behaviour by testing the condition given in
      // the constructor

      bool result = false;
      yield return CoroutineSys.Instance.StartCoroutine(_testCondition(theBase, val => result = val)); // Run the test condition (also coroutine for future proofing)
      if(result)
      {
          returnCode(ReturnCode.Success); // Set returncode
          yield break; // Exit coroutine
      }
      else
      {
          returnCode(ReturnCode.Failure);
          yield break;
      }

    } // Behave()

  } // Conditional
}
