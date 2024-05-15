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
    private Tile startingTile;
    [SerializeField]
    private Tile targetTile;

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
        Vector3 topleft = transform.position - Vector3.right * size.rows / 2 + Vector3.up * size.columns / 2;
        for (int i = 0; i < graph.Nodes.GetLength(0); i++)
        {
            for (int j = 0; j < graph.Nodes.GetLength(1); j++)
            {
                Vector3 position = topleft + Vector3.right * (j) + Vector3.up * (1 - i);
                GameObject go = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = go.GetComponent<Tile>();
                tile.Node = graph.Nodes[i,j];
                go.GetComponent<Renderer>().material.color = tile.Node.NodeType == 0 ? Color.black : Color.white;
            }
        }
    }
    private List<Node> bfsPath = new List<Node>();
    public void SetAlgorithmPoint(Tile tile)
    {
        if (tile != null && tile.Node.NodeType == 1)
        {
            if (startingTile == null)
            {
                startingTile = tile;
                startingTile.ShowMarker(true);
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
                targetTile.ShowMarker(true);
                targetTile.SetMarkerColor(Color.cyan);
            }
        }
        ClearPath();
        CalculatePath();
    }
    private void CalculatePath()
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
                    Vector3 difference = nextPathNode.Tile.transform.position - pathNode.Tile.transform.position;
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
                pathNode.Tile.ShowArrow(true);
                pathNode.Tile.SetArrowColor(Color.green);
            }
        }
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
            pathNode.Tile.ShowArrow(false);
        }
        bfsPath.Clear();
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
        else if(node.Tile == targetTile)
        {
            targetTile.ShowMarker(false);
            targetTile = null;
        }
        ClearPath();
        CalculatePath();
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
}
