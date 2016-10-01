using UnityEngine;
using System.Collections;
using Assets.Scripts.Animals;

namespace Assets.Scripts.BehaviourTree.Base
{
  public abstract class BehaveComponent
  {
    protected ReturnCode _returnCode;

    public BehaveComponent() { }

    public abstract ReturnCode Behave(AnimalBase theBase);
   
  } // Component
}
