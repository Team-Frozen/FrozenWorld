using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] Player player;
    [SerializeField] StageManager stageManager;
    int layerMask_player;

    void Start()
    {
        layerMask_player = 1 << LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        //Player의 이동방향이 Exit로 향할 때
        if (player.direction == Vector3.back)
        {
            //Player와 Exit가 충돌했을 때, StageClear Panel 띄움
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 0.6f, layerMask_player))
            {
                stageManager.ShowStageClearUI();
            }
        }
    }    
}
