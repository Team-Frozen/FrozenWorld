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
    public List<CanvasGroup> fadeGroup;
    public GameObject mainCanvas;
    public Text totalScore;

    private GameObject focusButton;

    void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);
        totalScore.text = Database.Chapter_List[Database.FocusChapter].GetScore() + "";

        focusButton = null;
        Database.Canvas.SetActive(true);

        fadeGroup.Add(Database.Canvas.GetComponent<CanvasGroup>());
        FadeIn(fadeGroup);

        //Add Listener To All Btns
        foreach (GameObject Button in Database.Btn_Stages)
        {
            Button.GetComponent<Button>().onClick.AddListener(BtnClicked);
        }
    }

    public void BtnClicked() //Btn_Load_Stage 눌렀을 때 실행하는 함수
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        focusButton = EventSystem.current.currentSelectedGameObject;
        LoadStage();
    }

    private void LoadStage()
    {
        int index;
        for (index = 0; Database.Btn_Stages[index] != focusButton; index++) ;
        Database.FocusStage = index;        

        ChangeScene_GamePlay();
    }

    private void ChangeScene_GamePlay()
    {
        Database.Canvas.SetActive(false);
        FadeOut(fadeGroup, "4_GamePlay");
    }

    //뒤로가기 버튼
    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        if (focusButton != null)
            focusButton.GetComponent<Image>().color = Color.white;

        Database.Canvas.SetActive(false);
        FadeOut(fadeGroup, "2_Chapters");
    }

    private void FadeIn(List<CanvasGroup> cg)
    {
        StartCoroutine(FadeCanvasGroup(cg, 0, 1));
    }
    private void FadeOut(List<CanvasGroup> cg, string SceneName)
    {
        StartCoroutine(FadeCanvasGroup(cg, 1, 0, SceneName));
    }

    public IEnumerator FadeCanvasGroup(List<CanvasGroup> cg, float start, float end, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            for (int i = 0; i < cg.Count; i++)
                cg[i].alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeCanvasGroup(List<CanvasGroup> cg, float start, float end, string SceneName, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            for (int i = 0; i < cg.Count; i++)
                cg[i].alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneName);
    }
}
