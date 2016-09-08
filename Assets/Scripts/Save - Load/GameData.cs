using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData {
	public static GameData current;
	public TimeData LastLaunchDateTime;

	// Use this for initialization
	public GameData () 
	{
		LastLaunchDateTime = new TimeData ();
	}
}
