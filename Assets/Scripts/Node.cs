// Title        : Node.cs
// Purpose      : Nodes for pathfinding
// Author       : Jacob Miller
// Date         : 09/17/2016

public class Node
{
  public bool Checked;
  public bool Closed;
  public bool path;
  public Node parent;
  public char type;
  public int H;
  public int G;
  public int x;
  public int y;
}