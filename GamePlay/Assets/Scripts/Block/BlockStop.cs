using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStop : Element
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.SetVelocityZero();
            //수정. 중앙까지 이동필요
            //player.MoveToTarget(position_vec);
            
            player.SetPosToCenter();
            Player.canMove = true;
        }
    }
}
