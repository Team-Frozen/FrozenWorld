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
        Player.canMove = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.STP;
    }
}

//-------------------------------BoxCollider Size (1,1,1)일때---------------------------------//
//private Collider player;

//private void Update()
//{
//    if (player != null && player.GetComponent<Player>().isReachedToTarget(position_vec))
//    {
//        Debug.Log("in2");
//        Action(player.GetComponent<Player>());
//        player = null;
//    }
//}

//void OnTriggerEnter(Collider other)
//{
//    if (other.gameObject.CompareTag("Player"))
//    {
//        Debug.Log("in1");
//        player = other;
//    }
//}

//public override void Action(Player player)
//{
//    player.SetVelocityZero();
//    player.MoveToCenter();
//    Player.canMove = true;
//}

//public override BlockType ReturnType()
//{
//    return BlockType.STP;
//}