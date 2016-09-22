// Title        : Money.cs
// Purpose      : Controlls the money
// Author       : Jacob Miller
// Date         : 22/09/2016

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class Money : MonoBehaviour {

    // Use this for initialization
    public Rect _locRect = new Rect(Screen.width - (Screen.width / 10),(Screen.height / 10),20,20);
    public float _money = 0.0f;
    
    private void OnGUI()
    { // Display buttons for rotation 
      
      GUI.Label(_locRect, _money.ToString());
    } // OnGUI()
  }
}

