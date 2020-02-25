using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrow_Down : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("arrow down");

            //Player의 위치를 ArrowBlock의 위치로 변환
            player.SetPosToCenter();
            player.transform.position += player.GetDirection();

            player.SetVelocityZero();
            player.TryMove(Vector3.back);
        }
    }
}
