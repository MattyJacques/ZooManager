using gui = UnityEngine.GUILayout;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class AStar : MonoBehaviour
{
    public enum NodeType { None, Wall, A, B }
    public Node[,] map;
    private const int width = 100;
    private const int height = 100;
    private Vector2 startPos = new Vector2(1,1);
    private Vector2 endPos = new Vector2(85,63);
    private List<Vector2> wall = new List<Vector2>();
    public List<Node> pathNodes = new List<Node>();
    public int currentNode = 0; 
    public Node targetNode;
    public GameObject testObject;
    IEnumerator ie;
    
    public void placePoint(string type, int xcoord, int zcoord)
    {
      switch (type)
      {
        case "start":
          if (map[(int)startPos.x,(int)startPos.y].type == 'a')
            map[(int)startPos.x,(int)startPos.y] = new Node { type = '-', pos = startPos };
          startPos = new Vector2((int)(xcoord),(int)(zcoord));
          map[(int)startPos.x,(int)startPos.y] = new Node { type = 'a', pos = startPos };
          break;
        case "end":
          if (map[(int)endPos.x,(int)endPos.y].type == 'b')
            map[(int)endPos.x,(int)endPos.y] = new Node { type = '-', pos = endPos };
          endPos = new Vector2((int)(xcoord),(int)(zcoord));
          map[(int)endPos.x,(int)endPos.y] = new Node { type = 'b', pos = endPos };
          break;
        default: //Wall
          Vector2 pos = new Vector2((int)(xcoord),(int)(zcoord));
          map[(int)pos.x,(int)pos.y] = new Node { type = 'x', pos = pos };
          wall.Add(new Vector2((int)(xcoord),(int)(zcoord)));
          break;
      }
    }
    
    private IEnumerator InitMap()
    {
        ie = FindPath().GetEnumerator();            
        map = new Node[width, height];
        for (int i = 0; i < width; i++)
        {
          for (int j = 0; j < height; j++)
          {
            map[i, j] = new Node { type = '-', pos = new Vector2(i, j) };
          }
          yield return null;
        }
        Debug.Log("Fin Init Map");
				StopAllCoroutines();
    }
    
    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
        StartCoroutine (InitMap());            
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
        placePoint("start",(int)startPos.x,(int)startPos.y);
        placePoint("end",(int)endPos.x,(int)endPos.y);
      }
      else if (Input.GetKeyDown(KeyCode.E))
      {
        StartCoroutine(pushPath());
      }
      if (pathNodes.Any())
      {
        if(targetNode == null)
        {
          targetNode = pathNodes[currentNode];
        }
        moveTowardsNode();
      }
    }
    
    private IEnumerator pushPath()
    {
      foreach (var a in FindPath()) 
      {
        yield return null;
      }
      StopAllCoroutines();
    }
    private IEnumerable FindPath()
    {
        Node startNode = map.Cast<Node>().FirstOrDefault(a => a.type == 'a');
        Node endNode = map.Cast<Node>().FirstOrDefault(a => a.type == 'b');
        Node cur = startNode;
        cur.Closed = true;
        for (; ; )
        {
            SortedList<float, Node> openNodes = new SortedList<float, Node>();
            for (int i = Mathf.Max(cur.x - 1, 0); i < Mathf.Min(cur.x +2, width); i++)
                for (int j = Mathf.Max(cur.y - 1, 0); j < Mathf.Min(cur.y + 2, height); j++)
                {
                    Node n = map[i, j];
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
            pathNodes.Add(cur);
            Debug.Log(cur.x + ", " + cur.y);
        }
        yield return null;
    }
    
    void moveTowardsNode(){
         // rotate towards the target
         Vector3 nodePos = new Vector3(targetNode.x*5,1,targetNode.y*5);
         testObject.transform.forward = Vector3.RotateTowards(testObject.transform.forward, nodePos - testObject.transform.position, 4*Time.deltaTime, 0.0f);
 
         // move towards the target
         testObject.transform.position = Vector3.MoveTowards(testObject.transform.position, nodePos,   4*Time.deltaTime);
 
         if(testObject.transform.position == nodePos)
         {
             currentNode ++ ;
             targetNode = pathNodes[currentNode];
         }
     } 
    
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
    }
}