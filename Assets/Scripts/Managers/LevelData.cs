using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Animals;
using Assets.Scripts.Helpers;


namespace Assets.Scripts.Managers
{

  public class LevelData
  {
    public static LevelTemplateCollection _templates;

    static void LoadLevelData()
    { // Load the level data from LevelData.json, storing in _templates

      _templates = JSONReader.ReadJSON<LevelTemplateCollection>("LevelData");

    } // LoadLevelData()

    void LoadLevel(int index)
    { // Load the level that corrosponds to the index given as a argument

      foreach (LevelBuildingTemplate building in _templates.levels[index].buildings)
      {

      }

      foreach (LevelBuildingTemplate building in _templates.levels[index].buildings)
      {

      }

    } // LoadLevel()

  } // LevelData

} //Namespace
