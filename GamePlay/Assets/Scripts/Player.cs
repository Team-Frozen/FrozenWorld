using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static bool canMove = true;
    [SerializeField ] float moveSpeed = 30f;
    Stage stage;
    Rigidbody rigid;
    RaycastHit hit;
    Vector3 moveDir;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        stage = GameObject.FindGameObjectWithTag("Stage").GetComponent<Stage>();
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
            //else if(Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layer_Slope) && (GetDirection() == Vector3.right))
            //{
            //    StageManager.playerMoves++;
            //    TryMove(GetDirection());
            //}
        }
    }

    private bool CheckMove()
    {
        int layerMask_exit = 1 << LayerMask.NameToLayer("Exit");
        int layerMask_obstacle = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("OriginalBlock") | 1 << LayerMask.NameToLayer("SlopeBlock"));

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

    public void MoveToTarget(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, 1f);
    }

    public bool isReachedToCenter(Vector3 target)
    {
        if (GetDirection() == Vector3.left && transform.position.x < target.x && target.x - 0.1f < transform.position.x)
            return true;
        else if (GetDirection() == Vector3.right && transform.position.x > target.x)
            return true;
        else if (GetDirection() == Vector3.forward && this.transform.position.z > target.z)
            return true;
        else if (GetDirection() == Vector3.back && this.transform.position.z < target.z)
            return true;
        else
            return false;
    }

    public void SetPosToCenter()
    {
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
