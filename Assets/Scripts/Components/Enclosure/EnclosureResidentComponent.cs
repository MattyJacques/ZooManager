// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.Enclosure
{
    public class EnclosureResidentComponent 
        : MonoBehaviour
    {
        public IEnclosureInterface RegisteredEnclosure { get; set; }
    }
}
