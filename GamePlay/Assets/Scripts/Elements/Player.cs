using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Element
{
    public static bool canMove = true;
    [SerializeField ] float moveSpeed = 30f;
    Rigidbody rigid;
    Vector3 moveDir;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //속도가 0일 때만 이동
        if (canMove && (rigid.velocity == Vector3.zero))
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            SetDirection(new Vector3(h, 0, v));

            //수직 또는 수평 키를 누른 경우 & 이동하려는 방향에 장애물이 없는 경우 이동
            if ((h * v == 0) && !(h == 0 && v == 0) && CheckMove())
            {
                canMove = false;
                GameManager.playerMoves++;
                TryMove(GetDirection());
            }
        }
    }

    private bool CheckMove()
    {
        // slope block 옆면 충돌시 false 추가해야 함 //
        if (Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layerMask_exit))
            return true;
        else if (Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layerMask_obstacle))
            return false;
        else
            return true;
    }

    public void TryMove(Vector3 dir)
    {
        SetDirection(dir);

        //player 이동 방향으로 회전
        Quaternion q = Quaternion.LookRotation(new Vector3(-dir.z, dir.y, dir.x));
        transform.rotation = q;

        //player 가속
        rigid.AddForce(dir * moveSpeed, ForceMode.Impulse);
    }

    public bool isReachedToTarget(Vector3 target)
    {
        if (Mathf.Abs(transform.position.x - target.x) < 0.05f && Mathf.Abs(transform.position.z - target.z) < 0.05f)
            return true;
        else
            return false;
    }

    public void SetPosToCenter()
    {
        transform.position = new Vector3(CalcCenterPos(transform.position.x), 1, CalcCenterPos(transform.position.z));
    }

    private float CalcCenterPos(float point)
    {
        return Mathf.Floor(point - (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f + 0.5f) + (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f;
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

    public override void Action() 
    {

    }

    public override BlockType ReturnType()
    {
        return BlockType.UNIT;
    }

}
