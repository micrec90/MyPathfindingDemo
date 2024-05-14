using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector2Int maxMapSize;
    [SerializeField]
    [TextArea(10, 11)]
    private string map;

    private void Start()
    {
        int[,] nodes = new int[maxMapSize.x, maxMapSize.y];
        string[] lines = map.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] strings = line.Split(' ', '\t');
            for (int j = 0; j < strings.Length; j++)
            {
                int tileType;
                bool parsed = int.TryParse(strings[j], out tileType);
                if (!parsed)
                    continue;
                nodes[i,j] = tileType;
            }
        }
        Graph graph = new Graph(nodes);
        var path = BFS.FindPath(graph.Nodes[1, 0], graph.Nodes[1,8]);
        string s = "";
        for(int i = 0; i < path.Count; i++)
        {
            s += path[i].NodeCoordinates + ", ";
        }
        Debug.Log(s);
    }
}
