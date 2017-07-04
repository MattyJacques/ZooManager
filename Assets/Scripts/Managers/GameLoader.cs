// Title        : GameManager.cs
// Purpose      : Loads prefabs, IDs and templates into the BuildingManager and AnimalManager lists
// Author       : Niels Weber & Matthew Jacques
// Date         : 02/07/2017

using System.Collections;                 
using System.Collections.Generic;         // Lists
using System.IO;                          // Directory Infos
using UnityEngine;                        // Monobehaviour
using Assets.Scripts.Characters.Animals;  // AnimalBase
using Assets.Scripts.Buildings;           // BuildingBase
using Assets.Scripts.Helpers;             // JSONReader

namespace Assets.Scripts.Managers
{
    public class GameLoader : MonoBehaviour
    {
        private AnimalManager _animalManager;   //Refer to AnimalManager script in scene
        private bool loaded;                    //Check if the AnimalManager has been loaded, then move on to BuildingManager loading
        public string test = "Animals";         //DESTROY IF SCRIPT IS CORRECT AND CHANGE LoadPrefabDirectory("Assets/Resources/" + test) 
                                                //INTO LoadPrefabDirectory("Assets/Resources/Animals");

        private void Start()
        {
            if (_animalManager == null)     //Look for any AnimalManager scripts in the scene
            {
                _animalManager = FindObjectOfType<AnimalManager>();
            }
            StartCoroutine(LoadingOrder());
        }

        private IEnumerator LoadingOrder()  //This function makes sure the buildings are loaded after the animals have been loaded.
        {
            Debug.Log("------------Loading Animal Assets...-----------");
            LoadPrefabDirectory("Assets/Resources/" + test);
            yield return new WaitUntil(() => loaded = true);
            Debug.Log("------------Loading Building Assets...------------");
            LoadPrefabDirectory("Assets/Resources/Buildings");
            yield break;
        }

        private void LoadPrefabDirectory(string directory)  //Loads all IDs, prefabs and templates in given directory and subdirectories.
        { 
            //Get the Animal or Building directories and subdirectories
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();

            if (!loaded) //...which means we're loading the Animal data
            {
                //Read JSON data
                AnimalTemplateCollection _templates;
                _templates = JSONReader.ReadJSON<AnimalTemplateCollection>("AnimalData");

                foreach (DirectoryInfo dir in subDirInfo)
                { // Loop through all directories looking for animals and buildings

                    foreach (FileInfo file in dir.GetFiles())
                    { // Check each directory to see if it contains a prefab file. If so, store
                      // in an Animal object

                        if (file.Name.EndsWith("prefab"))
                        { // Create a new Animal object with the ID, prefab and template
                          // of the animal found
                            AnimalManager.Animal newAnimal = new AnimalManager.Animal();
                            newAnimal.ID = Path.GetFileNameWithoutExtension(file.Name);
                            newAnimal.Prefab = (GameObject)Resources.Load("Animals/" + dir.Name + "/"
                              + Path.GetFileNameWithoutExtension(file.Name));
                            Debug.Log("Loaded " + dir.Name + "/" + file.Name);

                            foreach (AnimalTemplate template in _templates.animalTemplates)
                            { // Search the loaded templates for one matching the prefab, setting
                              // if found
                                if (template.id == newAnimal.ID)
                                {
                                    newAnimal.Template = template;
                                    break;
                                }
                            }

                            if (newAnimal.Template != null)
                            { // Add the animal to the collection if setup was successful
                                _animalManager._animalCollection.Add(newAnimal);
                                loaded = true;
                            }
                            else
                            { // Raise an exception if template not found
                                Debug.Assert(newAnimal.Template != null, "Animal template not found: " + file.Name);
                            }

                        } // if (file.Name.EndsWith("prefab"))
                    } // foreach (FileInfo file in dir.GetFiles())
                } // foreach (DirectoryInfo dir in subDirectories)
            } //if (!loaded)
            else //...which means we're loading the Building Data
            {
                BuildingManager._buildings = new List<GameObject>();

                foreach (DirectoryInfo dir in subDirInfo)
                {
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        if (file.Name.EndsWith("prefab"))
                        {
                            BuildingManager._buildings.Add((GameObject)Resources.Load(dir.Name + "/" + file.Name));
                            Debug.Log("Loaded " + dir.Name + "/" + file.Name);
                        }
                    }//foreach (FileInfo file in dir.GetFiles())

                }//foreach (DirectoryInfo dir in subDirInfo)
            }// else, so if (loaded)
        }
    }//GameManager
}//namespace
