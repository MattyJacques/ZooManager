// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Needs;
using UnityEngine;

namespace Assets.Scripts.Buildings.Enclosures
{
    public class EnclosureInteriorItem
    {
        public GameObject UnderlyingGameObject;
        public Transform UnderlyingTransform;
        public NeedType SatisfiedNeedType;

        public EnclosureInteriorItem(GameObject gameObject, NeedType inSatisfiedNeedType)
        {
            UnderlyingGameObject = gameObject;
            UnderlyingTransform = gameObject.transform;
            SatisfiedNeedType = inSatisfiedNeedType;
        }
    }
}
