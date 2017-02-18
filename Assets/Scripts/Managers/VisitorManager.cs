// Title        : VisitorManager.cs
// Purpose      : Initiates templates, manages instances of visitors
// Author       : Christos Alatzidis
// Date         : 03/12/2016

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

		// Holds all visitor templates read from JSON array
		public VisitorTemplateCollection _templates;

		private List<Visitor> _visitorCollection;

		// List of all active visitors
		List<VisitorBase> _visitors = new List<VisitorBase> { };

		void Start()
		{ // Call to get the templates from JSON

			_templates = JSONReader.ReadJSON<VisitorTemplateCollection>("Visitors/Visitors");

			// Load all visitors
			_visitorCollection = new List<Visitor>();
			LoadVisitors();

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

		} // Create(LevelVisitorTemplate)


		private int GetVisitorIndex(string id)
		{ // Get the index of the Visitor struct within the _visitorCollection

			int visitorIndex = -1;              // Holds the template index found

			for (int i = 0; i < _templates.visitorTemplates.Length; i++)
			{ // Check if there is a match for every template in the array


				if (_templates.visitorTemplates[i].id == id)
				{ // Check for matching ID, if found set index and break out of loop
					visitorIndex = i;
					break;
				}
			}

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
