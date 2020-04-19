using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected RaycastHit hit;
    protected int layerMask_player, layerMask_wall, layerMask_exit, layerMask_obstacle, layerMask_slope;

    protected Vector3 position_vec; //element position
    protected int property;

    void Start()
    {
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
        layerMask_wall = 1 << LayerMask.NameToLayer("Wall");

        position_vec = new Vector3(transform.position.x, 1, transform.position.z);
    }

    public abstract BlockType ReturnType();
    public abstract void Action(Player player);

    public Vector3 GetPosition()
    {
        return position_vec;
    }

    public int getProperty()
    {
        return this.property;
    }

    public virtual void setProperty(int property)
    {
        this.property = property;
    }
}
