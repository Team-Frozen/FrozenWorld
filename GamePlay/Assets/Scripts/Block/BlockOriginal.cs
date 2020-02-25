using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOriginal : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(transform.position, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            Debug.Log("original");

            player.SetVelocityZero();
            player.SetPosToCenter();
            Player.canMove = true;
        }
    }
}
