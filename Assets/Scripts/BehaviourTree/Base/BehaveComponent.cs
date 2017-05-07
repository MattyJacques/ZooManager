using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public abstract class BehaveComponent
  {
    protected ReturnCode _returnCode;

    public BehaveComponent() { }

    public abstract IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode);
   
  } // Component
}
