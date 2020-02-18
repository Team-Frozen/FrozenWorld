using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Element
{
    [SerializeField] StageManager stageManager;

    void Update()
    {
        //Player의 이동방향이 Exit로 향할 때
        if (player.GetDirection() == Vector3.back)
        {
            //Player와 Exit가 충돌했을 때, StageClear Panel 띄움
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 0.5f, layerMask_player) && Player.canMove)
            {
                stageManager.ShowStageClearUI();
            }
        }
    }

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Player") && player.GetDirection() == Vector3.back)
    //    {
    //        stageManager.ShowStageClearUI();
    //    }
    //}
}
