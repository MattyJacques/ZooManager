using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slide : MonoBehaviour {
	public Button myButtond;


	public GameObject twitter;
	public GameObject cam;
	public GameObject button;

	public RectTransform twitterRect;

	public Image commentbox2;
	public Text commentbox2text;
	public Image commentbox3;
	public Text commentbox3text;
	public Image commentbox4;
	public Text commentbox4text;
	public Image commentbox5;
	public Text commentbox5text;

	public float rotx;
	public float timer = 0;
	public float startingRotation;

	private bool _isPressed = false;

	private Vector2 targetOffset;
	private float defaultY = 550f;
	private float extendedY = 115f;

	void Start(){
		targetOffset = twitterRect.offsetMin;
	}

	void OnEnable()
	{
		myButtond.onClick.AddListener(MyFunction);//adds a listener for when you click the button

	}
	void MyFunction()// your listener calls this function
	{
		Debug.Log ("pressed");
		this._isPressed = !this._isPressed;

		if (this._isPressed) {
			targetOffset = new Vector2 (twitterRect.offsetMin.x, extendedY);
		} else {
			targetOffset = new Vector2 (twitterRect.offsetMin.x, defaultY);
		}

		//button.GetComponent<RectTransform>().Rotate(0, 0, 180);
	}

	void Update () {

		twitterRect.offsetMin = Vector2.Lerp(twitterRect.offsetMin, targetOffset, Time.deltaTime * 5f);

		if (this._isPressed) {
			/*
			if (twitter.GetComponent<RectTransform>().anchoredPosition.y > -177 ) {
				//print(twitter.GetComponent<RectTransform>().anchoredPosition.y);
				twitter.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,-4.5f);
			} 
			if (twitter.GetComponent<RectTransform> ().sizeDelta.y != 830) {
				twitter.GetComponent<RectTransform> ().sizeDelta += new Vector2 (0, 10f);
			}
			if (button.GetComponent<RectTransform> ().anchoredPosition.y > -648) {
				button.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,-10f);
			}*/

			if (twitter.GetComponent<RectTransform> ().anchoredPosition.y <= -177) {
				timer += Time.deltaTime;
				float speed = 0.1f;
				Color c = commentbox2.color;
				c.a = Mathf.Lerp (commentbox2.color.a, 1f, timer * speed);

				commentbox2.color = c;
				commentbox2text.color = c;
				commentbox3.color = c;
				commentbox3text.color = c;
				commentbox4.color = c;
				commentbox4text.color = c;
				SetStartRot1 ();
			}
			if (button.GetComponent<RectTransform> ().rotation.eulerAngles.z < 179 && startingRotation == 0) {
				button.GetComponent<RectTransform>().Rotate(0, 0, 5);
			}
		}else{
			/*
			if (twitter.GetComponent<RectTransform>().anchoredPosition.y < 13 ) {
				//	print(twitter.GetComponent<RectTransform>().anchoredPosition.y);
				twitter.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,4.5f);
			} 
			if (twitter.GetComponent<RectTransform> ().sizeDelta.y != 400) {
				twitter.GetComponent<RectTransform> ().sizeDelta -= new Vector2 (0, 10f);
			}
			if (button.GetComponent<RectTransform> ().anchoredPosition.y < -218) {
				button.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0, 10f);
			}*/

				timer += Time.deltaTime;
				float speed = 0.05f;
				Color d = commentbox2.color;
				d.a = Mathf.Lerp (commentbox2.color.a, 0f, timer * speed);
				commentbox2.color = d;
				commentbox2text.color = d;
				commentbox3.color = d;
				commentbox3text.color = d;
				commentbox4.color = d;
				commentbox4text.color = d;
			SetStartRot2 ();

			if (button.GetComponent<RectTransform> ().rotation.eulerAngles.z > 0) {
				button.GetComponent<RectTransform>().Rotate(0, 0, -5);
			}
		}
	}

	void SetStartRot1() {
		startingRotation = 180;
		Debug.Log (startingRotation);
	}
	void SetStartRot2() {
		startingRotation = 0;
		Debug.Log (startingRotation);
	}
}