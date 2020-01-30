using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameManager manager;
    public Vector3 direction;
    Rigidbody rigid;
    Vector3 view;
    float maxVelocity = 20f;
    float h, v;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //속도가 0일 때만 이동
        if (rigid.velocity == Vector3.zero)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            //수직, 수평 동시에 눌린 경우 제외
            if (h == 0 || v == 0)
            {
                direction = new Vector3(h, 0, v);       //player의 실제 이동 방향 설정
                view = new Vector3(-v, 0, h);           //(수정필요)player가 바라보는 방향 설정
                rigid.AddForce(direction, ForceMode.Impulse);
            }
        }
        //player, maxVelocity까지 가속
        else if (rigid.velocity.sqrMagnitude < maxVelocity)
        {
            //player 회전
            Quaternion q = Quaternion.LookRotation(view);
            transform.rotation = q;

            //player 가속
            rigid.AddForce(direction, ForceMode.Impulse);
        }
    }

    public void SetVelocityZero()
    {
        rigid.velocity = Vector3.zero;
        manager.playerMoves++;
    }
}
