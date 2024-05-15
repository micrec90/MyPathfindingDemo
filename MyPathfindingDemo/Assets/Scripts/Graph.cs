using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Graph
{
    private Node[,] nodes;
    public Node[,] Nodes { get { return nodes; } }

    public Graph(int[] map, SizeInfo sizeInfo)
    {
        InitGraph(map, sizeInfo);
        InitNeighbours();
    }
    public Graph(int[,] map)
    {
        nodes = new Node[map.GetLength(0), map.GetLength(1)];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Node node = new Node()
                {
                    NodeCoordinates = new RowColumn(i, j),
                    NodeType = map[i, j]
                };
                nodes[i, j] = node;
            }
        }
        InitNeighbours();
    }
    public void InitGraph(int[] map, SizeInfo sizeInfo)
    {
        nodes = new Node[sizeInfo.rows, sizeInfo.columns];
        for (int i = 0; i < map.Length; i++)
        {
            int row = i / sizeInfo.rows;
            int column = i % sizeInfo.columns;

            Node node = new Node()
            {
                NodeCoordinates = new RowColumn(row, column),
                NodeType = map[i]
            };
            nodes[row, column] = node;
        }
    }
    public void InitNeighbours()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i, j].NodeType == 0)
                    continue;

                if (i > 0 && nodes[i - 1, j].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i - 1, j]);
                if ((i < nodes.GetLength(0) - 1) && nodes[i + 1, j].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i + 1, j]);
                if (j > 0 && nodes[i, j - 1].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i, j - 1]);
                if ((j < nodes.GetLength(1) - 1) && nodes[i, j + 1].NodeType != 0)
                    nodes[i, j].NeighbouringNodes.Add(nodes[i, j + 1]);
            }
        }
    }

    public void ChangeNode(Node node)
    {
        int j = (int)node.NodeCoordinates.Column;
        int i = (int)node.NodeCoordinates.Row;
        Node up = (i > 0 && nodes[i - 1, j].NodeType != 0) ? nodes[i - 1, j] : null;
        Node down = (i < nodes.GetLength(0) - 1 && nodes[i + 1, j].NodeType != 0) ? nodes[i + 1, j] : null;
        Node left = (j > 0 && nodes[i, j - 1].NodeType != 0) ? nodes[i, j - 1] : null;
        Node right = (j < nodes.GetLength(1) - 1 && nodes[i, j + 1].NodeType != 0) ? nodes[i, j + 1] : null;

        node.NodeType = (node.NodeType + 1)% 2;
        if (node.NodeType == 0)
        {
            up?.NeighbouringNodes.Remove(node);
            down?.NeighbouringNodes.Remove(node);
            left?.NeighbouringNodes.Remove(node);
            right?.NeighbouringNodes.Remove(node);
        }
        else
        {
            SetNeighbour(node, up);
            SetNeighbour(node, down);
            SetNeighbour(node, left);
            SetNeighbour(node, right);
        }
    }
    private void SetNeighbour(Node node, Node neighbour)
    {
        if (neighbour != null)
        {
            neighbour.NeighbouringNodes.Add(node);
            node.NeighbouringNodes.Add(neighbour);
        }
    }
}
