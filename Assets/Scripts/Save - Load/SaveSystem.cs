using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem 
{

	public static List<GameData> savedGames = new List<GameData>();

	public static void Save() 
	{
		savedGames.Add(GameData.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveSystem.savedGames);
		file.Close();
  } // Save()

	public static void Load() 
	{
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveSystem.savedGames = (List<GameData>)bf.Deserialize(file);
			file.Close();
		}
  } // Load()
} // SaveSystem
