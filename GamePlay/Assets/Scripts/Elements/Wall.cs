using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(other.transform.position, other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_wall) && hit.transform.gameObject == this.gameObject)
        {
            Action(other.transform.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        GameObject elementOnPos = Database.Stage.GetComponent<Stage>().GetElementOn(player.transform.position);
        if (elementOnPos == null || elementOnPos.GetComponent<Element>().ReturnType() != BlockType.SLP)
        {
            player.SetVelocityZero();
            player.MoveToCenter(player.transform.position.y);
            Player.canMove = true;
        }
        //SLP 위에서 벽 충돌
        else
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.SetDirection(-player.GetDirection());
            //Quaternion q = Quaternion.LookRotation(new Vector3(-player.GetDirection().z, player.GetDirection().y, player.GetDirection().x));
            //player.transform.rotation = q;
        }
    }

    public override BlockType ReturnType()
    {
        return BlockType.WALL;
    }
}