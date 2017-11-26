// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;
using Assets.Scripts.Instance;
using UnityEngine;

namespace Assets.Scripts.Components.Placeable
{
    public class PlaceableComponent 
        : MonoBehaviour
    {
        public Material PlacingMaterial;
        public Material InvalidPlacementMaterial;

        private GameObject _currentObjectToPlace;
        private string _currentObjectToInstantiate;

        private UnityMessageEventHandle<RequestPlaceableMessage> _requestPlaceableMessageHandle;

        protected void Start ()
        {
            if (GameInstance.CurrentInstance != null)
            {
                _requestPlaceableMessageHandle = GameInstance.CurrentInstance.GetUIMessageDispatcher()
                    .RegisterForMessageEvent<RequestPlaceableMessage>(OnRequestPlaceableMessage);
            }
        }

        protected void OnDestroy()
        {
            if (GameInstance.CurrentInstance != null)
            {
                GameInstance.CurrentInstance.GetUIMessageDispatcher().UnregisterForMessageEvent(_requestPlaceableMessageHandle);
            }
        }

        private void OnRequestPlaceableMessage(RequestPlaceableMessage inMessage)
        {
            var placeable = Resources.Load<GameObject>(inMessage.ModelName);
            _currentObjectToPlace = Instantiate(placeable);
            _currentObjectToPlace.GetComponent<Renderer>().material = PlacingMaterial;
            SetObjectToMousePosition(placeable);

            _currentObjectToInstantiate = inMessage.EntityName;
        }

        private void SetObjectToMousePosition(GameObject inObject)
        {
            if (inObject != null)
            {
                inObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
	
        void Update ()
        {
            SetObjectToMousePosition(_currentObjectToPlace);
        }
    }
}
