using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject go_StageClearUI;
    [SerializeField] Text txt_playerMoves;
    static public int playerMoves;
    static public int currentStage;

    [SerializeField] GameObject[] go_Stages;
    //[SerializeField] Transform[] tf_PlayerPos;
    //[SerializeField] Rigidbody rigid_Player;
    //int[] minimumMoves;
    //int[] score;
    
    void Start()
    {
        ShowNewStage(currentStage);
    }

    void Update()
    {
        txt_playerMoves.text = playerMoves.ToString();
    }

    public void ShowNewStage(int stageIndex)
    {
        Player.canMove = true;
        currentStage = stageIndex;
        go_Stages[currentStage - 1].SetActive(true);
        go_StageClearUI.SetActive(false);
        playerMoves = 0;
    }
    
    public void ShowStageClearUI()
    {
        Player.canMove = false;
        go_StageClearUI.SetActive(true);
        //하트 띄우기, 하트 개수 저장 추가해야 함
    }

    public void NextStageBtn()
    {
        if (currentStage < go_Stages.Length)
        {
            Player.canMove = true;
            //rigid_Player.gameObject.transform.position = tf_PlayerPos[currentStage].position;
            currentStage -= 1;
            go_Stages[currentStage++].SetActive(false);
            go_Stages[currentStage++].SetActive(true);
            go_StageClearUI.SetActive(false);
            playerMoves = 0;
        }
    }
}
