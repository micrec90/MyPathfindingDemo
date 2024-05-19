using Assets.Scripts;
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
                    map += "1\t";
                }
                map = map.TrimEnd('\t');
                map += Environment.NewLine;
            }
            maxMapSizeChange = maxMapSize;
        }
    }
    public void GenerateMap(bool random = false)
    {
        GraphManager.Instance.ParseMapText(map, maxMapSize.x, maxMapSize.y, random);
    }
}
#if UNITY_EDITOR
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
#endif