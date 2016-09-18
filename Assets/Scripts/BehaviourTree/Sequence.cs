using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.BehaviourTree
{
  public class Sequence : BehaveComponent
  {
    private BehaveComponent[] _behaviours;

    public Sequence(BehaveComponent[] behaviours)
    { // Constructor to set up behaviours of the sequence
      _behaviours = behaviours;
    } // Sequence


    public override ReturnCode Behave()
    { // Process the given behaviour, returning to return code

      bool isRunning = false;

      for (int i = 0; i < _behaviours.Length; i++)
      { // Process all of the behaviours in the array for this sequence
        try
        {
          switch (_behaviours[i].Behave())
          { // Process current behaviour, if a behaviour is currently running
            // set isRunning to true, if we fail or default return the return
            // code, else continue

            case ReturnCode.Failure:
              _returnCode = ReturnCode.Failure;
              return _returnCode;
            case ReturnCode.Success:
              continue;
            case ReturnCode.Running:
              isRunning = true;
              continue;
            default:
              _returnCode = ReturnCode.Success;
              return _returnCode;
          }
        } // try
        catch (Exception excep)
        { // Print out the exception for debugging purposes and continue with
          // behaviour checking

          Debug.Log(excep.ToString());
          continue;
        } // catch
      } // for i < _behaviours.Length

      //if none running, return success, otherwise return running
      _returnCode = !isRunning ? ReturnCode.Success : ReturnCode.Running;
      return _returnCode;
    } // Behave()

  } // Sequence
}
