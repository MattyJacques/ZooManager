// Title        : FundManager.cs
// Purpose      : Controlls the money
// Author       : Jacob Miller
// Date         : 22/09/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class FundManager : MonoBehaviour 
  {
    public Rect _locRect = new Rect(Screen.width - (Screen.width / 10),(Screen.height / 10),20,20);
    public float _money = 0.0f;
    public List<Recipt> _fundLog = new List<Recipt>();
    
    private void OnGUI()
    { // Display buttons for rotation 
      
      GUI.Label(_locRect, _money.ToString());
      Rect tempRect = _locRect;
      foreach(Recipt recipt in _fundLog)
      {
        tempRect.y += 20;
        GUI.Label(tempRect, recipt.Print());
      }
    } // OnGUI()
    
    public void AddFunds(float amount)
    {
      _money += amount;
      _fundLog.Add(new Recipt(Recipt.Type.NA,amount,"N/A"));
    }
    public void AddFunds(float amount, Recipt.Type type)
    {
      _money += amount;
      _fundLog.Add(new Recipt(type,amount,"N/A"));
    }
    public void AddFunds(float amount, Recipt.Type type, string whatFor)
    {
      _money += amount;
      _fundLog.Add(new Recipt(type,amount,whatFor));
    }
  }
}

