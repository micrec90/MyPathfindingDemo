using Assets.Scripts;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector2Int maxMapSize;
    private Vector2Int maxMapSizeChange;
    [SerializeField]
    [TextArea(10, 11)]
    private string map;

    private void OnValidate()
    {
        if (maxMapSize != maxMapSizeChange)
        {
            map = string.Empty;
            for (int i = 0; i < maxMapSize.x; i++)
            {
                for (int j = 0; j < maxMapSize.y; j++)
                {
                    map += "0\t";
                }
                map = map.TrimEnd('\t');
                map += Environment.NewLine;
            }
            maxMapSizeChange = maxMapSize;
        }
    }
    public void GenerateMap(bool random = false)
    {
        int[] nodes = new int[maxMapSize.x*maxMapSize.y];
        string[] lines = map.Replace(Environment.NewLine,"\t").TrimEnd().Split('\t');
        for(int i = 0; i < lines.Length; i++)
        {
            if (random)
                nodes[i] = UnityEngine.Random.Range(0, 2);
            else
            {
                int tileType;
                bool parsed = int.TryParse(lines[i], out tileType);
                if (!parsed)
                    continue;
                nodes[i] = tileType;
            }
        }
        SizeInfo size = new SizeInfo()
        {
            rows = maxMapSize.x,
            columns = maxMapSize.y
        };
        GraphManager.Instance.GenerateGraph(nodes, size);
    }
}

[CustomEditor(typeof(GraphGenerator))]
public class GraphGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            GraphGenerator generator = (GraphGenerator)target;
            if (GUILayout.Button("Generate"))
            {
                generator.GenerateMap();
            }
            if (GUILayout.Button("Randomize"))
            {
                generator.GenerateMap(true);
            }
        }
    }
}