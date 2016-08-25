using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour {
	public Button myButtond;

	void OnEnable()
	{

		myButtond.onClick.AddListener(MyFunction);//adds a listener for when you click the button

	}
	void MyFunction()// your listener calls this function
	{
		Application.Quit();
	}

}
