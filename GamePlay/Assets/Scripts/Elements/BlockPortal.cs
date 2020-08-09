﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Element
{
    Vector3 position_vec;
    private BlockPortal linkedPortal;
    private bool isActive = false;

    void Start()
    {
        position_vec = new Vector3(transform.position.x, Mathf.Floor(transform.position.y + 1), transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.gameObject.CompareTag("Player"))
        {
            isActive = true;
            linkedPortal.isActive = true;
            Action(other.GetComponent<Player>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        isActive = false;
    }

    public override void Action(Player player)
    {
        player.transform.position = linkedPortal.position_vec;
        player.GetRigidbody().velocity = Vector3.zero;

        if (player.CheckMove())
            player.TryMove(player.GetDirection());
        else
            player.SetCanMove(true);
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