using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    Vector3 dragOrigin;
    void PanCamera()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;

        if (Input.GetMouseButtonDown(2))
            dragOrigin = mainCamera.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(mousePos);
            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + difference);
        }
    }
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = mainCamera.orthographicSize * mainCamera.aspect;

        if (GraphManager.Instance.Graph == null)
            return targetPosition;

        int rowCount = GraphManager.Instance.Graph.Nodes.GetLength(0);
        int columnCount = GraphManager.Instance.Graph.Nodes.GetLength(1);
        Tile topLeftTile = GraphManager.Instance.Graph.Nodes[0,0].Tile;
        Tile bottomRightTile = GraphManager.Instance.Graph.Nodes[rowCount-1,columnCount-1].Tile;

        float minX = topLeftTile.transform.position.x - 5 + camWidth;
        float maxX = bottomRightTile.transform.position.x + 5 - camWidth;
        float minY = bottomRightTile.transform.position.z - 10 + camHeight;
        float maxY = topLeftTile.transform.position.z - camHeight;

        float newX = minX >= maxX ? 0 : Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = minY >= maxY ? 0 : Mathf.Clamp(targetPosition.z, minY, maxY);
        return new Vector3(newX, mainCamera.transform.position.y, newY);
    }
    private void Update()
    {
        PanCamera();
    }
}
