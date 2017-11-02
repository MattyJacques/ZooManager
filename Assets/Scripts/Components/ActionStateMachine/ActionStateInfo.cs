// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine
{
    public class ActionStateInfo
    {
        public ActionStateInfo()
        {
            Owner = null;
        }

        public ActionStateInfo(GameObject inOwner)
        {
            Owner = inOwner;
        }

        public GameObject Owner { get; set; }
    }
}
