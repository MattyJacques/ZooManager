// Title        : FundManager.cs
// Purpose      : Controlls the money
// Author       : Jacob Miller
// Date         : 22/09/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
  public class FundManager : MonoBehaviour 
  {
    public Rect _locRect = new Rect(Screen.width - (Screen.width / 10),(Screen.height / 10),20,20);
    public float _money = 0.0f;
    public List<Recipt> _fundLog = new List<Recipt>();
    public List<Texture2D> _icons = new List<Texture2D>();
    public bool showlog = false;
    
    private void OnGUI()
    { // Display buttons for rotation 
      
      GUI.Label(_locRect, _money.ToString());
      Rect tempRect = _locRect;
      tempRect.x += 20;
      Rect tempImgRect = _locRect;
      foreach(Recipt recipt in _fundLog)
      {
        tempRect.y += 20;
        tempImgRect.y += 20;
        tempRect.width = recipt.Print().Length * 7.5f;
        
        GUI.Label(tempRect, recipt.Print());
        GUI.DrawTexture(tempImgRect,_icons[(int)recipt._type]);
      }
    } // OnGUI()
        
    public void AddFunds(float amount, Recipt.Type type = Recipt.Type.NA, string whatFor = "N/A")
    {//Used to add money // Use AllocateFunds if you are subtracting
      _money += amount;
      _fundLog.Add(new Recipt(type,amount,whatFor));
    }//AddFunds type float for
    
    public bool AllocateFunds(float withdraw, Recipt.Type type = Recipt.Type.NA, string whatFor = "N/A")
    {//Subtract funds // Use AddFunds if you are adding
      withdraw = Mathf.Abs(withdraw);
      if (_money - withdraw >= 0)
      {
        AddFunds(-withdraw,type,whatFor);
        return true;
      }
        return false;
    }
    
  }
}
