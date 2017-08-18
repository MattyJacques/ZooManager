// Title        : VehicleManager.cs
// Purpose      : Creates, destroys, and manages visitors
// Author       :  Alexander Falk
// Date         : 11.02.2017
//
using System.Collections;
using System.Collections.Generic;         // Lists
using System.IO;                          // Directory Infos
using UnityEngine;                        // Monobehaviour
using Assets.Scripts.Characters.Vehicles;  // AnimalBAse
using Assets.Scripts.BehaviourTree;       // Behaviours
using Assets.Scripts.Helpers;             // JSONReader


namespace Assets.Scripts.Characters.Visitors
{
  public class VisitorQueue : VisitorBase
  {

    public List<VisitorBase> Queue = new List<VisitorBase>();

    public void CreateQueue()
    {
      //TODO: Find every TicketBooth
      //TODO: Check if there's a queue line 
      //TODO: If there is create spaces along the line for visitors to wait until a space in front opens
      //TODO: If there isn't a queue line make visitors wait in a spot 
    }

    public void CheckPosition()
    {
      //TODO: Check if there is a person within the target 
      //TODO: If there is put a visitor in a list of waiting visitors
    }

    public void UpdateQueue()
    {
      //TODO: As soon as the visitor in the target leaves, send the first one in the list to the target
    }
  }
}

