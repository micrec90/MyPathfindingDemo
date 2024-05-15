using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Node
{
    // instead of Vector2Int which properties are called x/y which can be misleading
    private RowColumn nodeCoordinates;
    private int nodeType;
    private Node parent;
    private Tile tile;
    private List<Node> neighbouringNodes = new List<Node>();

    public RowColumn NodeCoordinates { get { return nodeCoordinates; } set { nodeCoordinates = value; } }
    public int NodeType { get { return nodeType; } set { nodeType = value; } }
    public Node Parent { get { return parent; } set { parent = value; } }
    public Tile Tile { get { return tile; } set { tile = value; } }
    public List<Node> NeighbouringNodes { get { return neighbouringNodes; } set { neighbouringNodes = value; } }
}
