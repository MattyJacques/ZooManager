using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Animals;
using Assets.Scripts.Helpers;


namespace Assets.Scripts.Managers
{

public class LevelData : MonoBehaviour {
        // Which mode to find the animal template with
        enum CreateMode { ID, NAME };
        public AnimalManager manager;

        public LevelTemplateCollection _templates;

        void Start()
        { // Call to get the templates from JSON

            _templates = JSONReader.ReadJSON<LevelTemplateCollection>("LevelData");
            
        } // Start()
        
        void Update()
        {
            foreach (LevelBuildingTemplate building in _templates.buildings) {
                GameObject obj = Instantiate(Resources.Load("Buildings/Prefabs/" + building.name, typeof(GameObject)), new Vector3(building.posX,building.posY,building.posZ), new Quaternion (building.rotX, building.rotY, building.rotZ, building.rotW)) as GameObject;
            } 
        } // Update()

  } // LevelData
} //Namespace
