using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameObject marker;
    private Node node;
    public Node Node { get { return node; } set { node = value; } }
    // Start is called before the first frame update
    void Start()
    {
        node.Tile = this;
        marker.GetComponent<Renderer>().material.color = Color.green;
    }
    // OnMouseDown is only triggered by left click
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GraphManager.Instance.SetAlgorithmPoint(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GraphManager.Instance.Graph.ChangeNode(node);
            GetComponent<Renderer>().material.color = node.NodeType == 0 ? Color.black : Color.white;
        }
    }
    public void ShowMarker(bool toActive)
    {
        marker.gameObject.SetActive(toActive);
    }
}
