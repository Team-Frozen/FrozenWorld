using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrow : Element
{
    private Vector3 blockDir;

    private void Awake()
    {
        if (this.transform.position.y > 1)
            this.transform.localScale = new Vector3(0.7f, 0, 0.7f);
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
        player.MoveToCenter(player.transform.position.y);
        player.SetVelocityZero();

        player.SetDirection(blockDir);
        
        if (player.CheckMove())
            player.TryMove(blockDir);
        else
            player.SetCanMove(true);
    }

    public override BlockType ReturnType()
    {
        return BlockType.ARW;
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.GetChild(0).Rotate(0.0f, 0.0f, -90.0f * (property + 1), Space.Self);
        blockDir = new Vector3((1 - property) * (1 - (property % 2)), 0, (property - 2) * (property % 2));
    }
}