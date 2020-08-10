using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Element
{
    Vector3 position_vec;
    private BlockPortal linkedPortal;
    private bool isActive = false;

    void Awake()
    {
        position_vec = new Vector3(transform.position.x, Mathf.Floor(transform.position.y - 2), transform.position.z);
        
        if (this.transform.position.y > 1)
            this.transform.localScale = new Vector3(0.8f, 0, 0.8f);
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

    public void SetColor(int var)
    {
        switch (var)
        {
            case 0:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                break;
            case 1:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
                break;
            case 2:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 255, 100);
                break;
            default:
                break;
        }
    }
}