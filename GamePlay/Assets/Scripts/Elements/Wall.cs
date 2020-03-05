using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(Database.Player.transform.position, Database.Player.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_wall))
        {
            Action(other.transform.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        Debug.Log("wall");

        Database.Player.GetComponent<Player>().SetVelocityZero();
        Database.Player.GetComponent<Player>().MoveToCenter();
        Player.canMove = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.WALL;
    }
}