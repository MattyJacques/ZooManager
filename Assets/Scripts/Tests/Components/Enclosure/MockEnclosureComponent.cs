// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Needs;
using UnityEngine;

namespace Assets.Scripts.Tests.Components.Enclosure
{
    public class MockEnclosureComponent 
        : MonoBehaviour
        , IEnclosureInterface
    {
        public Transform ClosestInteriorItemTransformResult { get; set; }
        public GameObject UnregisteredInteriorItem { get; set; }
        public Vector3 GetRandomPointOnTheGroundResult { get; set; }

        public void RegisterEnclosureResident(EnclosureResidentComponent inResident)
        {
            throw new System.NotImplementedException();
        }

        public void UnregisterEnclosureResident(EnclosureResidentComponent inResident)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetRandomPointOnTheGround()
        {
            return GetRandomPointOnTheGroundResult;
        }

        public Transform GetClosestInteriorItemTransform(Vector3 fromPosition, NeedType itemType)
        {
            return ClosestInteriorItemTransformResult;
        }

        public void RegisterNewInteriorItem(GameObject gameObject, NeedType itemType)
        {
            throw new System.NotImplementedException();
        }

        public void UnregisterInteriorItem(GameObject gameObject)
        {
            UnregisteredInteriorItem = gameObject;
        }
    }
}
