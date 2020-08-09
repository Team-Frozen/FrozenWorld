using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlope : Element
{
    private Vector3 blockDir;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            // Stay 시, 올라가는 경우
            if (player.GetDirection() == -blockDir)
            {
                player.GetRigidbody().velocity = -blockDir * 3.0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.SetOnSlope(false);
            player.GetRigidbody().velocity = Vector3.zero;

            // Exit 시, 내려가는 경우 (올라가는 경우 제외)
            if (player.GetDirection() == blockDir)
            {
                player.MoveToCenter(-2);
            }

            if (player.CheckMove())
                player.TryMove(player.GetDirection());
            else
                player.SetCanMove(true);
        }
    }

    public override void Action(Player player)
    {
        // 충돌(Enter) 시, 내려가는 경우
        if (player.GetDirection() == blockDir)
        {
            player.GetRigidbody().velocity = blockDir * 3.0f;
            player.GetComponent<Player>().SetOnSlope(true);
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
        blockDir = new Vector3((1 - property) * (1 - (property % 2)), 0, (property - 2) * (property % 2));
    }
}
