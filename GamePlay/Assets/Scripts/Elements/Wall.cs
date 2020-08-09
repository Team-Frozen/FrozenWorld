using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Element
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") 
            && Physics.Raycast(other.transform.position, other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_wall)
            && hit.transform.gameObject == this.gameObject)
        {
            Action(other.transform.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        GameObject elementOnPos = Database.Stage.GetComponent<Stage>().GetElementOn(player.transform.position);
        if ((player.isOnLayer() == "GameArea")          // 1층에서 충돌
            || player.isOnLayer() == "OriginalBlock")   // 2층 ORG 위에서 충돌
        {
            player.SetVelocityZero();
            player.MoveToCenter(player.transform.position.y);
            player.SetCanMove(true);
        }
    }

    public override BlockType ReturnType()
    {
        return BlockType.WALL;
    }
}