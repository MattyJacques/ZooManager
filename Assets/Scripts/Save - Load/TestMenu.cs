using UnityEngine;
using System.Collections;

public class TestMenu : MonoBehaviour {

	public enum Menu {
		MainMenu,
		NewGame,
		Continue
	}

	public Menu currentMenu;

	void OnGUI () {

		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		if(currentMenu == Menu.MainMenu) {

			GUILayout.Box("ZooManager");
			GUILayout.Space(10);

			if(GUILayout.Button("New Game")) {
				GameData.current = new GameData();
				currentMenu = Menu.NewGame;
			}

			if(GUILayout.Button("Continue")) {
				SaveSystem.Load();
				currentMenu = Menu.Continue;
			}

			if(GUILayout.Button("Quit")) {
				Application.Quit();
			}
		}

		else if (currentMenu == Menu.NewGame) {

			GUILayout.Box("Save da time.");
			GUILayout.Space(5);

			GameData.current.LastLaunchDateTime.time = System.DateTime.UtcNow.ToString ();

			if(GUILayout.Button("Save")) {
				//Save the current Game as a new saved Game
				SaveSystem.Save();
				//Move on to game...
			//	Application.LoadLevel(1);
			}

			GUILayout.Space(10);
			if(GUILayout.Button("Cancel")) {
				currentMenu = Menu.MainMenu;
			}

		}

		else if (currentMenu == Menu.Continue) {

			GUILayout.Box("Select Save File");
			GUILayout.Space(10);

			foreach(GameData g in SaveSystem.savedGames) {
				if(GUILayout.Button(g.LastLaunchDateTime.time)) {
					GameData.current = g;
					//Move on to game...
					Application.LoadLevel(1);
				}

			}

			GUILayout.Space(10);
			if(GUILayout.Button("Cancel")) {
				currentMenu = Menu.MainMenu;
			}

		}

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

	}
}
