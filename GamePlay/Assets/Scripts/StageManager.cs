using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] List<Stage> stageList;
    [SerializeField] GameObject go_StageClearUI;
    [SerializeField] Text txt_playerMoves;
    [SerializeField] List<Text> txt_Score;
    Stage focusedStage;
    public static int playerMoves;
    public static int focusedStageNum = 1;
    
    //Scene이 시작할 때마다 실행되는 기본적인 세팅
    void Start()
    {
        playerMoves = 0;
        Player.canMove = true;
        go_StageClearUI.SetActive(false);
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].gameObject.SetActive(false);
        }
        stageList[focusedStageNum - 1].gameObject.SetActive(true);
        focusedStage = stageList[focusedStageNum - 1];
    }

    //player 이동횟수 업데이트
    void Update()
    {
        txt_playerMoves.text = playerMoves.ToString();
    }
    
    //Stage Clear 했을 때 나타나는 UI(Panel)
    public void ShowStageClearUI()
    {
        Player.canMove = false;
        go_StageClearUI.SetActive(true);

        focusedStage.CalcScore();
        Debug.Log(focusedStage.GetScore());

        for (int i = 0; i < focusedStage.GetScore(); i++)
        {
            Debug.Log("in");
            txt_Score[i].gameObject.SetActive(true);
        }
    }

    //StageClear UI에서 NextStage 버튼 클릭 시
    public void NextStageBtn()
    {
        if (focusedStageNum < stageList.Count)
        {
            playerMoves = 0;
            Player.canMove = true;
            go_StageClearUI.SetActive(false);
            stageList[focusedStageNum - 1].gameObject.SetActive(false);
            stageList[focusedStageNum++].gameObject.SetActive(true);
        }
    }
}