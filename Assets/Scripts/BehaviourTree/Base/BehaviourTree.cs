// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.BehaviourTree.Base
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

        private readonly Selector _root;

        public BehaviourTree(Selector root)
        { 
            _root = root;
        } 

        public IEnumerator Behave(AIBase theBase)
        { 
            while (true) // Do it forever as the operations will be performed asynchronously
            {
                yield return null;
                ReturnCode result;
                Debug.Log("Start BT");
                yield return CoroutineSys.Instance.StartCoroutine(_root.Behave(theBase, val => result = val));
                Debug.Log("End BT");
                yield return new WaitForSeconds(UpdateDelay);
            }
        } 
    } 
}
