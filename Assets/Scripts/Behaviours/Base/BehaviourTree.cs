// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Blackboards;
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
        public static string GameObjectKey = "GameObject";

        public readonly BehaviourBase Root;

        public BehaviourTree(BehaviourBase root)
        { 
            Root = root;
        } 

        // Still pass in AIBase for now, eventually we will be passing in a Blackboard component that contains all the init
        // information we require
        public IEnumerator Behave(GameObject inGameObject)
        {
            if (Root == null)
            {
                Debug.LogError("Root is null!");
            }
            else
            {
                var blackboard = new Blackboard();
                blackboard.InstanceBlackboard.Add(GameObjectKey, new BlackboardItem(inGameObject));

                while (true) // Do it forever as the operations will be performed asynchronously
                {
                    yield return null;
                    Debug.Log("Start BT");
                    yield return CoroutineSys.Instance.StartCoroutine(Root.Behave(blackboard, val => { }));
                    Debug.Log("End BT");
                    yield return new WaitForSeconds(UpdateDelay);
                }
            }

            yield return null;
        } 
    } 
}
