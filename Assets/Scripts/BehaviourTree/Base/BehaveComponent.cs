using UnityEngine;
using System.Collections;

namespace Assets.Scripts.BehaviourTree.Base
{
  public abstract class BehaveComponent
  {
    protected ReturnCode _returnCode;

    public BehaveComponent() { }

    public abstract ReturnCode Behave();
   
  } // Component
}
