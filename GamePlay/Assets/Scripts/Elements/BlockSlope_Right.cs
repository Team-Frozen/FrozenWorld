using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlope_Right : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action();
        }
    }

    public override void Action()
    {
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 1f, layerMask_player))
        {
            Debug.Log("Slope Right");

            player.GetComponent<Rigidbody>().AddForce(player.GetDirection() * 10, ForceMode.Impulse);
        }
        else if (Physics.Raycast(transform.position, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            player.SetVelocityZero();
            player.MoveToCenter();
            Player.canMove = true;
        }
    }

    public override BlockType ReturnType()
    {
        return BlockType.SLP;
    }
}
