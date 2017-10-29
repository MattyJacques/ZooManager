// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Behaviours.Base
{
    public enum ReturnCode
    { 
        Failure,                 // Needs attention
        Success,                 // No attention, continue
        Running                  // Currently active
    }

    public class BehaviourTree
    {
        public static float UpdateDelay = 5f;

        public readonly BehaviourBase Root;

        public BehaviourTree(BehaviourBase root)
        { 
            Root = root;
        } 

        public IEnumerator Behave(AIBase theBase)
        {
            if (theBase != null)
            {
                while (true) // Do it forever as the operations will be performed asynchronously
                {
                    yield return null;
                    Debug.Log("Start BT");
                    yield return CoroutineSys.Instance.StartCoroutine(Root.Behave(theBase, val => {}));
                    Debug.Log("End BT");
                    yield return new WaitForSeconds(UpdateDelay);
                }
            }
        } 
    } 
}
