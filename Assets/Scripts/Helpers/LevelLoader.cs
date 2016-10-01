using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Animals;
using Assets.Scripts.Managers;


namespace Assets.Scripts.Helpers
{

  public class LevelLoader
  {
    LevelTemplateCollection LoadLevelData()
    { // Load the level data from LevelData.json, storing in _templates

      return JSONReader.ReadJSON<LevelTemplateCollection>("LevelData");

    } // LoadLevelData()

    void LoadLevel(ref BuildingManager buildMGR, ref AnimalManager animalMGR, LevelTemplate template)
    { // Load the level that corrosponds to the index given as a argument

      foreach (LevelBuildingTemplate building in template.buildings)
      {
        buildMGR.Create(building);
      }

      foreach (LevelAnimalTemplate animal in template.animals)
      {
        animalMGR.Create(animal);
      }

    } // LoadLevel()  

  } // LevelData
} //Namespace
