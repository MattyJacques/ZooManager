// Title        : LogBookManager.cs
// Purpose      : Holds all information of money and stats
// Author       : Jacob Miller
// Date         : 24/09/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Managers
{
  public class LogBookManager : MonoBehaviour 
  {
    private enum Pages {Stats,Funds,Misc};                                              //The types of pages you can do. Allows for as many pages as needed  
    private Pages _page = Pages.Stats;                                                  //The page you are on
    private bool _showlog = false;                                                      //Whether the log is visible or not
    public Rect _locRect = new Rect(Screen.width / 8,(Screen.height / 10),750,450);     //Location of the book
    public Rect _logButtonRect;                                                         //Location of the log button
    private List<Rect> _buttonRects = new List<Rect>();                                  //Tabs rect
    private Rect _templateRects = new Rect(586, 20, 40, 40);                            //Filter buttons rect
    private List<Texture2D> _pages = new List<Texture2D>();                              //Page images
    private List<Texture2D> _icons = new List<Texture2D>();                             //List of icons
    public List<Receipt> _fundLog = new List<Receipt>();                                //List of receipts
    public float _money = 0.0f;                                                         //how much money the zoo has

    private bool shouldFilter = false;                                                  //Is the filter on
    private Receipt.Type filter = Receipt.Type.NA;                                      //Filter type
    
    void Update () 
    {//Updates the log book
      if (Input.GetMouseButtonDown(0) && _showlog)
      {
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
    
    void Awake()
    {
      string[] listOIcons = new string[] {"productIcon", "moneyIcon", "taskIcon", "paidIcon", "naIcon", "graphButton", "logIcon" };
      foreach(string fetchIco in listOIcons)
      {
        string folder = "TestArt/" + fetchIco;
        _icons.Add(((Texture2D)(Resources.Load(folder))));
      }
      string[] listOPages = new string[] {"logbook", "logbookp2", "logbookp3" };
      foreach(string fetchPage in listOPages)
      {
        string folder = "TestArt/" + fetchPage;
        _pages.Add(((Texture2D)(Resources.Load(folder))));
      }
      
      _buttonRects.Add(new Rect(0,   0, 120, 120));
      _buttonRects.Add(new Rect(0, 121, 120, 120));
      _buttonRects.Add(new Rect(0, 241, 120, 120));
      
    }//LoadIco
    
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
      Rect tempRect = _logButtonRect;
      if (_showlog)
        tempRect.x -= _locRect.width;
      
      if(GUI.Button(tempRect,_icons[6]))
      {
        ShowLog();
      }
    } // OnGUI()
    
    private void DrawFunds()
    {//Draws the fund page for the log book
      Rect tempRect = _templateRects;
      for(int i = 0; i <= 5; i++)
      {
        if(GUI.Button(tempRect, _icons[i]))
        {
          filter = (Receipt.Type)i;
          shouldFilter = true;
        }
        tempRect.x += 45;
      }
      
      tempRect = _locRect;
      tempRect.x += 110;
      tempRect.y += 40;
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
          if (iter == 9)
          { return;}
        }
      }
    }//DrawFunds()
    
    public void AddFunds(float amount, Receipt.Type type = Receipt.Type.NA, string whatFor = "N/A")
    {//Used to add money // Use AllocateFunds if you are subtracting
      _fundLog.Add(new Receipt(type, _money, amount, whatFor));
      _money += amount;
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