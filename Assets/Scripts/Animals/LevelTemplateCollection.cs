namespace Assets.Scripts.Animals
{
    [System.Serializable]
    public class LevelTemplateCollection
    {
        public int id;
        public int winCondition;
        public int loseCondition;
        public LevelAnimalTemplate[] animals;
        public LevelBuildingTemplate[] buildings;
    }
}
