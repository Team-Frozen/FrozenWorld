﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    [SerializeField] GameObject go_StageClearUI;    //Clear 했을 때 나타나는 Panel
    [SerializeField] List<Text> txt_Score;  // 점수
    [SerializeField] Button btn_Back;       // 뒤로가기 버튼
    [SerializeField] Text txt_PlayerMoves;  // Player 이동 횟수
    public static int playerMoves;

    void Awake()
    {
        playerMoves = 0;
        Player.canMove = true;
        btn_Back.interactable = true;
        Database.Player.GetComponent<Player>().MoveToInitPos();
        Database.Stage.SetActive(true);
        go_StageClearUI.SetActive(false);
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
        go_StageClearUI.SetActive(true);
        
        Database.Stage.GetComponent<Stage>().CalcScore();

        for (int i = 0; i < Database.Stage.GetComponent<Stage>().GetScore(); i++)
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
            btn_Back.interactable = false;
            go_StageClearUI.SetActive(false);
            Database.Stage.SetActive(false);
            Database.Player.GetComponent<Player>().MoveToInitPos();
            Database.FocusStage++;
            Database.Stage.SetActive(true);
        }
    }

    public void ChangeScene_Stages()
    {
        Database.Stage.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Start()
    {
        Database.Stage.SetActive(false);
        SceneManager.LoadScene("1_Start");
    }
}