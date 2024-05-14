using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Node
{
    private Vector2Int nodeCoordinates;
    private int nodeType;
    private Node parent;
    private List<Node> neighbouringNodes = new List<Node>();

    public Vector2Int NodeCoordinates { get { return nodeCoordinates; } set { nodeCoordinates = value; } }
    public int NodeType { get { return nodeType; } set { nodeType = value; } }
    public Node Parent { get { return parent; } set { parent = value; } }
    public List<Node> NeighbouringNodes { get { return neighbouringNodes; } set { neighbouringNodes = value; } }
}
