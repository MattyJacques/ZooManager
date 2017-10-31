// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Blackboards;

namespace Assets.Scripts.Behaviours.Base
{
    public abstract class BehaviourBase
    { 
        public abstract IEnumerator Behave(Blackboard inBlackboard, System.Action<ReturnCode> returnCode);
    }
}