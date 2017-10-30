// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Behaviours.Base
{
    public abstract class BehaviourBase
    { 
        public abstract IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode);
    }
}