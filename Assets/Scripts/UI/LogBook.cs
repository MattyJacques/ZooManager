using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Managers
{
  public class LogBook : MonoBehaviour 
  {
    private enum Pages {Stats,Funds,Misc};
    private Pages _page = Pages.Stats;
    public bool _showlog = false;
    public Rect _locRect = new Rect(Screen.width / 8,(Screen.height / 10),250,300);
    public List<Rect> _buttonRects = new List<Rect>();
    public List<Rect> _templateRects = new List<Rect>();
    public List<Texture2D> _pages = new List<Texture2D>();
    public List<Texture2D> _icons = new List<Texture2D>();
    public List<Receipt> _fundLog = new List<Receipt>();
    public float _money = 0.0f;                                                         //how much money the zoo has

    bool shouldFilter = false;
    Receipt.Type filter = Receipt.Type.NA;
      
    void Update () 
    {//Updates the log book
      if (Input.GetMouseButtonDown(0) && _showlog)
      {
        int turnPage = (int)_page;
        int iter = 0;
        foreach(Rect rect in _buttonRects)
        {
          Rect testRect = new Rect(rect.x + _locRect.x, rect.y + _locRect.y, rect.width, rect.height);
          if (testRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
          {
            _page = (Pages)iter;
          }
          iter++;
        }
      }
    }//Update()
    
    private void OnGUI()
    {// Draw stuffs
      GUI.Label(new Rect(Screen.width - (Screen.width/20),0,20,20), _money.ToString());
      
      if (_showlog)
      {
        GUI.DrawTexture(_locRect,_pages[(int)_page]);
        
        if(_page == Pages.Funds)
        {//Draws the fund page
          DrawFunds();          
        }
      }
    } // OnGUI()
    
    private void DrawFunds()
    {//Draws the fund page for the log book
      for(int i = 0; i <= 4; i++)
      {
        if(GUI.Button(_templateRects[i], _icons[i]))
        {
          filter = (Receipt.Type)i;
          shouldFilter = true;
        }
      }
      
      Rect tempRect = _locRect;
      tempRect.x += 60;
      Rect tempImgRect = tempRect;
      tempRect.x += 20;
      tempImgRect.width = 20;
      tempImgRect.height = 20;
      
      int iter = 0;
      List<Receipt> revReceipts = new List<Receipt>(_fundLog);
      revReceipts.Reverse();
      
      foreach(Receipt receipt in revReceipts)
      {
        if ((receipt._type == filter && shouldFilter) || !shouldFilter)
        {
          tempRect.y += 40;
          tempImgRect.y += 40;
          tempRect.width = receipt.Print().Length * 7.5f;
          
          GUI.Label(tempRect, receipt.Print());
          GUI.DrawTexture(tempImgRect,_icons[(int)receipt._type]);
          
          iter++;
          if (iter == 5)
          { return;}
        }
      }
    }//DrawFunds()
    
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
    }//AllocateFunds()
    
    public void ShowLog()
    {//Shows/hides the log
      _showlog = !_showlog;
    }//ShowLog()
  }
}