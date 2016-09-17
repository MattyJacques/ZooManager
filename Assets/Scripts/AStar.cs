﻿// Title        : AStar.cs
// Purpose      : Provides pathfinding for scripts
// Author       : Jacob Miller
// Date         : 09/17/2016

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AStar : MonoBehaviour
{
    public enum NodeType { None, Wall, A, B }             //The types of nodes possible
    public Node[,] _map;                                  //Map of all nodes
    private const int _width = 100;                       //Size of the nodes map width
    private const int _height = 100;                      //Size of the nodes map height
    private const int _scale = 5;                         //The scale of the terrain.
    private Vector2 _startPos = new Vector2(1,1);         //FTPO where you start
    private Vector2 _endPos = new Vector2(85,63);         //FTPO where you end
    private List<Vector2> _walls = new List<Vector2>();   //List of wall locations
    public List<Node> _pathNodes = new List<Node>();      //FTPO list of nodes in a path
    public int _currentNode = 0;                          //FTPO cycle nodes value 
    public Node _targetNode;                              //FTPO what node you are moving to
    public GameObject _testObject;                        //FTPO
    
    public void placePoint(string type, int xcoord, int zcoord)
    {//Places the deisred type at a location on the map. Also removes the previous point for start and end
      switch (type)
      {
        case "start":
          if (_map[(int)_startPos.x,(int)_startPos.y].type == 'a')
            _map[(int)_startPos.x,(int)_startPos.y] = new Node { type = '-', pos = _startPos };
          _startPos = new Vector2((int)(xcoord),(int)(zcoord));
          _map[(int)_startPos.x,(int)_startPos.y] = new Node { type = 'a', pos = _startPos };
          break;
        case "end":
          if (_map[(int)_endPos.x,(int)_endPos.y].type == 'b')
            _map[(int)_endPos.x,(int)_endPos.y] = new Node { type = '-', pos = _endPos };
          _endPos = new Vector2((int)(xcoord),(int)(zcoord));
          _map[(int)_endPos.x,(int)_endPos.y] = new Node { type = 'b', pos = _endPos };
          break;
        default: //_walls
          Vector2 pos = new Vector2((int)(xcoord),(int)(zcoord));
          _map[(int)pos.x,(int)pos.y] = new Node { type = 'x', pos = pos };
          _walls.Add(new Vector2((int)(xcoord),(int)(zcoord)));
          break;
      }
    }// placePoint()
    
    private IEnumerator initMap()
    {//Assigns the _map data with all empty nodes         
        _map = new Node[_width, _height];
        for (int i = 0; i < _width; i++)
        {
          for (int j = 0; j < _height; j++)
          {
            _map[i, j] = new Node { type = '-', pos = new Vector2(i, j) };
          }
          yield return null;
        }
        Debug.Log("Fin Init Map");
    }//initMap()
    
    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
        StartCoroutine (initMap());            
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
        placePoint("start",(int)_startPos.x,(int)_startPos.y);
        placePoint("end",(int)_endPos.x,(int)_endPos.y);
      }
      else if (Input.GetKeyDown(KeyCode.E))
      {
        StartCoroutine(findPath());
      }
      if (_pathNodes.Any())
      {
        if(_targetNode == null)
        {
          _targetNode = _pathNodes[_currentNode];
        }
        moveTowardsNode();
      }
    }//Update()
    
    private IEnumerator findPath()
    {//Pathfinding
      Node startNode = _map.Cast<Node>().FirstOrDefault(a => a.type == 'a');
      Node endNode = _map.Cast<Node>().FirstOrDefault(a => a.type == 'b');
      Node cur = startNode;
      cur.Closed = true;
      for (; ; )
      {
        SortedList<float, Node> openNodes = new SortedList<float, Node>();
        for (int i = Mathf.Max(cur.x - 1, 0); i < Mathf.Min(cur.x +2, _width); i++)
          for (int j = Mathf.Max(cur.y - 1, 0); j < Mathf.Min(cur.y + 2, _height); j++)
          {
            Node n = _map[i, j];
            if ((n.type == '-' || n.type == 'b') && !n.Closed)
            {                            
              n.G = cur.G + (i == cur.x || j == cur.y ? 10 : 14); 
              n.H = 10 * (Mathf.Abs(endNode.x - i) + Mathf.Abs(endNode.y - j));
              n.parent = cur;
              n.Checked = true;
              openNodes.Add(n.H + n.G + Random.value, n);
              yield return null;
            }
          }            
        cur = openNodes.FirstOrDefault().Value;
        if (cur == null || cur == endNode) break;
        cur.Closed = true;
        yield return null;
      }
      Debug.Log("Found");
      while (true)
      {            
          cur = cur.parent;
          if (cur == null) break;
          cur.path = true;
          _pathNodes.Add(cur);
          Debug.Log(cur.x + ", " + cur.y);
      }
      yield return null;
    }//findPath()
    
    void moveTowardsNode()
    {//Moves the test object along the pathnodes
      // rotate towards the target
      Vector3 nodePos = new Vector3(_targetNode.x*_scale,1,_targetNode.y*_scale);
      _testObject.transform.forward = Vector3.RotateTowards(_testObject.transform.forward, nodePos - _testObject.transform.position, 4*Time.deltaTime, 0.0f);

      // move towards the target
      _testObject.transform.position = Vector3.MoveTowards(_testObject.transform.position, nodePos,   4*Time.deltaTime);

      if(_testObject.transform.position == nodePos)
      {
        _currentNode ++ ;
        _targetNode = _pathNodes[_currentNode];
      }
   }//moveTowardsNode()
    
    public class Node
    {
      public bool Checked;
      public bool Closed;
      public bool path;
      public Vector2 pos;
      public Node parent;
      public char type;
      public int H;
      public int G;
      public int x { get { return (int)pos.x; } }
      public int y { get { return (int)pos.y; } }
    }//(class)Node
    
    public class AStarTracker
    {
      public Vector2 _startPos = new Vector2(1,1);          //where you start
      public Vector2 _endPos = new Vector2(85,63);         //where you end
      public List<Node> _pathNodes = new List<Node>();      //list of nodes in a path
      public int _currentNode = 0;                          //cycle nodes value 
      public Node _targetNode;                              //what node you are moving to
      public Transform _testObject;                         //moving object
    }
}