using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Element
{
    private bool canMove = true;
    private bool onSlope = false;

    [SerializeField]
    private float moveSpeed;
    private Vector3 moveDir;    //이동 방향
    private Rigidbody rigid;
    private Animator ani;

    private void Awake()
    {
        ani = transform.GetChild(0).GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
        rigid = GetComponent<Rigidbody>();
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
                move(moveDir);
        }

        if (rigid.velocity.y < -0.7 && !onSlope) //떨어질 때 y가속도가 붙는 걸 이용해서 떨어지는 중인 걸 체크 
        {
            string underBlock = isOnLayer();
            
            if (transform.position.x * moveDir.x > CalcCenterPos(transform.position.x) * moveDir.x || //떨어져야 되는 칸의 중앙보다 더 갔을 때
                transform.position.z * moveDir.z > CalcCenterPos(transform.position.z) * moveDir.z) {
                rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                MoveToCenter(transform.position.y); // x, z Freeze + MoveToCenter
            }

            if (underBlock != "null") //떨어지는 중에 밑에 어떤 블록이 있다면 == 바닥이라면
            {
                rigid.constraints = RigidbodyConstraints.FreezeRotation; //회전만 Freeze
                
                if (underBlock == "GameArea") //아무 블록도 없다면 TryMove
                {
                    MoveToCenter(-2);
                    if (CheckMove())
                        TryMove(moveDir);
                    else
                        canMove = true;
                }
                //블록이 있다면 그냥 계속 떨어지면서 Action 메서드 실행
            }
        }
    }
    void Update()
    {
        ani.SetBool("isMoving", rigid.velocity == Vector3.zero ? false : true);
    }

    public void move(Vector3 direction)
    {
        if (CheckMove())
        {
            canMove = false;
            GameManager.playerMoves++;
            TryMove(direction);
            GameManager.updateMoves();
        }
    }

    public bool CheckMove() //다음 칸으로 움직일 수 있는지 체크
    {
        //ORG(SLP 포함), WALL
        if (Physics.Raycast(transform.position, this.moveDir, out hit, 0.6f, layerMask_obstacle))
            return false;
        //EXIT
        //else if (Physics.Raycast(transform.position, this.moveDir, out hit, 0.6f, layerMask_exit))
        //    return true;
        //ARW, PRT, STP, EXIT
        else
            return true;
    }

    public void TryMove(Vector3 dir)
    {
        SetDirection(dir);
        rigid.AddForce(dir * moveSpeed, ForceMode.Impulse);     // 가속
    }

    public void initUnitImage()
    {
        int dir = (Database.Stage.GetComponent<Stage>().GetPlayerProperty() + 1) % 4 + 1;

        switch (dir)
        {
            case 1:
                moveDir = new Vector3(0, 0, 1);
                break;
            case 2:
                moveDir = new Vector3(1, 0, 0);
                break;
            case 3:
                moveDir = new Vector3(0, 0, -1);
                break;
            case 4:
                moveDir = new Vector3(-1, 0, 0);
                break;
        }

        setDirImg();
    }

   //바닥에 있는지 확인
    public string isOnLayer()
    {
        var ray = new Ray(this.transform.position, Vector3.down);
        var maxDistance = 0.6f;
        
        if (!Physics.Raycast(ray, out hit, maxDistance))
            return "null";
        return hit.transform.tag;
    }

    //초기 위치로 변환
    public void MoveToInitPos()
    {
        transform.position = Database.Stage.GetComponent<Stage>().GetPlayerPos();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    //한 칸의 중앙으로 위치 변환
    public void MoveToCenter(float yPos)
    {
        transform.position = new Vector3(CalcCenterPos(transform.position.x), yPos, CalcCenterPos(transform.position.z));
    }
       
    //한 칸의 중앙 위치값 계산
    private float CalcCenterPos(float point)
    {
        return Mathf.Floor(point - (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f + 0.5f) + (Database.Stage.GetComponent<Stage>().GetStageSize() + 1) % 2 * 0.5f;
    }

    private void setDirImg()
    {
        if (moveDir.z == 1)
            ani.SetInteger("direction", 1);
        else if (moveDir.x == 1)
            ani.SetInteger("direction", 2);
        else if (moveDir.z == -1)
            ani.SetInteger("direction", 3);
        else if (moveDir.x == -1)
            ani.SetInteger("direction", 4);
    }

    //player의 속도를 0으로 변환
    public void SetVelocityZero()
    {
        rigid.velocity = Vector3.zero;
        SetDirection(Vector3.zero);
    }

    public override void Action(Player player) { }

    // getter, setter
    public void SetDirection(Vector3 dir)
    {
        moveDir = dir;
        setDirImg();
    }

    public Vector3 GetDirection()   {   return moveDir; }
    public bool GetCanMove()        {   return canMove; }
    public void SetCanMove(bool canMove)    {   this.canMove = canMove; }
    public Rigidbody GetRigidbody()         {   return rigid;   }
    public void SetOnSlope(bool onSlope)    {   this.onSlope = onSlope; }
    
    public override BlockType ReturnType()
    {
        return BlockType.UNIT;
    }

    public override void setProperty(int property)
    {
        this.property = property;
    }
}
