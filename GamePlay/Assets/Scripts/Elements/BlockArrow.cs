using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrow : Element
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
        Debug.Log("arrow left");

        player.MoveToCenter();

        player.SetVelocityZero();
        player.TryMove(new Vector3((1 - property) * (1 - (property % 2)), 0, (property - 2) * (property % 2)));
    }

    public override BlockType ReturnType()
    {
        return BlockType.ARW;
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.Rotate(0.0f, 90.0f * property, 0.0f, Space.Self);
    }
}