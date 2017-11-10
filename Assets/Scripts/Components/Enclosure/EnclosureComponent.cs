// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buildings.Enclosures;
using Assets.Scripts.Characters.Animals;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Enclosure
{
    public class EnclosureComponent : MonoBehaviour
    {
        private const int MaxNameLength = 20;
        private const int MinNameLength = 3;

        private string _name;
        private EnclosureGUIController _enclosureGUIController;
        private Canvas _canvas;

        private readonly List<EnclosureInteriorItem> _interiorItems = new List<EnclosureInteriorItem>();
        private readonly List<EnclosureResidentComponent> _enclosureResidents = new List<EnclosureResidentComponent>();

        public void Start()
        {
            //Check that we have a general collider
            if (!GetComponent<BoxCollider>() && !GetComponent<MeshCollider>())
            {
                Debug.LogError("Enclosure " + name + " at " + transform.position.ToString() + " has no collider!");
            }

            //Get the GUI controller
            _enclosureGUIController = GetComponent<EnclosureGUIController>();
            _canvas = _enclosureGUIController.GetComponent<Canvas>();
        } 

        public void OnClick()
        { //Instantiates and initializes the GUI that allows for player interaction with the enclosure
            if (_enclosureGUIController._state == EnclosureGUIController.UIState.Uninitialized)
            {
                _enclosureGUIController.Initialize();
            }
            else if (_enclosureGUIController._state == EnclosureGUIController.UIState.Hidden)
            {
                _enclosureGUIController.ShowCanvas();
            }
            else
            {
                _enclosureGUIController.HideCanvas();
            }
        }

        public Vector3 GetRandomPointOnTheGround()
        { //Returns a random point inside of the enclosure that is on the ground
            //Get the extents of the enclosure's collider
            Bounds colBounds = GetComponent<Collider>().bounds;
            float xExtent = colBounds.extents.x;
            xExtent -= 0.25f; //Subtract so that the point isn't too close to a wall
            float zExtent = colBounds.extents.z;
            zExtent -= 0.25f;

            //Get a random point
            Vector3 randomPoint = transform.position;
            randomPoint.x += Random.Range(-xExtent, xExtent);
            randomPoint.z += Random.Range(-zExtent, zExtent);

            //Move the point to the ground (incase the ground is uneven)
            randomPoint.y += colBounds.extents.y - 0.2f;
            RaycastHit rayHit = new RaycastHit();
            if (!Physics.Raycast(randomPoint, Vector3.down, out rayHit))
            {
                Debug.LogError("Raycast inside of enclosure " + _name + " originating at " + randomPoint.ToString() +
                               " and going straight down, did not hit the ground.");
            }

            return rayHit.point;
        }

        public Transform GetClosestInteriorItemTransform(Vector3 fromPosition, EnclosureInteriorItem.InteriorItemType itemType)
        { //Returns the closest Transform of itemType
            //Check if any items of itemType exists
            if (_interiorItems.Count <= 0)
            {
                Debug.LogWarning("Tried getting the closest interior items of type " + itemType.ToString() +
                                 " but enclosure " + _name + " contains no interiorItems at all!");
                return null;
            }

            if (_interiorItems.Count(x => x.type == itemType) <= 0)
            {
                Debug.LogWarning("Tried getting the closest interior items of type " + itemType.ToString() +
                                 " but enclosure " + _name + " contains no interiorItems of that type!");
                return null;
            }

            //Get the item of type itemType
            if (itemType == EnclosureInteriorItem.InteriorItemType.Random)
            { //Random is a wildcard, so we return any item
                int r = Random.Range(0, _interiorItems.Count);
                return _interiorItems[r].transform;
            }

            //Return the closest item of itemType
            EnclosureInteriorItem interiorItem = _interiorItems.Where(x => x.type == itemType)
                .OrderBy(x => Vector3.Distance(fromPosition, x.transform.position))
                .FirstOrDefault();

            if (interiorItem == null)
            {
                return null;
            }

            return interiorItem.transform;
        }

        public void RegisterNewInteriorItem(GameObject gameObject, EnclosureInteriorItem.InteriorItemType itemType)
        { // Register a new interior object into a enclosure
            EnclosureInteriorItem newItem = new EnclosureInteriorItem(gameObject, itemType);
            _interiorItems.Add(newItem);

            Debug.Log("Added new interiorItem " + gameObject.name
                      + ", of type " + System.Enum.GetName(typeof(EnclosureInteriorItem.InteriorItemType), itemType)
                      + ", to enclosure " + _name
                      + ", at position " + gameObject.transform.position.ToString()
                      + ".");
        }

        public void RemoveInteriorItem(GameObject gameObject)
        {
            foreach (EnclosureInteriorItem item in _interiorItems)
            {
                if (item.gameObject == gameObject)
                {
                    Debug.Log("Removed interiorItem " + gameObject.name
                              + ", of tpye " + System.Enum.GetName(typeof(EnclosureInteriorItem.InteriorItemType), item.type)
                              + ", from enclosure " + _name
                              + ", at position " + gameObject.transform.position.ToString() + ".");
                    _interiorItems.Remove(item);
                    return;
                }
            }
        }

        public void RegisterNewAnimal(EnclosureResidentComponent inResident)
        { // Register a new animal into a enclosure
            _enclosureResidents.Add(inResident);

            Debug.Log("Added new animal " + inResident.gameObject.name
                      + " to enclosure " + _name);
        }

        public bool Rename(string name)
        { //Renames the enclosure
            if (name.Length > MinNameLength || name.Length < MaxNameLength)
            {
                _name = name;
                if (_canvas != null)
                {
                    _canvas.transform.Find("Text_EnclosureName").GetComponent<Text>().text = _name;
                }
                return true;
            }

            return false;
        }

        public bool PositionExistsInsideEnclosure(Vector3 pos)
        { //Returns true if pos is inside enclosure bounds

            //NOTE: Assumes that enclosure only has ONE collider, might not work with multiple.
            Debug.Log(GetComponent<Collider>().bounds.ToString());
            Debug.Log(pos.ToString());

            return GetComponent<Collider>().bounds.Contains(pos);
        }
    }
}
