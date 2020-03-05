using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    /*[SerializeField] */ public GameObject StageClearUI;    //Clear 했을 때 나타나는 Panel
    [SerializeField] List<Text> txt_Score;  // 점수
    [SerializeField] Button btn_Back;       // 뒤로가기 버튼
    [SerializeField] Text txt_PlayerMoves;  // Player 이동 횟수
    public static int playerMoves;
    private SaveLoadManager saveload;

    private void Awake()
    {
        InitStage();
        Database.Stage.SetActive(true);
    }

    //Player 이동횟수 업데이트
    void Update()
    {
        txt_PlayerMoves.text = playerMoves.ToString();
    }

    //Stage Clear 했을 때 나타나는 UI(Panel)
    public void ShowStageClearUI()
    {
        Player.canMove = false;
        btn_Back.interactable = false;
        StageClearUI.SetActive(true);
        
        //점수 계산
        Database.Stage.GetComponent<Stage>().CalcScore();
        for (int i = 0; i < Database.Stage.GetComponent<Stage>().GetScore(); i++)
        {
            txt_Score[i].gameObject.SetActive(true);
        }

        //StageClear 저장
        //saveload.Save_ClearData();
    }

    //StageClear UI에서 NextStage 버튼 클릭 시
    public void Change_NextStage()
    {
        //다음 Stage가 같은 Chapter인 경우
        if (Database.FocusStage + 1 < Database.Stages.Count)
        {
            Database.Stage.SetActive(false);
            OpenNextStageBtn();
            InitStage();
            Database.FocusStage++;
            Database.Stage.SetActive(true);
        }
        //다음 Stage가 다른 Chapter인 경우
        else if (Database.FocusChapter + 1 < Database.Chapters.Count)
        {
            Database.Stage.SetActive(false);
            Database.IsClearChapter = true;
            OpenNextStageBtn();
            InitStage();
            Database.FocusChapter++;
            Database.FocusStage = 0;
            Database.Stage.SetActive(true);
        }
        //모든 Stage를 클리어했을 경우
        else
        {
            Database.IsClearChapter = true;
            OpenNextStageBtn(); //save
            InitStage();
            ChangeScene_Chapters();
        }
    }

    private void OpenNextStageBtn()
    {
        //다음 Stage가 같은 Chapter인 경우
        if (Database.FocusStage + 1 < Database.Stages.Count)
        {
            Database.Btn_AllStages[Database.FocusChapter][Database.FocusStage + 1].GetComponent<Button>().interactable = true;

        }
        //다음 Stage가 다른 Chapter인 경우
        else if (Database.FocusChapter + 1 < Database.Chapters.Count)
        {
            Database.Btn_Chapters[Database.FocusChapter + 1].GetComponent<Button>().interactable = true;
            Database.Btn_AllStages[Database.FocusChapter + 1][0].GetComponent<Button>().interactable = true;
        }
        //모든 stage를 깼을 경우
        else
        {

        }
    }

    private void InitStage()
    {
        playerMoves = 0;
        Player.canMove = true;
        btn_Back.interactable = true;
        Database.Player.GetComponent<Player>().MoveToInitPos();
        StageClearUI.SetActive(false);
    }

    public void ChangeScene_Stages()
    {
        if (Database.Stage.GetComponent<Stage>().IsClear())
            OpenNextStageBtn();

        Database.Stage.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Chapters()
    {
        Database.Stage.SetActive(false);
        SceneManager.LoadScene("2_Chapters");
    }

    public void ChangeScene_Start()
    {
        Database.Stage.SetActive(false);
        SceneManager.LoadScene("1_Start");
    }
}