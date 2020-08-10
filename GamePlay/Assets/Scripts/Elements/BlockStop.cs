using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStop : Element
{
    private void Awake()
    {
        if (this.transform.position.y > 1)
            this.transform.localScale = new Vector3(0.6f, 0, 0.6f);
    }

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
        player.SetCanMove(true);
    }

    public override BlockType ReturnType()
    {
        return BlockType.STP;
    }
}