using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOriginal : Element
{
    void OnCollisionEnter(Collision other)
    {
        //옆면에 충돌한 경우
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(transform.position, -other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_player))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
        //Block위에서 충돌한 경우
        else if (other.gameObject.CompareTag("Player") && Physics.Raycast(transform.position + Vector3.up, -other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_player))
        {
            other.gameObject.GetComponent<Player>().SetUnderBlock(position_vec);
            other.gameObject.GetComponent<Player>().SetNextBlock(position_vec + other.gameObject.GetComponent<Player>().GetDirection()) ;  //player.update()에서 nextblock에 따른 움직임 구현
            Player.isCollide = true;
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
