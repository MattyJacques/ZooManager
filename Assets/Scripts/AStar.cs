// Title        : AStar.cs
// Purpose      : Provides pathfinding for scripts
// Author       : Jacob Miller
// Date         : 09/17/2016

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AStar : MonoBehaviour
{
    public Node[,] _map;                                  //Map of all nodes
    private const int _width = 100;                       //Size of the nodes map width
    private const int _height = 100;                      //Size of the nodes map height
    public int _scale = 5;                         //The scale of the terrain.
    private List<Vector2> _walls = new List<Vector2>();   //List of wall locations
    
    public Node[,] placeWall(int xcoord, int zcoord, Node[,] map)
    {//Places wall at a location on the map.
        
      Vector2 pos = new Vector2((int)(xcoord),(int)(zcoord));
      map[(int)pos.x,(int)pos.y] = new Node { type = 'x', x = (int)pos.x, y = (int)pos.y };
      _walls.Add(new Vector2((int)(xcoord),(int)(zcoord)));
      
      return map;
    }// placePoint()
    
    public void removePoint(int xcoord, int zcoord)
    {//Removes the desired point
      _map[(int)xcoord,(int)zcoord].type = '-';
    }// removePoint()
    
    private IEnumerator initMap()
    {//Assigns the _map data with all empty nodes         
        _map = new Node[_width, _height];
        for (int i = 0; i < _width; i++)
        {
          for (int j = 0; j < _height; j++)
          {
            _map[i, j] = new Node { type = '-', x = i, y = j};
          }
          yield return null;
        }
        Debug.Log("Fin Init Map");
    }//initMap()
    
    public void Start()
    {
      StartCoroutine (initMap());
    }//Start()
    
    public IEnumerable<List<Node>> findPath(Vector2 startPos, Vector2 endPos)
    {//Pathfinding
      var tempMap = _map;
      List<Node> pathNodes = new List<Node>();
      
      tempMap[(int)startPos.x,(int)startPos.y] = new Node { type = 'a', x = (int)startPos.x, y = (int)startPos.y };
      tempMap[(int)endPos.x,(int)endPos.y] = new Node { type = 'b', x = (int)endPos.x, y = (int)endPos.y };
      
      Node startNode = tempMap.Cast<Node>().FirstOrDefault(a => a.type == 'a');
      Node endNode = tempMap.Cast<Node>().FirstOrDefault(a => a.type == 'b');
      Node cur = startNode;
      cur.Closed = true;
      for (; ; )
      {
        SortedList<float, Node> openNodes = new SortedList<float, Node>();
        for (int i = Mathf.Max(cur.x - 1, 0); i < Mathf.Min(cur.x +2, _width); i++)
          for (int j = Mathf.Max(cur.y - 1, 0); j < Mathf.Min(cur.y + 2, _height); j++)
          {
            Node n = tempMap[i, j];
            if ((n.type == '-' || n.type == 'b') && !n.Closed)
            {                            
              n.G = cur.G + (i == cur.x || j == cur.y ? 10 : 14); 
              n.H = 10 * (Mathf.Abs(endNode.x - i) + Mathf.Abs(endNode.y - j));
              n.parent = cur;
              n.Checked = true;
              openNodes.Add(n.H + n.G + Random.value, n);
              yield return pathNodes;
            }
          }            
        cur = openNodes.FirstOrDefault().Value;
        if (cur == null || cur == endNode) break;
        cur.Closed = true;
        yield return pathNodes;
      }
      Debug.Log("Found");
      while (true)
      {            
          cur = cur.parent;
          if (cur == null) break;
          cur.path = true;
          pathNodes.Add(cur);
          Debug.Log(cur.x + ", " + cur.y);
      }
      yield return pathNodes;
    }//findPath()
    
}