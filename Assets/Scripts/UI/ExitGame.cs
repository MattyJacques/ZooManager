using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class ExitGame : MonoBehaviour
  {
    public Button myButtond;

    private void OnEnable()
    {
      myButtond.onClick.AddListener(MyFunction); //adds a listener for when you click the button
    }

    private void MyFunction() // your listener calls this function
    {
      Application.Quit();
    }
  }
}
