// Title        : BehaveComponent.cs
// Purpose      : Abstract template for a behaviour component, all templates
//                inherit this
// Author       : Matthew Jacques

using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public abstract class BehaveComponent
  {
    protected ReturnCode _returnCode;

    public BehaveComponent() { }

    public abstract ReturnCode Behave(AIBase theBase);
   
  } // Component
}
