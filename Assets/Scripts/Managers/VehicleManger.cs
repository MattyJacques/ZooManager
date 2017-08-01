// Title        : VehicleManager.cs
// Purpose      : Creates, destroys, and manages visitors
// Author       :  Alexander Falk
// Date         : 11.02.2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public List<Vehicle> _activeVehicles = new List<Vehicle> ();
    private GameObject[] VehiclePrefabs;

    public float interval;
    public float intervalMax;
    public float intervalMin;
    public bool stop;
    public Vector3 spawnPoint;

    private void Start()
    {
      LoadVehiclePrefabs();

      StartCoroutine(VisitorSpawner());
    } //Start()

    private void Update()
    {
      interval = Random.Range(intervalMin,intervalMax);
    }


    public void CreateVehicle(Vector3 position, GameObject vehiclePrefab)
    { //Creates a new vehicle based on specific parameters

      Vehicle newVehicle = new Vehicle();
      Instantiate(vehiclePrefab,position,Quaternion.identity);
      //TODO: Implement when ai is merged
      //newVisitor.getAIScript.StartBehaviour();
      _activeVehicles.Add (newVehicle);

    } //CreateVehicle()

    public void CreateRandomVehicle(Vector3 position)
    { //Creates a random vehicle

      int random = Random.Range (0, VehiclePrefabs.Length);
      CreateVisitor (position, VehiclePrefabs[random]);

    } //CreateRandomVehicle()

    public void DestroyVehicle(Vehicle vehicle)
    { //Destroys a specific vehicle

      //TODO: implement when AI is merged
      //visitor.getAIScript.StopBehaviour();
      _activeVisitors.Remove (visitor);
      //TODO: Destroy gameObject

    } //DestroyVehicle()


    private void LoadVehiclePrefabs()
    { //Loads the prefab visitor vehicles from the vehicle folder

      VehiclePrefabs = Resources.LoadAll<GameObject> ("Visitors/Vehicles/Prefabs");

    }//LoadVehiclePrefabs

    IEnumerator VehicleSpawner()
    {
      yield return new WaitForSeconds (1);

      while (!stop)
      {
        SpawnCreateRandomVehicle(spawnPoint);

        yield return new WaitForSeconds(interval);
      }
    }
  }
}