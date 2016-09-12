using UnityEngine;
using System.Collections;

public class TestMenu2 : MonoBehaviour {

	// Use this for initialization
	void Update () {
		//Check if achievement criteria are met
        if (!GameData.current.ach.noot && GameData.current.ach.nootnum >= 3) 
		{
			GameData.current.ach.noot = true;
			SaveSystem.Save ();
		}
        if (!GameData.current.ach.clap && GameData.current.ach.clapnum >= 97) 
		{
			GameData.current.ach.clap = true;
			SaveSystem.Save ();
		}
        if (!GameData.current.ach.life && GameData.current.ach.lifenum >= 1) 
		{
			GameData.current.ach.life = true;
			SaveSystem.Save ();
		}
        if (!GameData.current.ach.oneOfMany && GameData.current.ach.onenum >= 1) 
		{
			GameData.current.ach.oneOfMany = true;
			SaveSystem.Save ();
		}
        if (!GameData.current.ach.howDidYou && GameData.current.ach.hownum >= 1) 
		{
			GameData.current.ach.howDidYou = true;
			SaveSystem.Save ();
		}
        if (!GameData.current.ach.cash && GameData.current.ach.cashnum >= 1000000) 
		{
			GameData.current.ach.cash = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.cashnum >= 1000 && GameData.current.ach.onenum >= 200 && GameData.current.ach.animnum >= 10) 
		{ // Check if victory criteria are met
			Debug.Log ("You win");
			GameData.current.ach.didIWin = 1;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.cashnum <= -500) 
		{ // Check if lose criteria are met
			Debug.Log ("You lose");
			GameData.current.ach.didIWin = 2;
			SaveSystem.Save ();
		}
  } // Update()
	
	void OnGUI () {
    // Just some testing buttons
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

			GUILayout.Box ("ZooManager");
			GUILayout.Space (10);

		if (GUILayout.Button ("Noot Noot")) 
		{
			GameData.current.ach.nootnum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("Clap along if you feel")) 
		{
			GameData.current.ach.clapnum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("Circle of Life")) 
		{
			GameData.current.ach.lifenum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("One of many")) 
		{
			GameData.current.ach.onenum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("How did you do that?")) 
		{
			GameData.current.ach.hownum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("Big Cash")) 
		{
			GameData.current.ach.cashnum++;
			SaveSystem.Save ();
		}
		if (GUILayout.Button ("Get a Animal!")) 
		{
			GameData.current.ach.animnum++;
		    SaveSystem.Save ();
		}


   } // OnGUI()

} // TestMenu2
