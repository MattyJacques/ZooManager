using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metaManager : MonoBehaviour {

	// ~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	public string _playerName;
	public int _achievementNumber;
	public int _achievementCompleteNumber;
	

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