using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static bool canMove = true;
    Rigidbody rigid;
    RaycastHit hit;
    [HideInInspector] public Vector3 direction;
    [SerializeField] float moveSpeed;
    int layerMask;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        layerMask = (1 << LayerMask.NameToLayer("Block")) | (1 << LayerMask.NameToLayer("Wall"));
    }
    
    void FixedUpdate()
    {
        //속도가 0일 때만 이동
        if (canMove && (rigid.velocity == Vector3.zero))
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            
            SetDirection(new Vector3(h, 0, v));

            //수직 또는 수평 키를 누른 경우 & 이동하는 방향에 물체가 없는 경우 이동
            if ((h * v == 0) && !(h == 0 && v == 0) && !Physics.Raycast(transform.position, direction, out hit, 0.5f, layerMask))
            {
                TryMove(h, v);   
            }
        }
    }

    void TryMove(float h, float v)
    {
        //움직인 횟수 + 1
        StageManager.playerMoves++;

        //player 이동 방향으로 회전
        Quaternion q = Quaternion.LookRotation(new Vector3(-v, 0, h));
        transform.rotation = q;

        //player 가속
        rigid.AddForce(direction * moveSpeed, ForceMode.Impulse);
    }

    public void SetVelocityZero()
    {
        //SetDirection(Vector3.zero);
        rigid.velocity = Vector3.zero;
    }

    void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
