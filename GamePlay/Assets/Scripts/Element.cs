using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected int property;
    protected RaycastHit hit;
    protected int layerMask_player, layerMask_wall, layerMask_exit, layerMask_obstacle;

    protected Vector3 position_vec;

    void Start()
    {
        Debug.Log("element start");

        layerMask_player = 1 << LayerMask.NameToLayer("Player");
        layerMask_wall = 1 << LayerMask.NameToLayer("Wall");

        position_vec = new Vector3(transform.position.x, 1, transform.position.z);
    }

    public abstract BlockType ReturnType();
    public abstract void Action(Player player);

    public virtual void setProperty(int property)
    {
        this.property = property;
    }
}
