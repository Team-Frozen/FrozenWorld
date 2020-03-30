using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum BlockType
    {
        NULL,
        ORG,
        ARW,
        SLP,
        STP,
        PRT
    }
    private BlockType blockType;
    private GameObject ghostBlock;  //Block before click
    private GameObject focusBlock;  //Clicked block
    private Vector3 mouseDownPoint;

//----------- Prefabs-------------//
    public GameObject unit;       //
    public GameObject exit;       //
    public GameObject stage;      //
    public GameObject orgGhost;   //
    public GameObject orgBlock;   //
    public GameObject arwGhost;   //
    public GameObject arwBlock;   //
    public GameObject slpGhost;   //
    public GameObject slpBlock;   //
    public GameObject stpGhost;   //
    public GameObject stpBlock;   //
    public GameObject prtGhost;   //
    public GameObject prtBlock;   //
//--------------------------------//

    public ToggleGroup ToggleGroup;
    public Button btn_size;
    public Button btn_minMove;

    // Start is called before the first frame update
    void Awake()
    {
        btn_size.onClick.AddListener(resizeMap);//수정 수정 가능한지 생각해보기
        btn_minMove.onClick.AddListener(setMinMove);

        if (Test.Stages.Count == Test.Btn_Stages.Count) // stage load btn click (버튼의 수가 늘지 않았을 때)
            Test.Stage.SetActive(true);
        else // new btn click
        {
            if (Test.Stages.Count == Test.FocusStage)
                Test.Stages.Add(Instantiate(stage, new Vector3(0, 0, 0), Quaternion.identity));
            else
                Test.Stages.Insert(Test.FocusStage, Instantiate(stage, new Vector3(0, 0, 0), Quaternion.identity));
            setUnit();
            setExit();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getSelectedBlockType();
    }
    void OnGUI()
    {
        Event m_Event = Event.current;
        int layerMask = 1 << LayerMask.NameToLayer("MouseTarget");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 100f, layerMask);
        
        //--------마우스 클릭 (맵 안에서)----------//
        if (m_Event.type == EventType.MouseDown)
        {
            mouseDownPoint = Input.mousePosition;
            //블럭을 클릭한 경우와 그렇지 않은 경우
            if (onBlock(hitInfo))
                focusBlock = hitInfo.transform.gameObject;
            else if(ghostBlock)
            {
                switch (blockType)
                {
                    case BlockType.ORG:
                        focusBlock = Instantiate(orgBlock, ghostBlock.transform.position, Quaternion.identity);
                        break;
                    case BlockType.ARW:
                        focusBlock = Instantiate(arwBlock, ghostBlock.transform.position, Quaternion.identity);
                        break;
                    case BlockType.SLP:
                        focusBlock = Instantiate(slpBlock, ghostBlock.transform.position, Quaternion.identity);
                        focusBlock.AddComponent<SlpBlock>();
                        focusBlock.AddComponent<BoxCollider>();
                        focusBlock.layer = 8;
                        break;
                    case BlockType.STP:
                        focusBlock = Instantiate(stpBlock, ghostBlock.transform.position, Quaternion.identity);
                        break;
                    case BlockType.PRT:
                        focusBlock = Instantiate(prtBlock, ghostBlock.transform.position, Quaternion.identity);

                        GameObject linkedBlock;
                        Vector3 vaildPos = new Vector3(0, 0.5f, 0);
                        int stageSize = Test.Stage.GetComponent<Stage>().getStageSize();

                        for (int i = 0; i < stageSize; i++)
                        {
                            vaildPos.z = -stageSize / 2.0f + 0.5f + i;
                            for (int j = 0; j < stageSize; j++)
                            {
                                vaildPos.x = -stageSize / 2.0f + 0.5f + j;
                                if (!Test.Stage.GetComponent<Stage>().hasElementOn(vaildPos))
                                {
                                    linkedBlock = Instantiate(prtBlock, vaildPos, Quaternion.identity);

                                    linkedBlock.GetComponent<PrtBlock>().setLinkedPrt(focusBlock.GetComponent<PrtBlock>());
                                    focusBlock.GetComponent<PrtBlock>().setLinkedPrt(linkedBlock.GetComponent<PrtBlock>());

                                    Test.Stage.GetComponent<Stage>().getElements().Add(linkedBlock);
                                    Test.Stage.GetComponent<Stage>().setParent(linkedBlock);

                                    goto label;
                                }
                            }
                        }
                        label:
                        break;
                }
                Test.Stage.GetComponent<Stage>().getElements().Add(focusBlock);
                Test.Stage.GetComponent<Stage>().setParent(focusBlock);

                Destroy(ghostBlock);
                focusBlock = null;
            }
        }
        //-----------------------------------------//

        //-------------마우스 드래그---------------//
        if (m_Event.type == EventType.MouseDrag)
        {
            if (focusBlock != null)
            {
               focusBlock.transform.position = new Vector3(calcCrd(hitInfo.point.x), 0.5f + (focusBlock.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z));

                if (focusBlock.GetComponent<Element>().inValidArea(Test.Stage.GetComponent<Stage>()))
                    focusBlock.GetComponent<Element>().setVisible();
                else
                    focusBlock.GetComponent<Element>().setInvisible();
            }
        }

        //--------------마우스 놓음----------------//
        if (m_Event.type == EventType.MouseUp)
        {
            if (focusBlock && mouseDownPoint == Input.mousePosition)
            {
                focusBlock.GetComponent<Element>().changeProperty();
            }
            if (focusBlock && !focusBlock.GetComponent<Element>().inValidArea(Test.Stage.GetComponent<Stage>()))
            {
                if (focusBlock.GetComponent<Element>().returnType() == global::BlockType.PRT)   //PRT일 경우 linked PRT 제거
                    focusBlock.GetComponent<PrtBlock>().getLinkedPrt().deleteElement();

                focusBlock.GetComponent<Element>().deleteElement();
            }
            
            focusBlock = null;
        }

        //--------------마우스 이동----------------//
        if (inValidArea(hitInfo))
        {
            if (ghostBlock == null)
            {
                switch (blockType)
                {
                    case BlockType.ORG:
                        ghostBlock = Instantiate(orgGhost, new Vector3(calcCrd(hitInfo.point.x), 0.5f + (orgGhost.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)), Quaternion.identity);
                        break;
                    case BlockType.ARW:
                        ghostBlock = Instantiate(arwGhost, new Vector3(calcCrd(hitInfo.point.x), 0.5f + (arwGhost.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)), Quaternion.identity);
                        break;
                    case BlockType.SLP:
                        ghostBlock = Instantiate(slpGhost, new Vector3(calcCrd(hitInfo.point.x), 0.5f + (slpGhost.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)), Quaternion.identity);
                        ghostBlock.AddComponent<GhostBlock>();
                        break;
                    case BlockType.STP:
                        ghostBlock = Instantiate(stpGhost, new Vector3(calcCrd(hitInfo.point.x), 0.5f + (stpGhost.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)), Quaternion.identity);
                        break;
                    case BlockType.PRT:
                        ghostBlock = Instantiate(prtGhost, new Vector3(calcCrd(hitInfo.point.x), 0.5f + (prtGhost.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)), Quaternion.identity);
                        break;
                }
            }
            else
                ghostBlock.GetComponent<GhostBlock>().move(new Vector3(calcCrd(hitInfo.point.x), 0.5f + (ghostBlock.transform.localScale.y * 0.5f), calcCrd(hitInfo.point.z)));
        }
        else
            Destroy(ghostBlock);       
    }

    public float calcCrd(float point){
        return Mathf.Floor(point - (Test.Stage.GetComponent<Stage>().getStageSize() + 1) % 2 * 0.5f + 0.5f) + (Test.Stage.GetComponent<Stage>().getStageSize() + 1) % 2 * 0.5f;
    }

    public void getSelectedBlockType()
    {
        Toggle selectedToggle = ToggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null)
            blockType = (BlockType)selectedToggle.GetComponent<ToggleHandler>().getBlockType();
        else
            blockType = 0;
    }    

    public bool inValidArea(RaycastHit hitInfo)
    {
        if (inStage(hitInfo) && !onBlock(hitInfo))  //Stage 안에 있고 블럭이 없는 곳
            return true;
        return false;
    }

    public bool onBlock(RaycastHit hitInfo)
    {
        if (!Test.Stage.GetComponent<Stage>().getElements().Any() ||
            !hitInfo.transform.GetComponent<Element>())
            return false;

        foreach (GameObject element in Test.Stage.GetComponent<Stage>().getElements())
        {
            if (hitInfo.transform.position.x == element.transform.position.x &&
                hitInfo.transform.position.z == element.transform.position.z)
                return true;
        }
        return false;
    }

    public bool inStage(RaycastHit hitInfo)
    {
        if (hitInfo.point.x < 0.5f * Test.Stage.GetComponent<Stage>().getStageSize() &&
            hitInfo.point.x > -0.5f * Test.Stage.GetComponent<Stage>().getStageSize() &&
            hitInfo.point.z < 0.5f * Test.Stage.GetComponent<Stage>().getStageSize() &&
            hitInfo.point.z > -0.5f * Test.Stage.GetComponent<Stage>().getStageSize())
            return true;
        return false;
    }

   
    public void ChangeScene_Stages()
    {
        Test.Stage.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    private void setMinMove()
    {
        if (btn_minMove.GetComponent<ButtonHandler>().getText() != Test.Stage.GetComponent<Stage>().getMinMove())
            Test.Stage.GetComponent<Stage>().setMinMove(btn_minMove.GetComponent<ButtonHandler>().getText());
        Debug.Log("setMinMove" + Test.Stage.GetComponent<Stage>().getMinMove());
    }

    private void resizeMap()
    {
        if(btn_size.GetComponent<ButtonHandler>().getText() != Test.Stage.GetComponent<Stage>().getStageSize())
            Test.Stage.GetComponent<Stage>().setStageSize(btn_size.GetComponent<ButtonHandler>().getText());
        setUnit();
        setExit();
    }

    private void setUnit()
    {
        GameObject newUnit;

        newUnit = Instantiate(unit, new Vector3(-(Test.Stage.GetComponent<Stage>().getStageSize() * 0.5f - 0.5f), 0.5f + (unit.transform.localScale.y * 0.5f), Test.Stage.GetComponent<Stage>().getStageSize() * 0.5f - 0.5f), Quaternion.identity);
        Test.Stage.GetComponent<Stage>().getElements().Add(newUnit);
        Test.Stage.GetComponent<Stage>().setParent(newUnit);
    }

    private void setExit()
    {
        GameObject newExit;

        newExit = Instantiate(exit, new Vector3(Test.Stage.GetComponent<Stage>().getStageSize() * 0.5f + 0.5f, 0.5f + (exit.transform.localScale.y * 0.5f), Test.Stage.GetComponent<Stage>().getStageSize() * 0.5f - 0.5f), Quaternion.identity);
        Test.Stage.GetComponent<Stage>().getElements().Add(newExit);
        Test.Stage.GetComponent<Stage>().setParent(newExit);
    }
}