// Title        : metaManager.cs
// Purpose      : To store top-level information about the game
// Author       : Jeremy Mann
// Date         : 17/11/2016

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metaManager : MonoBehaviour {

	// ~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ~~~~ Player Settings ~~~~

	public string _playerName;

	public int _achievementNumber;
	public int _achievementCompleteNumber;

	string _saveSlot1Name;
	string _saveSlot2Name;
	string _saveSlot3Name;

	// ~~~~ Audio Settings ~~~~~

	public float _masterVolume;
	public float _gameVolume;
	public float _musicVolume;
	private string _audioSetup; // headphones, 2.0, 2.1, 5.1, etc.

	// ~~~~ Video Settings ~~~~~~

	public string _videoPreset; // Custom, Highest, High, Medium, Low, Potato, etc
	// need a bunch more settings, suspect it wont be a thing until much later

	// ~~~~ Gameplay Settings ~~~~

	public string _keybindPreset; // default, preset 1, preset 2, custom
	// a ton more, suspect it wont be developed until much later.

	public float _edgePanRate; //Speed of camera movement using edge pan
	public bool _edgePanDisable; // Self explanatory

	public bool _inverseMouseTrack; // Inverse mouse track using Middle Mouse
	

	// ~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	

	// ~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
	}
	

	void Update () 
	{
		
	}
}

	// ~~~~~~~~~~~~~~~~~ Function Declaration END ~~~~~~~~~~~~~~~~~~~~~~~~~~~~