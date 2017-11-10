// Sifaka Game Studios (C) 2017

namespace Assets.Scripts.Components.Enclosure
{
    public interface IEnclosureInterface
    {
        void RegisterEnclosureResident(EnclosureResidentComponent inResident);
        void UnregisterEnclosureResident(EnclosureResidentComponent inResident);
    }
}
