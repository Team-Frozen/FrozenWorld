using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentSceneNum;   //현재 Scene 번호

    void Start()
    {
        //Scene이 바뀔때마다 현재 Scene 번호 저장
        currentSceneNum = SceneManager.GetActiveScene().buildIndex;
    }

    //Scene 전환
    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        //currentScene = buildIndex;
    }

    //SelectStage Scene에서 Stage Button 눌렀을 때 Scene 전환
    public void SelectStage(int selectedBtn)
    {
        SceneManager.LoadScene("Stage");
        StageManager.focusedStageNum = selectedBtn;
    }
}