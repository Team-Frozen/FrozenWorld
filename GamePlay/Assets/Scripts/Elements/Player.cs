using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Element
{
    public static bool canMove = true;

    [SerializeField]
    private float moveSpeed;
    private Rigidbody rigid;
    //private Vector3 initPos;    // 처음 위치
    private Vector3 moveDir;    // 이동 방향

    private void Awake()
    {
        moveSpeed = 50;
        DontDestroyOnLoad(gameObject);
        rigid = GetComponent<Rigidbody>();
        layerMask_exit = 1 << LayerMask.NameToLayer("Exit");
        layerMask_obstacle = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("OriginalBlock") | 1 << LayerMask.NameToLayer("SlopeBlock"));
    }

    private void FixedUpdate()
    {
        if (canMove && !SettingData.ControlMode_Button)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            SetDirection(new Vector3(h, 0, v));

            // 이동
            if ((h * v == 0) && !(h == 0 && v == 0))
                move(GetDirection());
        }
    }

    public void move(Vector3 direction)
    {
        if (CheckMove())
        {
            canMove = false;
            GameManager.playerMoves++;
            TryMove(direction);
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

        //player 이동 방향으로 회전 +수정
        Quaternion q = Quaternion.LookRotation(new Vector3(-dir.z, dir.y, dir.x));
        transform.rotation = q;

        //player 가속
        rigid.AddForce(dir * moveSpeed, ForceMode.Impulse);
    }

    // target 위치에 도달했는지 검사 (한 칸의 중앙에 위치했는지 검사)
    public bool isReachedToTarget(Vector3 target)
    {
        if (Mathf.Abs(transform.position.x - target.x) < 0.05f && Mathf.Abs(transform.position.z - target.z) < 0.05f)
            return true;
        else
            return false;
    }

    //초기 위치로 변환
    public void MoveToInitPos()
    {
        transform.position = Database.Stage.GetComponent<Stage>().GetPlayerPos();
    }

    //한 칸의 중앙으로 위치 변환
    public void MoveToCenter()
    {
        transform.position = new Vector3(CalcCenterPos(transform.position.x), 1, CalcCenterPos(transform.position.z));
    }
       
    //한 칸의 중앙 위치값 계산
    private float CalcCenterPos(float point)
    {
        return Mathf.Floor(point - (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f + 0.5f) + (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f;
    }

    //player의 속도를 0으로 변환
    public void SetVelocityZero()
    {
        rigid.velocity = Vector3.zero;
        SetDirection(Vector3.zero);
    }

    //public void SetInitPos(Vector3 pos)
    //{
    //    initPos = pos;
    //}

    public void SetDirection(Vector3 dir)
    {
        moveDir = dir;
    }

    public Vector3 GetDirection()
    {
        return moveDir;
    }

    public override void Action(Player player) 
    {

    }

    public override BlockType ReturnType()
    {
        return BlockType.UNIT;
    }

    public override void setProperty(int property)
    {
        this.property = property;
        this.transform.Rotate(0.0f, 90.0f * property, 0.0f, Space.Self);
    }
}