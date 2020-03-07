using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Element
{
    private BlockPortal linkedPortal;
    private Collider player;

    private void Update()
    {
        if (player != null && player.GetComponent<Player>().isReachedToTarget(position_vec))
        {
            Action(player.GetComponent<Player>());
            player = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(position_vec, -other.GetComponent<Player>().GetDirection(), out hit, 1f, layerMask_player))
        {
            player = other;
        }
    }
    
    public override void Action(Player player)
    {
        player.MoveToCenter();
        player.transform.position = linkedPortal.position_vec;
    }

    public override BlockType ReturnType()
    {
        return BlockType.PRT;
    }

    public void SetLinkedPortal(BlockPortal linked)
    {
        linkedPortal = linked;
    }
}