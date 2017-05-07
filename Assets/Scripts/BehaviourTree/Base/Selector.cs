using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public class Selector : BehaveComponent
  {
    private BehaveComponent[] _behaviours;

    public Selector(BehaveComponent[] behaviours)
    { // Constructor to set up the behaviour array
      _behaviours = behaviours;
    } // Selector()


    public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
    {
      for (int i = 0; i < _behaviours.Length; i++)
      { // Loop through and process all behaviours in the the array
        
        ReturnCode result = ReturnCode.Failure;
        yield return CoroutineSys.Instance.StartCoroutine(_behaviours[i].Behave(theBase, val => result = val)); // run the current behaviour as coroutine

        switch (result)
        { // Process current behaviour, checking return code
        case ReturnCode.Failure:
            continue;
        case ReturnCode.Success:
            returnCode(ReturnCode.Success); // set returncode
            yield break; // exit coroutine
        case ReturnCode.Running:
            returnCode(ReturnCode.Failure);
            yield break;
        default:
            continue;
        }
      } // for i < _behaviours.Length

    } // Behave()

  } // Selector
}
