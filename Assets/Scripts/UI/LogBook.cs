using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class LogBook : MonoBehaviour 
{
  public Component _funds;
  private enum Pages {Stats,Funds,Misc};
  private Pages _page = Pages.Stats;
  public bool _showlog = false;
  public Rect _locRect = new Rect(Screen.width / 8,(Screen.height / 10),250,300);
  public List<Rect> _buttonRects = new List<Rect>();
  public List<Rect> _templateRects = new List<Rect>();
  public List<Texture2D> _pages = new List<Texture2D>();
  public List<Texture2D> _icons = new List<Texture2D>();
  
      
  bool shouldFilter = false;
  Receipt.Type filter = Receipt.Type.NA;
	
  private void Start()
  {
    _funds = GetComponent("FundManager");
  }
	void Update () 
  {
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
	}
  
  private void OnGUI()
    {
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
    {
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
      List<Receipt> revReceipts = new List<Receipt>(_funds.GetComponent<Assets.Scripts.Managers.FundManager>()._fundLog);
      revReceipts.Reverse();
      
      foreach(Receipt receipt in revReceipts)
      {
        if ((receipt._type == filter && shouldFilter) || !shouldFilter)
        {
          tempRect.y += 40;
          tempImgRect.y += 40;
          tempRect.width = receipt.Print().Length * 7.5f;
          
          GUI.Label(tempRect, receipt.Print());
          GUI.DrawTexture(tempImgRect,_funds.GetComponent<Assets.Scripts.Managers.FundManager>()._icons[(int)receipt._type]);
          
          iter++;
          if (iter == 5)
          { return;}
        }
      }
    }
}
