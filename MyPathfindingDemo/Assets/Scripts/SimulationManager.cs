using Assets.Scripts;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{

    [SerializeField]
    private GraphManager graphManager;
    [SerializeField]
    GameObject player;
    [SerializeField]
    ThirdPersonController controller;
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
