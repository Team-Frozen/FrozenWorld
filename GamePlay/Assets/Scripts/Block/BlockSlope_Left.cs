﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlope_Left : Element       //leftSlope Block
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 1f, layerMask_player))
            {
                Debug.Log("Slope");

                player.GetComponent<Rigidbody>().AddForce(player.GetDirection() * 10, ForceMode.Impulse);
            }
            else if(Physics.Raycast(transform.position, -player.GetDirection(), out hit, 1f, layerMask_player))
            {
                player.SetVelocityZero();
                player.SetPosToCenter();
                Player.canMove = true;
            }
        }
    }
}