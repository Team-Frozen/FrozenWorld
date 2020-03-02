using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStop : Element
{
    bool isAction;

    private void Update()
    {
        if (!isAction && player.isReachedToTarget(position_vec))
        {
            Action();
            isAction = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAction = false;
        }
    }

    public override void Action()
    {
        Debug.Log("STOP");

        player.SetVelocityZero();
        player.MoveToCenter();
        Player.canMove = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.STP;
    }
}
