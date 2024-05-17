using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> markers;
    private GameObject marker;
    private Node node;
    public Node Node { get { return node; } set { node = value; } }
    // Start is called before the first frame update
    void Start()
    {
        node.Tile = this;
        marker = markers[0];
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
            GraphManager.Instance.ChangeNode(node);
            GetComponent<Renderer>().material.color = node.NodeType == 0 ? Color.black : Color.white;
        }
    }
    public void ShowMarker(bool toActive, MarkerType markerType = MarkerType.None)
    {
        marker.gameObject.SetActive(false);

        marker = markers[(int)markerType];
        marker.gameObject.SetActive(toActive);
    }
    public void SetMarkerColor(Color color)
    {
        marker.GetComponent<Renderer>().material.color = color;
    }
    public void RotateArrow(Vector3 rotation)
    {
        markers[(int)MarkerType.Arrow].transform.localEulerAngles = rotation;
    }
}