using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    public GameObject settingPanel;
    public CanvasGroup fadeGroup;

    void Awake()
    {
        FadeIn();
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);
    }

    public void ShowSettingPanel()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        SaveLoadManager.Save_SettingData();
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        settingPanel.SetActive(false);
    }

    public void ChangeScene_Character()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1-1_Character");
    }

    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        FadeOut("2_Chapters");
    }

    private void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(fadeGroup, 0, 1));
    }

    private void FadeOut(string SceneName)
    {
        StartCoroutine(FadeCanvasGroup(fadeGroup, 1, 0, SceneName));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, string SceneName, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(SceneName);
    }
}