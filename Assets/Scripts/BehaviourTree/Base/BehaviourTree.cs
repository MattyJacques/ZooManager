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
        private readonly Selector _root;            
        private ReturnCode _returnCode;

        public ReturnCode returnCode { get; set; }

        public BehaviourTree(Selector root)
        { 
            _root = root;
        } 

        public IEnumerator Behave(AIBase theBase)
        { // Process the behaviour of the current selector

            while (true) // Do it forever since we are in a thread
            {
                yield return null;
                ReturnCode result;
                Debug.Log("Start BT");
                yield return CoroutineSys.Instance.StartCoroutine(_root.Behave(theBase, val => result = val)); // Start the root selector as coroutine
                Debug.Log("End BT");
                yield return new WaitForSeconds(5f);
            }

        } 
    } 
}
