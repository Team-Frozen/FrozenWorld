using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerStages : MonoBehaviour
{
    //prefabs
    public GameObject button;
    public GameObject ghostButton;
    public GameObject canvas;
    //

    public GameObject mainCanvas;
    private GameObject focusButton;

    void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);

        focusButton = null;
        Database.Canvas.SetActive(true);

        //Add Listener To All Btns
        foreach (GameObject Button in Database.Btn_Stages)
        {
            Button.GetComponent<Button>().onClick.AddListener(BtnClicked);
        }
    }

    public void BtnClicked() //Btn_Load_Stage 눌렀을 때 실행하는 함수
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        //한번 눌렀을 때
        if (focusButton == null || focusButton != EventSystem.current.currentSelectedGameObject)
        {
            if (focusButton != null)
                focusButton.GetComponent<Image>().color = Color.white;
            focusButton = EventSystem.current.currentSelectedGameObject;
            focusButton.GetComponent<Image>().color = Color.cyan;
        }
        //두번 눌렀을 때
        else
            LoadStage();
    }

    private void LoadStage()
    {
        int index;
        for (index = 0; Database.Btn_Stages[index] != focusButton; index++) ;
        Database.FocusStage = index;
        
        focusButton.GetComponent<Image>().color = Color.white;

        ChangeScene_GamePlay();
    }

    private void ChangeScene_GamePlay()
    {
        Database.Canvas.SetActive(false);
        SceneManager.LoadScene("4_GamePlay");
    }

    //뒤로가기 버튼
    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        if (focusButton != null)
            focusButton.GetComponent<Image>().color = Color.white;

        Database.Canvas.SetActive(false);
        SceneManager.LoadScene("2_Chapters");
    }
}
