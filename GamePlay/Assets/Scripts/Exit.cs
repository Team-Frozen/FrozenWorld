using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //void Update()
    //{
    //    //Player의 이동방향이 Exit로 향할 때
    //    if (player.GetDirection() == Vector3.right)
    //    {
    //        //Player와 Exit가 충돌했을 때, StageClear Panel 띄움
    //        if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.6f, layerMask_player) && Player.canMove)
    //        {
    //            gameManager.ShowStageClearUI();
    //        }
    //    }
    //}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && player.GetDirection() == Vector3.right)
        {
            gameManager.ShowStageClearUI();
        }
    }
}
