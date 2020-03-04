using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        //Debug.Log(player.GetDirection().x + " " + player.GetDirection().y + " " + player.GetDirection().z);
        if (player.GetDirection() == Vector3.right)
        {
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.6f, layerMask_player))
            {
                Action();
            }
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && player.GetDirection() == Vector3.right)
    //    {
    //        Action();
    //    }
    //}

    public override void Action()
    {
        Debug.Log("EXIT");

        Database.Stage.GetComponent<Stage>().SetIsClear(true);
        Debug.Log(Database.Stage.GetComponent<Stage>().IsClear());
        gameManager.ShowStageClearUI();
    }

    public override BlockType ReturnType()
    {
        return BlockType.EXIT;
    }
}