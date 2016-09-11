using UnityEngine;
using System.Collections;

public class TestMenu2 : MonoBehaviour {

	// Use this for initialization
	void Update () {
		Debug.Log (GameData.current.ach.noot.ToString ());
		if (GameData.current.ach.nootnum >= 3) 
		{
			GameData.current.ach.noot = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.clapnum >= 97) 
		{
			GameData.current.ach.clap = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.lifenum >= 1) 
		{
			GameData.current.ach.life = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.onenum >= 1) 
		{
			GameData.current.ach.oneofmany = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.hownum >= 1) 
		{
			GameData.current.ach.howdidyou = true;
			SaveSystem.Save ();
		}
		if (GameData.current.ach.cashnum >= 1000000) 
		{
			GameData.current.ach.cash = true;
			SaveSystem.Save ();
		}
	}
	
	void OnGUI () {

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


	}

}
