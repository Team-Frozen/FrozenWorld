using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalBlock : MonoBehaviour
{
    RaycastHit hit;
    public Player player;
    int layerMask_player;

    void Start()
    {
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        
    }

    //Player와 충돌 시 속도 0
    void OnCollisionEnter(Collision other)
    {
        if (Physics.Raycast(transform.position, -player.direction, out hit, 0.5f, layerMask_player))
        {
            player.SetVelocityZero();
        }
    }
}
