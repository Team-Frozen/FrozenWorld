using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerStages : MonoBehaviour
{
//------------ Prefabs--------------//
    public  GameObject button;      //
    public  GameObject canvas;      //
    public  GameObject ghostButton; //
    public  GameObject indicateBar; //
//----------------------------------//
    public  GameObject mainCanvas;

    private const int  STAGES_IN_A_ROW = 7;
    private Vector3 INIT_POSITION { get { return new Vector3(50, -100); } }
    private const int  INTERVAL = 100;
    private int        clickedBtnIndex;
    private GameObject focusButton;
    private GameObject ghostBtn;
    private GameObject indicator;

    void Awake()
    {
        focusButton = null;

        //Choose Which Canvas To Activate
        if (Test.Canvases.Count > Test.FocusChapter)
            Test.Canvas.SetActive(true);
        else
            Test.Canvases.Add(Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity));

        //Add Listener To All Btns
        foreach (GameObject Button in Test.Btn_Stages)
        {
            Button.GetComponent<Button>().onClick.AddListener(checkBtnState);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GraphicRaycaster gr = Test.Canvas.GetComponent<GraphicRaycaster>();
        GraphicRaycaster m_gr = mainCanvas.GetComponent<GraphicRaycaster>();

        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();    //Btn_LoadStage모아 놓은 Canvas에서 쏘는 Ray에 맞는 UI 저장
        List<RaycastResult> m_results = new List<RaycastResult>();  //기존 Canvas에서 쏘는 Ray에 맞는 UI 저장

        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDown) //마우스 클릭했을 때
        {
            clickedBtnIndex = -1;

            m_gr.Raycast(ped, m_results);
            gr.Raycast(ped, results);
            if (isRaycastResultNull(results) && isRaycastResultNull(m_results)) //Ray에 아무것도 맞지 않았을 때
            {
                if (focusButton != null)
                    focusButton.GetComponent<Image>().color = Color.white;
                focusButton = null;
            }
            else if(isRaycastResultNull(m_results)) //Ray에 Btn_LoadStage가 맞았을 때
            {
                int i;

                for (i = 0; Test.Btn_Stages[i] != results[0].gameObject; i++) ;

                clickedBtnIndex = i;
            }
        }

        if(m_Event.type == EventType.MouseDrag)
        {
           if(clickedBtnIndex != -1)
            {
                if (ghostBtn != null)
                    ghostBtn.transform.position = Input.mousePosition;
                else
                {
                    ghostBtn = Instantiate(ghostButton, Input.mousePosition, Quaternion.identity);
                    ghostBtn.transform.SetParent(mainCanvas.transform, false);
                }

                if (indicator != null)
                    indicator.GetComponent<RectTransform>().anchoredPosition = new Vector3(calcBtnMovePos() % STAGES_IN_A_ROW * 100 + 45, -(calcBtnMovePos() / STAGES_IN_A_ROW) * 100 - 95, 0);
                else {
                    indicator = Instantiate(indicateBar, new Vector3(calcBtnMovePos() % STAGES_IN_A_ROW * 100 + 45, - (calcBtnMovePos() / STAGES_IN_A_ROW) * 100 - 95, 0), Quaternion.identity);
                    indicator.transform.SetParent(mainCanvas.transform, false);
                }
            }
        }

        if(m_Event.type == EventType.MouseUp)
        {
            if(clickedBtnIndex != -1)
            {
                GameObject temp = Test.Stages[clickedBtnIndex];


                if (focusButton != null)
                    focusButton.GetComponent<Image>().color = Color.white;

                if (clickedBtnIndex > calcBtnMovePos())
                {
                    Test.Stages.RemoveAt(clickedBtnIndex);
                    Test.Stages.Insert(calcBtnMovePos(), temp);
                    focusButton = Test.Btn_Stages[calcBtnMovePos()];
                }
                else if (clickedBtnIndex < calcBtnMovePos() - 1)
                {
                    Test.Stages.Insert(calcBtnMovePos(), temp);
                    Test.Stages.RemoveAt(clickedBtnIndex);
                    focusButton = Test.Btn_Stages[calcBtnMovePos() - 1];
                }

                if(focusButton != null)
                    focusButton.GetComponent<Image>().color = Color.cyan;

                if (ghostBtn != null)
                {
                    Destroy(ghostBtn);
                    ghostBtn = null;
                }

                if (indicator != null)
                {
                    Destroy(indicator);
                    indicator = null;
                }
            }
        }
    }

    private int calcBtnMovePos() //return 0 ~ Btn_Stages.Count - 1 마우스 위치에 따른 List index 위치
    {
        int index_X, index_Y;

        if (Input.mousePosition.x >= 895)
            index_X = STAGES_IN_A_ROW;
        else
            index_X = (int)((Input.mousePosition.x + 5) / 100);

        if (Input.mousePosition.y > Screen.height - INIT_POSITION.y)
            index_Y = 0;
        else if (Input.mousePosition.y <= Test.Btn_Stages[Test.Btn_Stages.Count - 1].transform.position.y - 95)
            index_Y = (int)((Screen.height - Test.Btn_Stages[Test.Btn_Stages.Count - 1].transform.position.y - 50)/ 100 - 1) * STAGES_IN_A_ROW;
        else
            index_Y = ((Screen.height - (int)Input.mousePosition.y - 5) / 100 - 1) * STAGES_IN_A_ROW;

        if (index_X + index_Y > Test.Btn_Stages.Count)
            return Test.Btn_Stages.Count;
        return index_X + index_Y;
    }

    public void checkBtnState() //Btn_Load_Stage 눌렀을 때 실행하는 함수
    {
        var colors = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors;
        
        if (focusButton == null || focusButton != EventSystem.current.currentSelectedGameObject)
        {
            if (focusButton != null)
                focusButton.GetComponent<Image>().color = Color.white;
            focusButton = EventSystem.current.currentSelectedGameObject;
            focusButton.GetComponent<Image>().color = Color.cyan;
        }
        else
            loadStage();
    }


    private void loadStage()
    {
        int index;

        for (index = 0; Test.Btn_Stages[index] != focusButton; index++) ;

        Test.FocusStage = index;
        focusButton.GetComponent<Image>().color = Color.white;
        ChangeScene_MapEdit();

        Debug.Log("minmove: " +  Test.Stage.GetComponent<Stage>().getMinMove());
    }

    public void newStage() //Btn_Create 눌렀을 때 실행하는 함수 Button GameObject만들고 Scene넘김 - GameArea는 넘어간 Scene에서 생성
    {
        GameObject Button;

        if (focusButton == null)
            Test.FocusStage = Test.Stages.Count;
        else {
            int index;
            for (index = 0; Test.Btn_Stages[index] != focusButton; index++) ;
            Test.FocusStage = index + 1;
            focusButton.GetComponent<Image>().color = Color.white;
        }
        Button = Instantiate(button, calcStagePosition(Test.Btn_Stages.Count), Quaternion.identity);
        Button.transform.SetParent(Test.Canvas.transform, false);

        Test.Btn_Stages.Add(Button);

        ChangeScene_MapEdit();
    }

    private Vector3 calcStagePosition(int stageNum)
    {
        return INIT_POSITION + new Vector3(INTERVAL * (stageNum % STAGES_IN_A_ROW), -INTERVAL * (stageNum / STAGES_IN_A_ROW));
    }

    private void ChangeScene_MapEdit()
    {
        Test.Canvas.SetActive(false);
        SceneManager.LoadScene("4_MapEdit");
    }

    public void ChangeScene_Chapters()
    {
        if (focusButton != null)
            focusButton.GetComponent<Image>().color = Color.white;
        if (Test.Btn_Stages.Count != 0)
            Test.Canvas.SetActive(false);
        SceneManager.LoadScene("2_Chapters");
    }

    public void removeStage() //Btn_Remove 눌렀을 때 실행하는 함수
    {
        if (focusButton != null)
        {
            int index;

            for (index = 0; Test.Btn_Stages[index] != focusButton; index++) ;

            Destroy(Test.Btn_Stages[Test.Btn_Stages.Count - 1]);
            Test.Btn_Stages.RemoveAt(Test.Btn_Stages.Count - 1);

            Destroy(Test.Stages[index]);
            Test.Stages.RemoveAt(index);

            focusButton.GetComponent<Image>().color = Color.white;
            focusButton = null;
        }
    }

    private bool isRaycastResultNull(List<RaycastResult> results)
    {
        foreach (RaycastResult result in results)
            if (result.gameObject.GetComponent<Button>())
                return false;
        return true;
    }
}