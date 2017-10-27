// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public abstract class BehaviourBase
    {
        protected ReturnCode _returnCode;

        protected BehaviourBase() { }

        public abstract IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode);
    }
}