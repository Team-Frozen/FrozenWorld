using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOriginal : Element
{
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("halo");
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(transform.position, -other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_player))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        player.SetVelocityZero();
        player.MoveToCenter();
        Player.canMove = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.ORG;
    }
}
