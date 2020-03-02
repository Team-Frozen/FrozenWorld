using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManagerChapter : MonoBehaviour
{
 
//-------------Prefabs--------------//
    public GameObject btn_Chapters; //
    public GameObject ghostButton;  //
//----------------------------------//
    public GameObject mainCanvas;
    public GameObject pan_Confirm;

    private int         clickedBtnIndex;
    private GameObject  focusButton;
    private GameObject  ghostBtn;
    private bool        GUIActivate;
    private float       mousePos;
    private int         page;
    private float       timeCounter;

    void Awake()
    {
        GUIActivate = true;
        pan_Confirm.SetActive(false);
        page = Test.FocusChapter / 3;
        clickedBtnIndex = -1;

        setChptrLoc(0f);

        if (Test.Btn_Chapters.Count != 0)
            Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(true);

        //Add Listener To All Btns
        foreach (GameObject Button in Test.Btn_Chapters)
        {
            Button.GetComponent<Button>().onClick.AddListener(checkBtnState);
        }

    }

    void OnGUI()
    {
        if (GUIActivate)
        {
            GraphicRaycaster gr = GameObject.Find("Canvas_Load_Chapters").GetComponent<GraphicRaycaster>();
            GraphicRaycaster m_gr = mainCanvas.GetComponent<GraphicRaycaster>();

            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();    //Btn_LoadStage모아 놓은 Canvas에서 쏘는 Ray에 맞는 UI 저장
            List<RaycastResult> m_results = new List<RaycastResult>();  //기존 Canvas에서 쏘는 Ray에 맞는 UI 저장

            Event m_Event = Event.current;
            
            if (clickedBtnIndex != -1)
            {
                if ((Input.mousePosition.x > 1000 && page < (Test.Btn_Chapters.Count - 1) / 3))
                {
                    timeCounter += Time.deltaTime;
                    if (timeCounter > 2)
                    {
                        page++;
                        setChptrLoc(0f);
                        timeCounter = 0;
                    }
                }
                else if ((Input.mousePosition.x < 24 && page > 0))
                {
                    timeCounter -= Time.deltaTime;
                    if (timeCounter < -2)
                    {
                        page--;
                        setChptrLoc(0f);
                        timeCounter = 0;
                    }
                }
                else
                    timeCounter = 0;
            }

            //Mouse Pressed
            if (m_Event.type == EventType.MouseDown)
            {
                m_gr.Raycast(ped, m_results);
                gr.Raycast(ped, results);
                if (isRaycastResultNull(results) && isRaycastResultNull(m_results)) //Ray에 아무것도 맞지 않았을 때
                {
                    mousePos = Input.mousePosition.x;

                    if (focusButton != null)
                        focusButton.GetComponent<Image>().color = Color.white;
                    focusButton = null;
                }
                else if (isRaycastResultNull(m_results)) //Ray에 Btn_LoadChapter가 맞았을 때 (main 이 null일 때)
                {
                    int i;
                    for (i = 0; Test.Btn_Chapters[i] != results[0].gameObject; i++) ;

                    clickedBtnIndex = i;
                }
            }

            //Mouse Draged
            if (m_Event.type == EventType.MouseDrag)
            {
                if (clickedBtnIndex != -1)
                {
                    if (ghostBtn != null)
                        ghostBtn.transform.position = Input.mousePosition;
                    else
                    {
                        ghostBtn = Instantiate(ghostButton, Input.mousePosition, Quaternion.identity);
                        ghostBtn.transform.SetParent(mainCanvas.transform, false);
                    }

                    //추가
                }
                else
                {
                    float diff = Input.mousePosition.x - mousePos;
                    if (Mathf.Abs(diff) <= 150)
                        setChptrLoc(diff);
                }
            }

            //Mouse Released
            if (m_Event.type == EventType.MouseUp)
            {
                if (clickedBtnIndex != -1)
                {
                    List<GameObject> temp = Test.Chapters[clickedBtnIndex];
                    List<GameObject> tempBtnStage = Test.Btn_AllStages[clickedBtnIndex];
                    GameObject tempCan = Test.Canvases[clickedBtnIndex];

                    if (clickedBtnIndex > calcBtnMovePos())
                    {
                        Test.Chapters.RemoveAt(clickedBtnIndex);
                        Test.Chapters.Insert(calcBtnMovePos(), temp);

                        Test.Canvases.RemoveAt(clickedBtnIndex);
                        Test.Canvases.Insert(calcBtnMovePos(), tempCan);

                        Test.Btn_AllStages.RemoveAt(clickedBtnIndex);
                        Test.Btn_AllStages.Insert(calcBtnMovePos(), tempBtnStage);
                    }
                    else if (clickedBtnIndex < calcBtnMovePos() - 1)
                    {
                        Test.Chapters.Insert(calcBtnMovePos(), temp);
                        Test.Chapters.RemoveAt(clickedBtnIndex);

                        Test.Canvases.Insert(calcBtnMovePos(), tempCan);
                        Test.Canvases.RemoveAt(clickedBtnIndex);

                        Test.Btn_AllStages.Insert(calcBtnMovePos(), tempBtnStage);
                        Test.Btn_AllStages.RemoveAt(clickedBtnIndex);
                    }

                    if (ghostBtn != null)
                    {
                        Destroy(ghostBtn.gameObject);
                        ghostBtn = null;
                    }
                }
                else
                {
                    float diff = Input.mousePosition.x - mousePos;

                    if (Mathf.Abs(diff) > 150)
                    {
                        if (diff < 0 && page < (Test.Btn_Chapters.Count - 1) / 3)
                            page++;
                        else if (diff > 0 && page > 0)
                            page--;
                    }

                    setChptrLoc(0f);
                }
                clickedBtnIndex = -1;
            }
        }
    }

    public void loadChapter()
    {
        int index;

        for (index = 0; Test.Btn_Chapters[index] != EventSystem.current.currentSelectedGameObject; index++) ;
        Test.FocusChapter = index;
        ChangeScene_Stages();
    }

    private void setChptrLoc(float diff)
    {
        int i = 0;
        foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
        {
            Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) + diff - (900 * page), 0, 0);
            i++;
        }
    }

    public void showPopUp()
    {
        if (focusButton != null)
        {
            pan_Confirm.SetActive(true);
            GUIActivate = false;
        }
    }

    public void hidePopUp()
    {
        pan_Confirm.SetActive(false);
        GUIActivate = true;
    }

    public void removeChptr()
    {
        if (focusButton != null)
        {
            int index;

            for (index = 0; Test.Btn_Chapters[index] != focusButton; index++) ;

            Destroy(Test.Btn_Chapters[Test.Btn_Chapters.Count - 1]);
            Test.Btn_Chapters.RemoveAt(Test.Btn_Chapters.Count - 1);

            foreach(GameObject Stage in Test.Chapters[index])
                Destroy(Stage);
            Test.Chapters.RemoveAt(index);

            Test.Btn_AllStages.RemoveAt(index);

            Destroy(Test.Canvases[index]);
            Test.Canvases.RemoveAt(index);

            focusButton.GetComponent<Image>().color = Color.white;
            focusButton = null;
        }
        if (page > (Test.Btn_Chapters.Count - 1) / 3)
        {
            page--;
            setChptrLoc(0f);
        }
        hidePopUp();
    }

    public void newChapter()
    {
        GameObject Button;

        Button = Instantiate(btn_Chapters, new Vector3(87 + 300 * (Test.Btn_Chapters.Count) - (900 * page), 0, 0), Quaternion.identity);// 수정
        Button.transform.SetParent(GameObject.Find("Canvas_Load_Chapters").transform, false);
        Test.Btn_Chapters.Add(Button);

        Test.FocusChapter = Test.Chapters.Count;
        Test.Chapters.Add(new List<GameObject>());
        Test.Btn_AllStages.Add(new List<GameObject>());

        ChangeScene_Stages();
    }

    private void ChangeScene_Stages()
    {
        if (focusButton != null)
            focusButton.GetComponent<Image>().color = Color.white;
        Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(false);
        GUIActivate = false;
        SceneManager.LoadScene("Stages");
    }

    private int calcBtnMovePos() //return 0 ~ Btn_Chapters.Count - 1 마우스 위치에 따른 List index 위치
    {
        int index_X;

        index_X = (int)((Input.mousePosition.x + 87) / 300 + (page * 3));
        
        if (index_X > Test.Btn_Chapters.Count)
            return Test.Btn_Stages.Count;
        return index_X;
    }

    public void checkBtnState() //Btn_Load_Stage 눌렀을 때 실행하는 함수
    {
        if (GUIActivate)
        {
            var colors = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors;

        if (focusButton == null || focusButton != EventSystem.current.currentSelectedGameObject)
        {
            if (focusButton != null)
                focusButton.GetComponent<Image>().color = Color.white;
            focusButton = EventSystem.current.currentSelectedGameObject;
            focusButton.GetComponent<Image>().color = Color.red;
        }
        else
            loadChapter();
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
