using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }

    [SerializeField]
    private GraphManager graphManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
    }
    void Update()
    {
    }
    bool isToggleChecked = false;
    string rows, columns, map;

    private void OnGUI()
    {
        isToggleChecked = GUILayout.Toggle(isToggleChecked, "Editor");
        if (isToggleChecked)
        {
            rows = GUILayout.TextField(rows);
            columns = GUILayout.TextField(columns);
            if (GUILayout.Button("Generate new map"))
            {
                int rowCount, columnCount;
                bool rowsParsed = int.TryParse(rows, out rowCount);
                bool columnsParsed = int.TryParse(columns, out columnCount);

                if(rowsParsed && columnsParsed)
                    GraphManager.Instance.GenerateGraph(rowCount, columnCount);
            }
        }

    }
}
