using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Node
{
    // instead of Vector2Int which properties are called x/y which can be misleading
    public RowColumn NodeCoordinates {get; set;}
    public int NodeType { get; set; }
    public Node Parent { get; set; }
    public Tile Tile { get; set; }
    public List<Node> NeighbouringNodes { get; set; } = new List<Node>();
}
