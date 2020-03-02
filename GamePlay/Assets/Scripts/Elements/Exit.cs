using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //void Update()
    //{
    //    Debug.Log(player.GetDirection().x + " " + player.GetDirection().y + " " + player.GetDirection().z);
    //    if (player.GetDirection() == Vector3.right)
    //    {
    //        Debug.Log("in 2");
    //        if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.6f, layerMask_player))
    //        {
    //            Action();
    //        }
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player.GetDirection() == Vector3.right)
        {
            Action();
        }
    }

    public override void Action()
    {
        Debug.Log("EXIT");

        gameManager.ShowStageClearUI();
    }

    public override BlockType ReturnType()
    {
        return BlockType.EXIT;
    }
}
