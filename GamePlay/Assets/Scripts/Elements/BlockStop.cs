using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStop : Element
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        player.SetVelocityZero();
        player.MoveToCenter(player.transform.position.y);
        player.SetCanMove(true);
    }

    public override BlockType ReturnType()
    {
        return BlockType.STP;
    }
}