// Title        : GameClock.cs
// Purpose      : Controls the games relative clock and the day / night cycle. Add this script to the directional light (sun).
// Author       : Dan Budworth-Mead
// Date         : 20/08/2016

using UnityEngine;
using System.Collections;

public class GameClock : MonoBehaviour
{
    float currentTimeSeconds;
    int minute, hour;
    float timeMultiplier_;
    public float timeMultiplier
    {
        get
        {
            return timeMultiplier_;
        }
        set
        {
            timeMultiplier_ = value;
        }
    }
    float convertedDeltaTime_;
    public float convertedDeltaTime
    {
        get
        {
            return convertedDeltaTime_;
        }
    }

    // Use this for initialization
    void Start ()
    {
        transform.position = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 180, 1);
        currentTimeSeconds = PlayerPrefs.GetFloat("currentTimeSeconds");
        timeMultiplier_ = 1.0f;
        transform.Rotate(new Vector3(-currentTimeSeconds / 240, 0, 0));
    }
	
	// Update is called once per frame
	void Update ()
    {
        convertedDeltaTime_ = Time.deltaTime / 0.0416666667f * timeMultiplier_;
        currentTimeSeconds += convertedDeltaTime;

        minute = (int)(currentTimeSeconds / 60);
        hour = (int)(minute / 24);

        transform.Rotate(new Vector3(-convertedDeltaTime / 240, 0, 0));
    }

    void GetTime(out int a_hour, out int a_minute)
    {
        a_hour = hour;
        a_minute = minute;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("currentTimeSeconds", currentTimeSeconds);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("currentTimeSeconds", currentTimeSeconds);
    }
}
