// Sifaka Game Studios (C) 2017

using Assets.Scripts.Animals;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Managers
{
    public class LevelData
    {
        // Which mode to find the animal template with
        enum CreateMode { ID, NAME };

        public LevelTemplateCollection _templates;

        void Start()
        { // Call to get the templates from JSON
            _templates = JSONReader.ReadJSON<LevelTemplateCollection>("LevelData");
            foreach (LevelBuildingTemplate building in _templates.levels[0].buildings)
            {
                //GameObject obj = Instantiate(Resources.Load("Buildings/Prefabs/" + building.name, typeof(GameObject)), new Vector3(building.posX,building.posY,building.posZ), new Quaternion (building.rotX, building.rotY, building.rotZ, building.rotW)) as GameObject;
            }
        }
    }
}
