using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Animals;

namespace Assets.Scripts.BehaviourTree.Base
{
  public class Selector : BehaveComponent
  {
    private BehaveComponent[] _behaviours;

    public Selector(BehaveComponent[] behaviours)
    { // Constructor to set up the behaviour array
      _behaviours = behaviours;
    } // Selector()


    public override ReturnCode Behave(AnimalBase theBase)
    {
      for (int i = 0; i < _behaviours.Length; i++)
      { // Loop through and process all behaviours in the the array
        try
        {
          switch (_behaviours[i].Behave(theBase))
          { // Process current behaviour, checking return code
            case ReturnCode.Failure:
              continue;
            case ReturnCode.Success:
              _returnCode = ReturnCode.Success;
              return _returnCode;
            case ReturnCode.Running:
              _returnCode = ReturnCode.Running;
              return _returnCode;
            default:
              continue;
          }
        } // try
        catch (Exception excep)
        { // Print out the exception for debugging purposes and continue with
          // behaviour checking

          Debug.Log(excep.ToString());
          continue;
        } // catch
      } // for i < _behaviours.Length

      _returnCode = ReturnCode.Failure;
      return _returnCode;
    } // Behave()

  } // Selector
}
