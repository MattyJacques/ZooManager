using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData 
{
	public static GameData current;
	public TimeData lastLaunchDateTime;
	public Achievements ach;

	// Use this for initialization
	public GameData () 
	{ // Getting default GameData
		lastLaunchDateTime = new TimeData ();
		ach = new Achievements ();
    } // GameData()
} // GameData
