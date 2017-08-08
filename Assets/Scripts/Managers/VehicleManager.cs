// Title        : VehicleManager.cs
// Purpose      : Creates, destroys, and manages visitors
// Author       :  Alexander Falk
// Date         : 11.02.2017
//
using System.Collections;
using System.Collections.Generic;         // Lists
using System.IO;                          // Directory Infos
using UnityEngine;                        // Monobehaviour
using Assets.Scripts.Characters.Vehicles;  // AnimalBAse
using Assets.Scripts.BehaviourTree;       // Behaviours
using Assets.Scripts.Helpers;             // JSONReader


namespace Assets.Scripts.Managers
{
  public class VehicleManager : MonoBehaviour
  {
    public struct Vehicle
    { // Struct to hold all the information on a vehicle, this includes the ID,
      // the template and the prefab
      public string ID { get; set; }
      public VehicleTemplate Template { get; set; }
      public GameObject Prefab { get; set; }
    };
    
    public List<Vehicle> _activeVehicles = new List<Vehicle>();
    public List<VehicleBase> _vehicles = new List<VehicleBase>();
    BehaviourCreator _behaviours;

    private GameObject[] VehiclePrefabs;
    
    public float interval;
    public float intervalMax;
    public float intervalMin;
    public bool stop;
    public Vector3 spawnPoint;
    
    void Start()
    {
      LoadVehiclePrefabs();
      
     // StartCoroutine(VehicleSpawner());
    } //Start()
    
    void Update()
    {
      //interval = Random.Range(intervalMin,intervalMax);
    }
    
    
    public void CreateVehicle(Vector3 position, GameObject vehiclePrefab)
    { //Creates a new vehicle based on specific parameters
      
      Vehicle newVehicle = new Vehicle();
      newVehicle.Prefab = Instantiate(vehiclePrefab,position,Quaternion.identity);
      newVehicle.Template = new VehicleTemplate();

      VehicleBase newVehicleBase = new VehicleBase(newVehicle);

      newVehicleBase.Init();
      //TODO: Implement when ai is merged
      //newVisitor.getAIScript.StartBehaviour();
      _activeVehicles.Add (newVehicle);
      _vehicles.Add(newVehicleBase);
      
    } //CreateVehicle()
 
    public void CreateTest()
    { // Create an animal that will follow the mouse

      Vector3 position = new Vector3(0, 10, 0);
      CreateRandomVehicle(position);
    } // Create(string)
      

    public void CreateRandomVehicle(Vector3 position)
    { //Creates a random vehicle
      
      int random = Random.Range (0, VehiclePrefabs.Length);
      Debug.Log(VehiclePrefabs.Length);
      CreateVehicle (position, VehiclePrefabs[random]);
      
    } //CreateRandomVehicle()
    
    public void DestroyVehicle(Vehicle vehicle)
    { //Destroys a specific vehicle
      
      //TODO: implement when AI is merged
      //visitor.getAIScript.StopBehaviour();
      _activeVehicles.Remove (vehicle);
      //TODO: Destroy gameObject
      
    } //DestroyVehicle()
    
    
    private void LoadVehiclePrefabs()
    { //Loads the prefab visitor vehicles from the vehicle folder
      
      VehiclePrefabs = Resources.LoadAll<GameObject> ("Vehicles/Prefabs");
      
    }//LoadVehiclePrefabs
    
    IEnumerator VehicleSpawner()
    {
      yield return new WaitForSeconds (1);
      
      while (!stop)
      {
        CreateRandomVehicle(spawnPoint);
        
        yield return new WaitForSeconds(interval);
      }
    }
  }
}