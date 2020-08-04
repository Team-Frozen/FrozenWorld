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

            // 올라갈 때
            if (player.GetDirection() == -blockDir)
            {
                player.GetComponent<Rigidbody>().velocity = -blockDir * 3.0f;
            }
            // 내려갈 때
            else if(player.GetDirection() == blockDir)
            {
                
            }
            // 에러
            else
                Debug.Log("SLP error");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("SLP exit");

            Player player = other.gameObject.GetComponent<Player>();
            player.GetComponent<Player>().setOnSlope(false);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //player.TryMove(player.GetDirection());

            // 내려갈 때만 중심으로
            if (player.GetDirection() == blockDir)
            {
                player.MoveToCenter(-2);
            }

            if (player.CheckMove())
                player.TryMove(player.GetDirection());
            else
                Player.canMove = true;
        }
    }

    public override void Action(Player player)
    {

        if (player.GetDirection() == blockDir)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Player>().setOnSlope(true);
            Debug.Log("SLP");
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
