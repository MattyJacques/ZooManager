// Sifaka Game Studios (C) 2017

namespace Assets.Scripts
{
	[System.Serializable]
	public class LevelTemplate
	{
		public int id;
		public int winCondition;
		public int loseCondition;
		public LevelBuildingTemplate[] buildings;
	}
}