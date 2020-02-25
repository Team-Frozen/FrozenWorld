using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Element
{
    [SerializeField] BlockPortal relatedPortal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Physics.Raycast(position_vec, -player.GetDirection(), out hit, 1f, layerMask_player))
        {
            Debug.Log("portal");

            //Player의 위치를 ArrowBlock의 위치로 변환
            player.SetPosToCenter();
            player.transform.position += player.GetDirection();

            //Player를 짝지어진 Portal로 이동시키기
            player.transform.position = relatedPortal.position_vec;
        }
    }
}
