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

        public override ReturnCode Behave(AIBase theBase)
        { // Return failure
            
            _returnCode = ReturnCode.Failure;
            return _returnCode;

        }

    } // ForceFailure
}
