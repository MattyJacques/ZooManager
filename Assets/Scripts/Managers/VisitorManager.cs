// Author       : Eivind Andreassen
// Date         : 11.02.2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers
{
  public class VisitorManager : MonoBehaviour
  {

    public struct Visitor
    { // Struct to hold all the information on a visitor, this includes the ID,
      // the template and the prefab
      public string ID { get; set; }
      public VisitorTemplate Template { get; set; }
      public GameObject Prefab { get; set; }
    };
    //Notes:
    //About visitor AI
    //Call the StartBehaviour method after creation/placement, Call the StopBehaviour method before destroying

    public List<Visitor> _activeVisitors = new List<Visitor> ();
    private GameObject[] VisitorPrefabs;
    //TODO: Object pool for destroying and creating new visitors, if it happens frequently enough

    private void Start()
    {
      LoadVisitorPrefabs ();
    } //Start()

    public void CreateVisitor(Vector3 position, GameObject visitorPrefab)
    { //Creates a new visitor based on specific parameters

      Visitor newVisitor = new Visitor ();
      //TODO: Implement when ai is merged
      //newVisitor.getAIScript.StartBehaviour();
      _activeVisitors.Add (newVisitor);

    } //CreateVisitor()

    public void CreateRandomVisitor(Vector3 position)
    { //Creates a random visitor

      int random = Random.Range (0, VisitorPrefabs.Length);
      CreateVisitor (position, VisitorPrefabs[random]);

    } //CreateRandomVisitor()

    public void DestroyVisitor(Visitor visitor)
    { //Destroys a specific visitor

      //TODO: implement when AI is merged
      //visitor.getAIScript.StopBehaviour();
      _activeVisitors.Remove (visitor);
      //TODO: Destroy gameObject

    } //DestroyVisitor()

    private void LoadVisitorPrefabs()
    { //Loads the prefab visitors from the visitor prefabs folder

      VisitorPrefabs = Resources.LoadAll<GameObject> ("Visitors/Prefabs");
    } //LoadVisitorPrefabs()
  }
}