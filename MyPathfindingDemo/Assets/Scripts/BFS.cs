using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BFS
{
    public static List<Node> FindPath(Node startingPosition, Node targetPosition)
    {
        List<Node> path = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        startingPosition.Parent = null;
        targetPosition.Parent = null;

        queue.Enqueue(startingPosition);
        visited.Add(startingPosition);

        while(queue.Count > 0)
        {
            Node node = queue.Dequeue();
            if (node == targetPosition)
                return path;
            foreach (Node neighbour in node.NeighbouringNodes)
            {
                if(!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    neighbour.Parent = node;
                    queue.Enqueue(neighbour);
                }
                if (neighbour == targetPosition)
                    break;
            }
            if(targetPosition.Parent != null)
                path = ReconstructPath(targetPosition);
        }

        return path;
    }

    private static List<Node> ReconstructPath(Node node)
    {
        List<Node> path = new List<Node>();
        Node n = node;

        while(n != null)
        {
            path.Insert(0, n);
            n = n.Parent;
        }
        return path;
    }
}
