using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
  public class Sequence : BehaveComponent
  {
    private BehaveComponent[] _behaviours;

    public Sequence(BehaveComponent[] behaviours)
    { // Constructor to set up behaviours of the sequence
      _behaviours = behaviours;
    } // Sequence


    public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
    { // Process the given behaviour, returning to return code

      for (int i = 0; i < _behaviours.Length; i++)
      { // Process all of the behaviours in the array for this sequence
        
        ReturnCode result = ReturnCode.Failure;
        yield return CoroutineSys.Instance.StartCoroutine(_behaviours[i].Behave(theBase, val => result = val)); // run the current behaviour as coroutine

        switch (result)
        { // Process current behaviour, checking return code
        case ReturnCode.Failure:
            returnCode(ReturnCode.Failure);
            yield break;
        case ReturnCode.Success:
            continue;
        case ReturnCode.Running:
            continue;
        default:
            returnCode(ReturnCode.Success); // set returncode
            yield break; // exit coroutine
        }
                    
      } // for i < _behaviours.Length

      returnCode(ReturnCode.Success);
      yield break;
    } // Behave()

  } // Sequence
}
