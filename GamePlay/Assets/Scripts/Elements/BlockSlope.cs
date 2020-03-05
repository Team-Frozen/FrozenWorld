using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlope : Element       //leftSlope Block
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        if (Physics.Raycast(transform.position, new Vector3(-(property / 2) * (1 - (property % 2)), 0, (property - 2) * (property % 2)), out hit, 1f, layerMask_player))
        {
            player.GetComponent<Rigidbody>().AddForce(player.GetDirection() * 10, ForceMode.Impulse);
        }
        else if (Physics.Raycast(transform.position, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            player.SetVelocityZero();
            player.MoveToCenter();
            Player.canMove = true;
        }
    }

    public override BlockType ReturnType()
    {
        return BlockType.SLP;
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.Rotate(0.0f, 90.0f * property, 0.0f, Space.Self);
    }
}
