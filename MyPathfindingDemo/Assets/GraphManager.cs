using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    private static GraphManager instance;
    public static GraphManager Instance { get { return instance; } private set { instance = value; } }

    string path = "Config";
    Graph graph;
    SizeInfo size;

    [SerializeField]
    private Tile startingNode;
    [SerializeField]
    private Tile targetNode;

    [SerializeField]
    private GameObject tilePrefab;
    public Graph Graph { get { return graph; } }
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
    private void Start()
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        LoadGraphFromFile(ta.text);
    }
    private void LoadGraphFromFile(string text)
    {
        GraphFileInfo graphFileInfo = JsonUtility.FromJson<GraphFileInfo>(text);
        graph = new Graph(graphFileInfo.nodes, graphFileInfo.size);
        size = graphFileInfo.size;
        CreateGrid();
    }

    private void CreateGrid()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(size.rows, 1, size.columns));
        if (graph == null)
            return;
        //Node mousePosition = NodeFromWorldPoint();
        for (int i = 0; i < graph.Nodes.GetLength(0); i++)
        {
            for (int j = 0; j < graph.Nodes.GetLength(1); j++)
            {
                Vector3 topleft = transform.position - Vector3.right * size.rows / 2 + Vector3.up * size.columns / 2;
                Vector3 position = topleft + Vector3.right * (j) + Vector3.up * (1 - i);

                GameObject go = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = go.GetComponent<Tile>();
                tile.Node = graph.Nodes[i,j];
                go.GetComponent<Renderer>().material.color = tile.Node.NodeType == 0 ? Color.black : Color.white;
            }
        }
    }
    private List<Node> bfsPath = new List<Node>();
    public void SetAlgorithmPoint(Tile node)
    {
        if (node != null)
        {
            if (startingNode == null)
                startingNode = node;
            else if (startingNode == node)
            {
                startingNode = null;
                targetNode = null;
            }
            else if (targetNode == node)
                targetNode = null;
            else
                targetNode = node;
        }
        ClearPath();
        if (startingNode != null && targetNode != null)
        {
            bfsPath = BFS.FindPath(startingNode.Node, targetNode.Node);
            foreach(Node pathNode in bfsPath)
            {
                pathNode.Tile.ShowMarker(true);
            }
        }
    }
    private void ClearPath()
    {
        if (bfsPath.Count == 0)
            return;
        foreach (Node pathNode in bfsPath)
        {
            pathNode.Tile.ShowMarker(false);
        }
        bfsPath.Clear();
    }
}
