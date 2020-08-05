using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOriginal : Element
{
    void OnCollisionEnter(Collision other)
    {
        //옆면에 충돌한 경우
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(transform.position, -other.transform.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_player))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        player.SetVelocityZero();
        player.MoveToCenter(player.transform.position.y);
        Player.canMove = true;
    }
    public override void setProperty(int property)
    {
        this.property = property;
        if(property == 1)
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("platform/block/OrgBlock2", typeof(Sprite));
    }

    public override BlockType ReturnType()
    {
        return BlockType.ORG;
    }
}
