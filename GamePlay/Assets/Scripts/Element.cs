using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    [SerializeField]
    protected Player player;

    protected RaycastHit hit;
    protected int layerMask_player, layerMask_wall, layerMask_exit, layerMask_obstacle;

    protected Vector3 position_vec;

    void Start()
    {
        position_vec = new Vector3(transform.position.x, 1, transform.position.z);
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
        layerMask_wall = 1 << LayerMask.NameToLayer("Wall");
        layerMask_exit = 1 << LayerMask.NameToLayer("Exit");
        layerMask_obstacle = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("OriginalBlock") | 1 << LayerMask.NameToLayer("SlopeBlock"));
    }

    public abstract BlockType ReturnType();
    public abstract void Action();
}
