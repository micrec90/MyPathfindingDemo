using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    string path = "Config";
    private void Start()
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        LoadGraphFromFile(ta.text);
    }
    private void LoadGraphFromFile(string text)
    {
        GraphFileInfo graphFileInfo = JsonUtility.FromJson<GraphFileInfo>(text);
        Graph graph = new Graph(graphFileInfo.nodes, graphFileInfo.size);
    }
}
