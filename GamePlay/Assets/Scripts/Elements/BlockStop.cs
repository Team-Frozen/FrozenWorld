using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockStop : Element
{
    private void Awake()
    {
        if (this.transform.position.y > 1)
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("platform/block/StopBlock2", typeof(Sprite));
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