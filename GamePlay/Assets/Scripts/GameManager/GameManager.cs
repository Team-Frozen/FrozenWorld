using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    [SerializeField] GameObject go_StageClearUI;
    [SerializeField] Text txt_playerMoves;
    [SerializeField] List<Text> txt_Score;
    public static int playerMoves;
    Stage focusStage;

    void Start()
    {
        playerMoves = 0;
        Player.canMove = true;
        go_StageClearUI.SetActive(false);
        Database.Stage.SetActive(true);
        focusStage = Database.Stage.GetComponent<Stage>();
    }

    //Player 이동횟수 업데이트
    void Update()
    {
        txt_playerMoves.text = playerMoves.ToString();
    }

    //Stage Clear 했을 때 나타나는 UI(Panel)
    public void ShowStageClearUI()
    {
        Player.canMove = false;
        go_StageClearUI.SetActive(true);
        
        focusStage.CalcScore();

        for (int i = 0; i < focusStage.GetScore(); i++)
        {
            txt_Score[i].gameObject.SetActive(true);
        }
    }

    //StageClear UI에서 NextStage 버튼 클릭 시
    public void Change_NextStage()
    {
        if (Database.FocusStage + 1 < Database.Stages.Count)
        {
            playerMoves = 0;
            Player.canMove = true;
            go_StageClearUI.SetActive(false);
            Database.Stage.SetActive(false);
            Database.FocusStage++;
            Database.Stage.SetActive(true);
        }
    }

    public void ChangeScene_Stages()
    {
        //추가
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Start()
    {
        SceneManager.LoadScene("1_Start");
    }
}