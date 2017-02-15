using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public delegate bool ForceFailureDelegate(AIBase theBase);

    public class ForceFailure : BehaveComponent
    {

        public ForceFailure() { }

        public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
        { // Return failure
            
            returnCode(ReturnCode.Failure); // Set returncode
            yield break; // Exit coroutine

        }

    } // ForceFailure
}
