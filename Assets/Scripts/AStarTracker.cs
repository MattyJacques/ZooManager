// Title        : AStarTracker.cs
// Purpose      : Provides pathfinding for assets
// Author       : Jacob Miller
// Date         : 09/17/2016

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarTracker : MonoBehaviour
{
  public Vector2 _startPos = new Vector2(1,1);          //where you start
  public Vector2 _endPos = new Vector2(85,63);          //where you end
  public List<Node> _pathNodes = new List<Node>();      //list of nodes in a path
  public int _currentNode = 0;                          //cycle nodes value 
  public Node _targetNode;                              //what node you are moving to
  public Transform _testObject;                         //moving object
  public AStar _aStar;                                  //AStar
  
  public void FindPoint()
  {//starts the coroutine
    StartCoroutine(AssignCoroutine());
  }//FindPoint()
  
  public void FindPoint(Vector2 startPos,Vector2 endPos)
  {//Assigns values then starts the coroutine
    _startPos = startPos;
    _endPos = endPos;
    StartCoroutine(AssignCoroutine());
  }//FindPoint(V2,V2)
  
  public IEnumerator AssignCoroutine()
  {//Coroutine for calculating a path
    List<Node> aHold = new List<Node>();
    foreach (var a in _aStar.FindPath(_startPos,_endPos)) 
    {
      aHold = a;
      yield return null;
    }
    _pathNodes = aHold;
  }//AssignCoroutine()
  
  public void MoveTowardsNode()
  {//Checks to see if pathNodes exist and and if you have started them
    if (_pathNodes.Any())
    {
      if(_targetNode == null)
      {
        _targetNode = _pathNodes[_currentNode];
      }
      MoveTowardsNodeMath();
    }
  }//MoveTowardsNode()
  
  private void MoveTowardsNodeMath()
  {//Moves the test object along the pathnodes
    // rotate towards the target
    Vector3 nodePos = new Vector3(_targetNode.x*_aStar._scale,1,_targetNode.y*_aStar._scale);
    _testObject.transform.forward = Vector3.RotateTowards(_testObject.transform.forward, nodePos - _testObject.transform.position, 4*Time.deltaTime, 0.0f);

    // move towards the target
    _testObject.transform.position = Vector3.MoveTowards(_testObject.transform.position, nodePos,   4*Time.deltaTime);

    if(_testObject.transform.position == nodePos)
    {
      _currentNode ++ ;
      _targetNode = _pathNodes[_currentNode];
    }
 }//MoveTowardsNodeMath()
 
}//Class AStarTracker