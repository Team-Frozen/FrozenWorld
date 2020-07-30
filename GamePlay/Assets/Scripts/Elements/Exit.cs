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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action(other.gameObject.GetComponent<Player>());
        }
    }

    public override void Action(Player player)
    {
        player.SetVelocityZero();

        Database.Stage.GetComponent<Stage>().SetIsClear(true);
        if (Database.FocusStage + 1 == Database.Stages.Count)
            Database.Chapter.SetIsClear(true);

        isCollide = true;
    }

    public override BlockType ReturnType()
    {
        return BlockType.EXIT;
    }
}