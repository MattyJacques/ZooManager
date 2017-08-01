// Title        : VisitorManager.cs
// Purpose      : Creates, destroys, and manages visitors
// Author       : Eivind Andreassen & Alexander Falk
// Date         : 11.02.2017

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Characters.Visitors;
using Assets.Scripts.Helpers;
using Assets.Scripts.BehaviourTree;


namespace Assets.Scripts.Managers
{

	public class VisitorManager : MonoBehaviour
	{

		struct Visitor
		{ // Struct to hold all the information on a visitor, this includes the ID,
			// the template and the prefab
			public string ID { get; set; }
			public VisitorTemplate Template { get; set; }
			public GameObject Prefab { get; set; }
		};



    public float interval;
    public float intervalMax;
    public float intervalMin;
    public bool stop;
    public Vector3 spawnPoint;
    //TODO: Object pool for destroying and creating new visitors, if it happens frequently enough

    private void Start()
    {
      LoadVisitorPrefabs ();
    } //Start()

    private void Update()
    {
      interval = Random.Range(intervalMin,intervalMax);


    }

			_templates = JSONReader.ReadJSON<VisitorTemplateCollection>("Visitors/Visitors");

      Visitor newVisitor = new Visitor();
      newVisitor.Prefab = Instantiate(visitorPrefab,position,Quaternion.identity);
      //TODO: Implement when ai is merged
      //newVisitor.getAIScript.StartBehaviour();
      _activeVisitors.Add (newVisitor);

		} // Start()

		void Update()
		{
			foreach (VisitorBase visitor in _visitors)
			{
			}
		}

		public void Create(string id, int amount, Vector3 location)
		{ // Create an visitor instance using the ID field of the templates

			// Find index in array
			int index = GetVisitorIndex(id);

			if (index >= 0)
			{ // Make sure template was found before creating the visitor
				CreateVisitor(index, amount, location);
			}

		} // Create(id)

		public void Create(LevelVisitorTemplate template)
		{ // Create a visitor instance using the template loaded from the level
			// loader

			// Find index in array
			int index = GetVisitorIndex(template.id);

			if (index >= 0)
			{ // Make sure template was found before creating the visitor
				CreateVisitor(index, 1, new Vector3(template.coords.x,
					template.coords.y,
					template.coords.z));
			}


    private void SpawnVisitors (Vector3 spawnPoint,int numberToSpawn)
    {
      for(int i = 0; i < numberToSpawn; i++)
      {
        CreateRandomVisitor(spawnPoint);
        //TODO: Create specific visitor groups 
      }
    }
  }
}

// Spawning visitors
// If game is not paused run spawner at random time interval
// Spawner will spawn a bus/car and will decide on the number of people
// Once Bus/car reaches drop of point spawn x amount of people from this position
// Each person will have their own stats and favourite animals
// The Game saves a list of their favourite animals and determines their path through the Zoo 

			return visitorIndex;

		} // GetTemplateIndex()


		private void CreateVisitor(int index, int amount, Vector3 location)
		{ // Create and store the visitor using the template index, amount of visitors
			// and the current location of the visitor

			for (int i = 0; i < amount; i++)
			{ // Create as many visitors as needed

				// Create new visitor with found template
				VisitorBase newBase = new VisitorBase(_visitorCollection[index].Template,
					_visitorCollection[index].Prefab);

				// Update location of object
				newBase.Model.transform.position = location;

				// Add visitor to instances list
				_visitors.Add(newBase);
			}

		} // Create()

		private void LoadVisitors()
		{ // Load visitors from Assets/Resources

			DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Visitors");
			DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

			foreach (DirectoryInfo dir in subDirectories)
			{
				Debug.Log("Searching directory: " + dir.Name);

				foreach (FileInfo file in dir.GetFiles())
				{
					if (file.Name.EndsWith("prefab"))
					{ // Create a new Visitor struct with the ID, prefab and template of
						// the visitor found
						Visitor newVisitor = new Visitor();
						newVisitor.ID = Path.GetFileNameWithoutExtension(file.Name);
						newVisitor.Prefab = (GameObject)Resources.Load(dir.Name + "/" + file.Name);

						foreach (VisitorTemplate template in _templates.visitorTemplates)
						{
							if (template.id == newVisitor.ID)
							{
								newVisitor.Template = template;
								break;
							}
						}

						Debug.Log("Loaded " + dir.Name + "/" + file.Name);
					}
				}
			}
		} // LoadVisitors()

	} // VisitorManager

} // Namespace
