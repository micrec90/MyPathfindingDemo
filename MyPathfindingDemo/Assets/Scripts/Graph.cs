using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private Node[,] nodes;
    public Node[,] Nodes { get { return nodes; } }

    public Graph(int[,] map)
    {
        nodes = new Node[map.GetLength(0), map.GetLength(1)];

        for(int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0;  j < map.GetLength(1); j++)
            {
                Node node = new Node()
                {
                    NodeCoordinates = new Vector2Int(i, j),
                    NodeType = map[i,j]
                };
                nodes[i, j] = node;
            }
        }
        // neighbours
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if(i > 0 && nodes[i - 1, j].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i-1,j]);
                if(i < nodes.GetLength(0) - 1 && nodes[i + 1, j].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i + 1, j]);
                if(j > 0 && nodes[i, j - 1].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i, j - 1]);
                if(j <  nodes.GetLength(1) - 1 && nodes[i, j + 1].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i, j + 1]);
            }
        }
    }
}
