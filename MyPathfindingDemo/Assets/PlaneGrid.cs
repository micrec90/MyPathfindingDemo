using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneGrid : MonoBehaviour
{
    [SerializeField]
    private float radius;
    private float diameter;
    private Vector2Int size;
    private Graph planeGraph;

    private void Start()
    {
        diameter = 2 * radius;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Node node = NodeFromWorldPoint();
            planeGraph.ChangeNode(node);
        }
    }
    public void InitializeGrid(Graph graph)
    {
        size = new Vector2Int(graph.Nodes.GetLength(0), graph.Nodes.GetLength(1));

        planeGraph = graph;
    }
    private Node NodeFromWorldPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        Vector3 worldPosition = (Vector3)Camera.main.ScreenToWorldPoint(mousePos);
        float percentX = (worldPosition.x + size.x / 2) / size.x;
        float percentY = (1 - worldPosition.y + size.y / 2) / size.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((size.x - 1) * percentX);
        int y = Mathf.RoundToInt((size.y - 1) * percentY);

        return planeGraph.Nodes[y, x];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 1, size.y));
        if (planeGraph == null)
            return;
        Node mousePosition = NodeFromWorldPoint();
        for (int i = 0; i < planeGraph.Nodes.GetLength(0); i++)
        {
            for (int j = 0; j < planeGraph.Nodes.GetLength(1); j++)
            {
                Vector3 topleft = transform.position - Vector3.right * size.x / 2 + Vector3.forward * size.y / 2;
                Vector3 position = topleft + Vector3.right * (j * diameter + radius) + Vector3.forward * (1 - i * diameter + radius);
                Gizmos.color = planeGraph.Nodes[i,j].NodeType != 0 ? Color.white : Color.black;
                if (mousePosition == planeGraph.Nodes[i, j])
                    Gizmos.color = Color.cyan;
                Gizmos.DrawCube(position, Vector3.one * (diameter-.2f));
            }
        }
    }
}
