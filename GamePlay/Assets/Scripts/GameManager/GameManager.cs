using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public GameObject StageClearUI;     //Clear 했을 때 나타나는 Panel
    public List<Image> img_Score;       //점수
    public Button btn_Back;             //뒤로가기 버튼
    public Text txt_PlayerMoves;        //Player 이동 횟수

    public static int playerMoves;
    private Exit exit;

    private void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.GAMEPLAY_BGM);

        InitStage();
        Database.Stage.SetActive(true);
        exit = Database.Stage.GetComponent<Stage>().GetElements()[1].GetComponent<Exit>();
    }

    //Player 이동횟수 업데이트
    void Update()
    {
        if (exit.isCollide)
        {
            exit.isCollide = false;
            ShowStageClearUI();
        }

        txt_PlayerMoves.text = playerMoves.ToString();
    }

    //Stage Clear 했을 때 나타나는 UI(Panel)
    public void ShowStageClearUI()
    {
        Player.canMove = false;
        btn_Back.interactable = false;
        StageClearUI.SetActive(true);

        Database.Stage.GetComponent<Stage>().CalcScore();   //점수 계산
        SaveLoadManager.Save_ClearData();   //StageClear 저장

        for (int i = 0; i < Database.Stage.GetComponent<Stage>().GetCurrentScore(); i++)
        {
            img_Score[i].gameObject.SetActive(true);    //Panel에 점수표시
        }

        Database.Stage.GetComponent<Stage>().SetActiveStageScore();     //StageScene에서 점수 표시
    }

    //StageClear UI에서 NextStage 버튼 클릭 시
    public void Btn_NextStage()
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
        exit = Database.Stage.GetComponent<Stage>().GetElements()[1].GetComponent<Exit>();
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

        img_Score[0].gameObject.SetActive(false);
        img_Score[1].gameObject.SetActive(false);
        img_Score[2].gameObject.SetActive(false);
        StageClearUI.SetActive(false);
    }

    public void ChangeScene_Stages()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        if (Database.Stage.GetComponent<Stage>().IsClear())
            OpenNextStageBtn();

        Database.Stage.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Stage.SetActive(false);
        SceneManager.LoadScene("2_Chapters");
    }

    public void Btn_RestartStage()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        InitStage();
    }
}