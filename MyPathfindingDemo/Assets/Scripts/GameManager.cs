using Assets.Scripts;
using StarterAssets;
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
    [SerializeField]
    GameObject player;
    [SerializeField]
    ThirdPersonController controller;
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
    bool isToggleChecked = false;
    string rows = "5";
    string columns = "5";

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
    public void PlaySimulation(List<Node> path)
    {
        if (path.Count == 0)
            return;
        if (controller != null)
        {
            // without the temporary disable the position is not updated
            controller.gameObject.SetActive(false);
            controller.SetStartingPositon(path[0]);
            controller.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = Instantiate(player, path[0].Tile.transform.position, Quaternion.identity);
            controller = go.GetComponent<ThirdPersonController>();
        }
        controller.ClearPath();
        controller.SetPath(path);
    }
    public void DestroyPlayer()
    {
        if (controller != null)
            Destroy(controller.gameObject);
    }
}
