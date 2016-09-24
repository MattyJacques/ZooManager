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
    public Rect _locRect = new Rect(Screen.width - (Screen.width / 10),(Screen.height / 10),20,20); //Location of the starting pos
    public float _money = 0.0f;                                                                     //how much money the zoo has
    public List<Receipt> _fundLog = new List<Receipt>();                                              //List of all transactions
    public List<Texture2D> _icons = new List<Texture2D>();                                          //list of all icons for the log
    public bool showlog = false;                                                                    //Whether log is shown or not
    
    private void OnGUI()
    { // Display money and the log
      
      GUI.Label(_locRect, _money.ToString());
      if (showlog)
      {
        Rect tempRect = _locRect;
        tempRect.x += 20;
        Rect tempImgRect = _locRect;
        foreach(Receipt receipt in _fundLog)
        {
          tempRect.y += 20;
          tempImgRect.y += 20;
          tempRect.width = receipt.Print().Length * 7.5f;
          
          GUI.Label(tempRect, receipt.Print());
          GUI.DrawTexture(tempImgRect,_icons[(int)receipt._type]);
        }
      }
    } // OnGUI()
        
    public void AddFunds(float amount, Receipt.Type type = Receipt.Type.NA, string whatFor = "N/A")
    {//Used to add money // Use AllocateFunds if you are subtracting
      _money += amount;
      _fundLog.Add(new Receipt(type,amount,whatFor));
    }//AddFunds type float for
    
    public bool AllocateFunds(float withdraw, Receipt.Type type = Receipt.Type.NA, string whatFor = "N/A")
    {//Subtract funds // Use AddFunds if you are adding
      withdraw = Mathf.Abs(withdraw);
      if (_money - withdraw >= 0)
      {
        AddFunds(-withdraw,type,whatFor);
        return true;
      }
        return false;
    }
    
    public void ShowLog()
    {//Shows/hides the log
      showlog = !showlog;
    }
    
  }
}
