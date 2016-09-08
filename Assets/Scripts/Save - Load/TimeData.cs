using UnityEngine;
using System.Collections;

[System.Serializable] 
public class TimeData {
	public string time;

	public TimeData () 
	{
		this.time = "";
		Debug.Log (time);
	}
}
