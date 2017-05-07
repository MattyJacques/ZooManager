	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;

	public class Slide : MonoBehaviour {
		public Button myButtond;
		public bool ispressed;
		public bool ispressed2;
		public GameObject twitter;
		public GameObject cam;
		public GameObject button; 
		public Image commentbox2;
		public Text commentbox2text;
		public Image commentbox3;
		public Text commentbox3text;
		public Image commentbox4;
		public Text commentbox4text;
		public Image commentbox5;
		public Text commentbox5text;
		public float rotx;
		public float timer;
		public float startingRotation;
  
		void Start() 
	  {
			ispressed = false;
			ispressed2 = false;
			timer = 0f;
		}

		void OnEnable()
		{

			myButtond.onClick.AddListener(MyFunction);//adds a listener for when you click the button

		}
		void MyFunction()// your listener calls this function
		{
			Debug.Log ("pressed");
			if (ispressed) 
		{
				ispressed2 = true;
				ispressed = false;
			} else 
		 {
				ispressed = true;
				ispressed2 = false;
			}
			//button.GetComponent<RectTransform>().Rotate(0, 0, 180);
		}

		void Update () 
	{
			if (ispressed) 
		{

				if (twitter.GetComponent<RectTransform>().anchoredPosition.y > -177 ) 
			{

					//print(twitter.GetComponent<RectTransform>().anchoredPosition.y);

					twitter.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,-4.5f);
				} 
				if (twitter.GetComponent<RectTransform> ().sizeDelta.y != 830) 
			{
					twitter.GetComponent<RectTransform> ().sizeDelta += new Vector2 (0, 10f);

				}
				if (button.GetComponent<RectTransform> ().anchoredPosition.y > -648) 
			{
					button.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,-10f);

				}
				if (twitter.GetComponent<RectTransform> ().anchoredPosition.y <= -177)
			{
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
				if (button.GetComponent<RectTransform> ().rotation.eulerAngles.z < 179 && startingRotation == 0) 
			{
					button.GetComponent<RectTransform>().Rotate(0, 0, 5);

				}
			}

			if (ispressed2)
		{
				if (twitter.GetComponent<RectTransform>().anchoredPosition.y < 13 ) 
			{

					//	print(twitter.GetComponent<RectTransform>().anchoredPosition.y);

					twitter.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,4.5f);
				} 
				if (twitter.GetComponent<RectTransform> ().sizeDelta.y != 400) 
			{
					twitter.GetComponent<RectTransform> ().sizeDelta -= new Vector2 (0, 10f);

				}
				if (button.GetComponent<RectTransform> ().anchoredPosition.y < -218) 
			{
					button.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0, 10f);

				}
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

				if (button.GetComponent<RectTransform> ().rotation.eulerAngles.z > 0) 
			{
					button.GetComponent<RectTransform>().Rotate(0, 0, -5);

				}
			}

		}

		void SetStartRot1() 
	{
			startingRotation = 180;
			Debug.Log (startingRotation);
		}
		void SetStartRot2() 
	{
			startingRotation = 0;
			Debug.Log (startingRotation);
		}

	}