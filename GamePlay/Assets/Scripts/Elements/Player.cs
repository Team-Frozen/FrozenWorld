using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Element
{
    public static bool canMove = true;
    public static bool isCollide = false;

    [SerializeField]
    private float moveSpeed;
    private Vector3 moveDir;    //이동 방향
    private Rigidbody rigid;

    private GameObject underBlock;      //player 아래에 있는 블록
    private GameObject nextBlock;       //player 이동 방향에 있는 블록

    private Animator ani;

    private void Awake()
    {
        ani = transform.GetChild(0).GetComponent<Animator>();
        moveSpeed = 50;
        DontDestroyOnLoad(gameObject);
        rigid = GetComponent<Rigidbody>();
        initUnitImage();
        layerMask_exit = 1 << LayerMask.NameToLayer("Exit");
        layerMask_slope = 1 << LayerMask.NameToLayer("SlopeBlock");
        layerMask_obstacle = (1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("OriginalBlock") | 1 << LayerMask.NameToLayer("SlopeBlock"));
    }

    private void FixedUpdate()
    {
        if (canMove && !SettingData.ControlMode_Button)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            SetDirection(new Vector3(h, 0, v));

            //이동 (수평/수직 동시에 이동 불가)
            if ((h * v == 0) && !(h == 0 && v == 0))
                move(GetDirection());
        }

        if (isCollide)
        {
            Debug.Log("Player.Update()...");

            if (GetDirection() != Vector3.zero) //WALL에 부딪힌 경우 제외
            {
                if (isReachedToTarget(underBlock.GetComponent<Element>().GetPosition() + GetDirection()))  //다음 칸으로 왔다면 내려감 (ARW, STP, PRT, NULL인 경우만)
                {
                    rigid.velocity = Vector3.zero;
                    rigid.velocity = Vector3.down * 1.5f;

                    if (isOnFloor())    //바닥에 닿았을 때
                    {
                        rigid.velocity = Vector3.zero;
                        isCollide = false;

                        if (nextBlock == null)
                            TryMove(GetDirection());
                        else
                            nextBlock = null;
                    }
                }
            }
            
        }

        ani.SetBool("isMoving", rigid.velocity == Vector3.zero ? false : true);
        if (moveDir.z == 1)
            ani.SetInteger("direction", 1);
        else if (moveDir.x == 1)
            ani.SetInteger("direction", 2);
        else if (moveDir.z == -1)
            ani.SetInteger("direction", 3);
        else if (moveDir.x == -1)
            ani.SetInteger("direction", 4);
    }

    public void move(Vector3 direction)
    {
        if (CheckMove())
        {
            canMove = false;
            GameManager.playerMoves++;

            //ORG위에서 벽에 부딪혔을 경우 다시 움직일 때를 위한 Setting
            SetUnderBlock(this.transform.position);
            SetNextBlock(this.transform.position + GetDirection());

            TryMove(direction);
        }
    }

    private bool CheckMove()
    {
        //SLP
        if (Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layerMask_slope))
        {
            int propertySlope = Database.Stage.GetComponent<Stage>().GetElementOn(this.transform.position + this.GetDirection()).GetComponent<Element>().getProperty();

            if (this.GetDirection() == new Vector3((propertySlope - 1) * (1 - (propertySlope % 2)), 0, (2 - propertySlope) * (propertySlope % 2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //ORG, WALL
        else if (Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layerMask_obstacle))
            return false;
        //EXIT
        else if(Physics.Raycast(transform.position, this.GetDirection(), out hit, 1f, layerMask_exit))
            return true;
        else
            return true;
    }

    public void TryMove(Vector3 dir)
    {
        SetDirection(dir);
        //player 가속
        rigid.AddForce(dir * moveSpeed, ForceMode.Impulse);
    }

    public void initUnitImage()
    {
        ani.SetInteger("direction", (property + 2) % 4);
    }

    // target 위치에 도달했는지 검사 (한 칸의 중앙에 위치했는지 검사)
    public bool isReachedToTarget(Vector3 target)
    {
        if (Mathf.Abs(transform.position.x - target.x) < 0.05f && Mathf.Abs(transform.position.z - target.z) < 0.05f)
            return true;
        else
            return false;
    }

    //바닥에 있는지 확인
    public bool isOnFloor()
    {
        var ray = new Ray(this.transform.position, Vector3.down);
        var maxDistance = 0.6f;
        return Physics.Raycast(ray, maxDistance, -1);
    }

    //초기 위치로 변환
    public void MoveToInitPos()
    {
        transform.position = Database.Stage.GetComponent<Stage>().GetPlayerPos();
    }

    //한 칸의 중앙으로 위치 변환
    public void MoveToCenter()
    {
        transform.position = new Vector3(CalcCenterPos(transform.position.x), transform.position.y, CalcCenterPos(transform.position.z));
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

    public void SetDirection(Vector3 dir)
    {
        moveDir = dir;
    }

    public Vector3 GetDirection()
    {
        return moveDir;
    }

    public void SetNextBlock(Vector3 blockPos)
    {
        nextBlock = Database.Stage.GetComponent<Stage>().GetElementOn(blockPos);
    }

    public void SetUnderBlock(Vector3 blockPos)
    {
        underBlock = Database.Stage.GetComponent<Stage>().GetElementOn(blockPos);
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
    }
}