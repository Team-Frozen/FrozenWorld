using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(other.transform.position, other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_wall) && hit.transform.gameObject == this.gameObject)
        {
            Action(other.transform.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        Database.Player.GetComponent<Player>().SetVelocityZero();
        Database.Player.GetComponent<Player>().MoveToCenter();
        Player.canMove = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.WALL;
    }
}