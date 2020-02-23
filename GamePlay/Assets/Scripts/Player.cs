using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static bool canMove = true;
    [SerializeField ]float moveSpeed = 30f;
    Stage stage;
    Rigidbody rigid;
    RaycastHit hit;
    Vector3 moveDir;
    int layer_Wall, layer_Slope;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        stage = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();
        layer_Wall = ((1 << LayerMask.NameToLayer("Wall")) | 1 << LayerMask.NameToLayer("OriginalBlock")) | (1 << LayerMask.NameToLayer("SlopeBlock"));
        layer_Slope = 1 << LayerMask.NameToLayer("SlopeBlock");
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
            if ((h * v == 0) && !(h == 0 && v == 0) && !Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layer_Wall))
            {
                GameManager.playerMoves++;
                TryMove(GetDirection());
            }
            //else if(Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layer_Slope) && (GetDirection() == Vector3.right))
            //{
            //    StageManager.playerMoves++;
            //    TryMove(GetDirection());
            //}
        }
    }

    public void TryMove(Vector3 dir)
    {
        SetVelocityZero();

        //player 이동 방향으로 회전
        Quaternion q = Quaternion.LookRotation(new Vector3(-dir.z, dir.y, dir.x));
        transform.rotation = q;

        //player 가속
        rigid.AddForce(dir * moveSpeed, ForceMode.Impulse);
    }

    public void SetPosToCenter()
    {
        Debug.Log("v = 0");
        transform.position = new Vector3(CalcCenterPos(transform.position.x), 0, CalcCenterPos(transform.position.z));
    }

    float CalcCenterPos(float point)
    {
        return Mathf.Floor(point - (stage.GetStageSize() + 1) % 2 * 0.5f + 0.5f) + (stage.GetStageSize() + 1) % 2 * 0.5f;
    }

    public void SetVelocityZero()
    {
        rigid.velocity = Vector3.zero;
    }

    public void SetDirection(Vector3 dir)
    {
        moveDir = dir;
    }

    public Vector3 GetDirection()
    {
        return moveDir;
    }
}
