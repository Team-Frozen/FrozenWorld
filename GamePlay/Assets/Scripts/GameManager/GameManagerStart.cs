using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    public GameObject settingPanel;
    public CanvasGroup fadeGroup;
    public GameObject btn_character;

    void Awake()
    {
        FadeIn();
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);

        string characterName = "MulBeong";
        if (CharacterSelectManager.selectedCharacter == 0)
            characterName = "MulBeong";
        else if (CharacterSelectManager.selectedCharacter == 1)
            characterName = "PengSik";
        else if (CharacterSelectManager.selectedCharacter == 2)
            characterName = "Ddori";
        btn_character.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/unit/" + characterName) as Sprite;
        btn_character.GetComponent<Image>().SetNativeSize();
        RectTransform rt = btn_character.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(btn_character.GetComponent<RectTransform>().sizeDelta.x * 0.6f, btn_character.GetComponent<RectTransform>().sizeDelta.y * 0.6f);
    }
    public void ExitGame()
    {
        Application.Quit();
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

    public void ChangeScene_HowToPlay()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        FadeOut("1-2_HowToPlay");
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