using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlope : Element
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        //정방향으로 충돌했을 경우
        if (Physics.Raycast(transform.position, new Vector3((1 - property) * (1 - (property % 2)), 0, (property - 2) * (property % 2)), out hit, 1f, layerMask_player))
        {
            player.GetComponent<Rigidbody>().AddForce(player.GetDirection() * 25, ForceMode.Impulse);
        }
        //옆면에 충돌했을 경우
        else if (Physics.Raycast(transform.position, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            player.SetVelocityZero();
            player.MoveToCenter(player.transform.position.y);
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
