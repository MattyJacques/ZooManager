// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;

namespace Assets.Scripts.Components.Placeable
{
    public class RequestPlaceableMessage
        : UnityMessagePayload
    {
        public string ModelName { get; private set; }
        public string EntityName { get; private set; }

        public RequestPlaceableMessage(string inModelName, string inEntityName)
            : base()
        {
            ModelName = inModelName;
            EntityName = inEntityName;
        }
    }
}
