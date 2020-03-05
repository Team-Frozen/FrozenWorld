using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStop : Element
{
    private Collider player;

    private void Update()
    {
        if (player != null && player.GetComponent<Player>().isReachedToTarget(position_vec))
        {
            Action(player.GetComponent<Player>());
            player = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other;
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
        return BlockType.STP;
    }
}
