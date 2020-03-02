using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrow_Left : Element
{
    bool isAction = true;

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
        Debug.Log("arrow left");

        player.MoveToCenter();

        player.SetVelocityZero();
        player.TryMove(Vector3.left);
    }

    public override BlockType ReturnType()
    {
        return BlockType.ARW;
    }
}