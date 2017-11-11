// Sifaka Game Studios (C) 2017

using Assets.Scripts.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Components.Behaviour
{
    public class BehaviourComponent 
        : MonoBehaviour
    {
        public string BehaviourName;

        protected void Start ()
        {
            var behaviour = BehaviourCreator.Instance.GetBehaviour(BehaviourName);
            if (behaviour != null)
            {
                StartCoroutine(behaviour.Behave(gameObject));
            }
            else
            {
                Debug.LogError("Tried to load behaviour with name " + BehaviourName + " which does not exist!");
            }
        }
    }
}
