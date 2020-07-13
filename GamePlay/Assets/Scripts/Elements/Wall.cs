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

        //정상적인 벽 충돌
        if(elementOnPos == null)
        {
            player.SetVelocityZero();
            player.MoveToCenter();
            Player.canMove = true;
        }
        //ORG 위에서 벽 충돌
        else if (elementOnPos.GetComponent<Element>().ReturnType() == BlockType.ORG)
        {
            player.SetVelocityZero();
            player.MoveToCenter();
            Player.canMove = true;
        }
        //SLP 위에서 벽 충돌
        else if (elementOnPos.GetComponent<Element>().ReturnType() == BlockType.SLP)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.SetDirection(-player.GetDirection());

            player.setUnitImage();
            //Quaternion q = Quaternion.LookRotation(new Vector3(-player.GetDirection().z, player.GetDirection().y, player.GetDirection().x));
            //player.transform.rotation = q;
        }
    }

    public override BlockType ReturnType()
    {
        return BlockType.WALL;
    }
}