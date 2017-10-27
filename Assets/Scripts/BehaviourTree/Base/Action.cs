// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public delegate IEnumerator ActionDelegate(AIBase theBase, System.Action<ReturnCode> returnCode);   // The action of the behaviour

  public class Action : BehaviourBase
  {
    private ActionDelegate _action;              // The action of the behaviour

    public Action() { }

    public Action(ActionDelegate action)
    { // Constructor to set up the action of the node
      _action = action;
    } // Action()


    public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Perform the assigned action, returning the return code of the
      // behaviour

      ReturnCode result = ReturnCode.Failure;
      yield return CoroutineSys.Instance.StartCoroutine(_action(theBase, val => result = val)); // Do the action as Coroutine and get it's result
      returnCode(result); // Set the result
      yield break; // Exit coroutine

    } // Behave()

  } // Action()
}
