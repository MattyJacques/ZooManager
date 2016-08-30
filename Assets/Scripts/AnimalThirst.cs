using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalThirst : MonoBehaviour {
	public float thirst;
	public Text perc;
	public Thirst needsWater;
	public Button myButtond;

	// Use this for initialization
	void Start () {
		thirst = 100;
		StartCoroutine (IncreaseThirst ());
	}
	
	// Update is called once per frame
	void Update () {
		perc.text = thirst.ToString ();
		if (thirst <= 50) {
			needsWater = Thirst.True;
		}
		else {
			needsWater = Thirst.False;
		}
		Debug.Log (needsWater);
	
	}


	void OnEnable()
	{
		myButtond.onClick.AddListener(MyFunction);//adds a listener for when you click the button

	}
	void MyFunction()// your listener calls this function
	{
		thirst = 100;
	}

	public IEnumerator IncreaseThirst() {
		yield return new WaitForSeconds(0.1f);
		if (thirst > 0) {
			thirst--;
		}
		Debug.Log (thirst);
		StartCoroutine (IncreaseThirst ());
	}
}

public enum Thirst {
	False,
	True
}
