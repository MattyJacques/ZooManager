// Sifaka Game Studios (C) 2017

using Assets.Scripts.Animals;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Helpers
{
    public class LevelLoader
    {
        LevelTemplateCollection LoadLevelData()
        { // Load the level data from LevelData.json, storing in _templates

            return JSONReader.ReadJSON<LevelTemplateCollection>("LevelData");
        }

        void LoadLevel(ref BuildingManager buildMGR, LevelTemplate template)
        { // Load the level that corrosponds to the index given as a argument
            foreach (var building in template.buildings)
            {
                buildMGR.Create(building);
            }
        }
    }
}
