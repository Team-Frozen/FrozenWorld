using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Element
{
    [SerializeField] BlockPortal relatedPortal;
    bool isAction = true;

    private void Update()
    {
        if (!isAction && player.isReachedToTarget(position_vec))
        {
            Action();
            isAction = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(position_vec, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            isAction = false;
        }
    }
    
    public override void Action()
    {
        Debug.Log("portal");

        player.MoveToCenter();
        player.transform.position = relatedPortal.position_vec;
    }

    public override BlockType ReturnType()
    {
        return BlockType.PRT;
    }
}