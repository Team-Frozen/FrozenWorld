using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(player.transform.position, player.GetDirection(), out hit, 1f, layerMask_wall))
        {
            Debug.Log("wall");

            player.SetVelocityZero();
            player.SetPosToCenter();
            Player.canMove = true;
        }
    }
}
