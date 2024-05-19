using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GraphManager : MonoBehaviour
{
    private static GraphManager instance;
    public static GraphManager Instance { get { return instance; } private set { instance = value; } }

    string path = "Config";
    Graph graph;
    SizeInfo size;

    [SerializeField]
    private Tile startingTile;
    [SerializeField]
    private Tile targetTile;

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject graphTiles;
    [SerializeField]
    SimulationManager simulationManager;
    public Graph Graph { get { return graph; } }
    private List<Node> bfsPath = new List<Node>();

    UnityEvent OnNodeChanged;
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
        if (OnNodeChanged == null)
            OnNodeChanged = new UnityEvent();
        OnNodeChanged.AddListener(ClearPath);
        OnNodeChanged.AddListener(SetupPath);
        OnNodeChanged.AddListener(PlaySimulation);
    }
    private void Start()
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        LoadGraphFromFile(ta.text);
    }
    private void LoadGraphFromFile(string text)
    {
        Clean();
        GraphFileInfo graphFileInfo = JsonUtility.FromJson<GraphFileInfo>(text);
        graph = new Graph(graphFileInfo.nodes, graphFileInfo.size);
        size = graphFileInfo.size;
        CreateGrid();
    }
    public void ParseMapText(string text, int rows, int columns, bool random = false)
    {
        int[] nodes = new int[rows * columns];
        string[] lines = text.Replace("\r", "").Replace("\n", "\t").TrimEnd().Split('\t',' ');
        for (int i = 0; i < lines.Length; i++)
        {
            if (i >= nodes.Length)
                break;
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
            rows = rows,
            columns = columns
        };
        GenerateGraph(nodes, size);
    }
    public void GenerateGraph(int[] nodes, SizeInfo size)
    {
        Clean();
        graph = new Graph(nodes, size);
        this.size = size;
        CreateGrid();
    }
    public void GenerateGraph(int rows, int columns)
    {
        Clean();
        size = new SizeInfo()
        {
            rows = rows,
            columns = columns
        };
        int[] map = new int[rows*columns];
        for (int i = 0; i < map.Length; i++)
            map[i] = 1;
        graph = new Graph(map, size);
        CreateGrid();
    }
    private void Clean()
    {
        ClearPath();
        CleanGrid();
        CleanMarkers();
        simulationManager.DestroyPlayer();
    }
    private void CleanGrid()
    {
        if (graph == null)
            return;

        foreach(Transform child in graphTiles.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void CleanMarkers()
    {
        startingTile = null;
        targetTile = null;
    }
    private void CreateGrid()
    {
        if (graph == null)
            return;
        graphTiles.transform.eulerAngles = new Vector3(0, 0, 0);
        Vector3 topleft = transform.position - Vector3.right * size.rows / 2 + Vector3.up * size.columns / 2;
        for (int i = 0; i < graph.Nodes.GetLength(0); i++)
        {
            for (int j = 0; j < graph.Nodes.GetLength(1); j++)
            {
                Vector3 position = topleft + Vector3.right * (j) + Vector3.up * (1 - i);
                GameObject go = Instantiate(tilePrefab, position, Quaternion.identity, graphTiles.transform);
                Tile tile = go.GetComponent<Tile>();
                tile.Node = graph.Nodes[i,j];
                go.GetComponent<Renderer>().material.color = tile.Node.NodeType == 0 ? Color.black : Color.white;
            }
        }
        graphTiles.transform.eulerAngles = new Vector3(90, 0, 0);
    }
    public void SetAlgorithmPoint(Tile tile)
    {
        if (tile != null && tile.Node.NodeType == 1)
        {

            if (startingTile == null)
            {
                startingTile = tile;
                startingTile.ShowMarker(true, MarkerType.Circle);
                startingTile.SetMarkerColor(Color.blue);
            }
            else if (startingTile == tile)
            {
                startingTile.ShowMarker(false);
                targetTile?.ShowMarker(false);
                startingTile = null;
                targetTile = null;

            }
            else if (targetTile == tile)
            {
                targetTile.ShowMarker(false);
                targetTile = null;
            }
            else
            {
                if(targetTile != null)
                    targetTile.ShowMarker(false);
                targetTile = tile;
                targetTile.ShowMarker(true, MarkerType.Cross);
                targetTile.SetMarkerColor(Color.red);
            }
        }
        if (OnNodeChanged != null)
            OnNodeChanged.Invoke();
    }
    public void ChangeNode(Node node)
    {
        int j = (int)node.NodeCoordinates.Column;
        int i = (int)node.NodeCoordinates.Row;
        Node up = (i > 0 && graph.Nodes[i - 1, j].NodeType != 0) ? graph.Nodes[i - 1, j] : null;
        Node down = (i < graph.Nodes.GetLength(0) - 1 && graph.Nodes[i + 1, j].NodeType != 0) ? graph.Nodes[i + 1, j] : null;
        Node left = (j > 0 && graph.Nodes[i, j - 1].NodeType != 0) ? graph.Nodes[i, j - 1] : null;
        Node right = (j < graph.Nodes.GetLength(1) - 1 && graph.Nodes[i, j + 1].NodeType != 0) ? graph.Nodes[i, j + 1] : null;

        node.NodeType = (node.NodeType + 1) % 2;
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
        if (startingTile == node.Tile)
        {
            startingTile.ShowMarker(false);
            targetTile?.ShowMarker(false);
            startingTile = null;
            targetTile = null;
        }
        else if (node.Tile == targetTile)
        {
            targetTile.ShowMarker(false);
            targetTile = null;
        }

        if (OnNodeChanged != null)
            OnNodeChanged.Invoke();
    }
    private void SetNeighbour(Node node, Node neighbour)
    {
        if (neighbour != null)
        {
            if (!neighbour.NeighbouringNodes.Contains(node))
                neighbour.NeighbouringNodes.Add(node);
            if (!node.NeighbouringNodes.Contains(neighbour))
                node.NeighbouringNodes.Add(neighbour);
        }
    }
    public void SetupPath()
    {
        if (startingTile != null && targetTile != null)
        {
            bfsPath = BFS.FindPath(startingTile.Node, targetTile.Node);
            for (int i = 0; i < bfsPath.Count; i++)
            {
                Node pathNode = bfsPath[i];
                if (i < bfsPath.Count - 1)
                {
                    Node nextPathNode = bfsPath[i + 1];
                    Vector3 difference = nextPathNode.Tile.transform.localPosition - pathNode.Tile.transform.localPosition;
                    Vector3 rotation = Vector3.zero;
                    if (difference == Vector3.right)
                    {
                        rotation = new Vector3(0, 0, 180);
                    }
                    if (difference == Vector3.up)
                    {
                        rotation = new Vector3(0, 0, -90);
                    }
                    if (difference == Vector3.down)
                    {
                        rotation = new Vector3(0, 0, 90);
                    }
                    pathNode.Tile.RotateArrow(rotation);
                }

                if (pathNode.Tile == startingTile)
                    continue;
                if (pathNode.Tile == targetTile)
                    continue;
                pathNode.Tile.ShowMarker(true, MarkerType.Arrow);
                pathNode.Tile.SetMarkerColor(Color.green);
            }
        }
    }
    private void PlaySimulation()
    {
        simulationManager.PlaySimulation(bfsPath);
    }
    private void ClearPath()
    {
        if (bfsPath.Count == 0)
            return;
        foreach (Node pathNode in bfsPath)
        {
            if (pathNode.Tile == startingTile)
                continue;
            if (pathNode.Tile == targetTile)
                continue;
            pathNode.Tile.ShowMarker(false);
        }
        bfsPath.Clear();
    }
}
