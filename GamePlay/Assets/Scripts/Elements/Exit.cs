using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
{
    public bool isCollide { get; set; }

    private void Start()
    {
        isCollide = false;
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
    }

    //void Update()
    //{
    //    if (player.GetDirection() == Vector3.right)
    //    {
    //        if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.6f, layerMask_player))
    //        {
    //            Action();
    //        }
    //    }
    //}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Player>().GetDirection() == Vector3.right)
        {
            Action(other.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        player.SetVelocityZero();
        Database.Stage.GetComponent<Stage>().SetIsClear(true);
        isCollide = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.EXIT;
    }
}