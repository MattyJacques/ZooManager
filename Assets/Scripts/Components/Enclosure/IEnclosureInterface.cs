// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Needs;
using UnityEngine;

namespace Assets.Scripts.Components.Enclosure
{
    public interface IEnclosureInterface
    {
        void RegisterEnclosureResident(EnclosureResidentComponent inResident);
        void UnregisterEnclosureResident(EnclosureResidentComponent inResident);

        Vector3 GetRandomPointOnTheGround();

        Transform GetClosestInteriorItemTransform(Vector3 fromPosition, NeedType itemType);
        void RegisterNewInteriorItem(GameObject gameObject, NeedType itemType);
        void UnregisterInteriorItem(GameObject gameObject);
    }
}
