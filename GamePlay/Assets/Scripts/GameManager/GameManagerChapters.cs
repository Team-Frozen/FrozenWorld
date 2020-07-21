using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerChapters : MonoBehaviour
{
    public List<CanvasGroup> fadeGroup;

    private float mousePos;
    private int page;
    private bool chptrBtnMoving;
    private bool mouseClicked;

    void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.MAIN_BGM);

        chptrBtnMoving = false;
        mouseClicked = false;
        page = 0;
        setChptrLoc(0);

        if (Database.Btn_Chapters.Count != 0)
            Database.Btn_Chapters[0].transform.parent.parent.gameObject.SetActive(true);

        fadeGroup.Add(Database.Btn_Chapters[0].transform.parent.parent.GetComponent<CanvasGroup>());
        FadeIn(fadeGroup);

        //Add Listener To All Btns
        foreach (GameObject Button in Database.Btn_Chapters)
        {
            Button.GetComponent<Button>().onClick.AddListener(loadChapter);
        }
    }

    void OnGUI()
    {
        if (chptrBtnMoving == false)
        {
            Event m_Event = Event.current;
            //Mouse Pressed
            if (m_Event.type == EventType.MouseDown)
            {
                mouseClicked = true;
                mousePos = Input.mousePosition.x;
            }

            //Mouse Draged
            if (m_Event.type == EventType.MouseDrag && mouseClicked)
            {
                float diff = Input.mousePosition.x - mousePos;

                if (EventSystem.current.currentSelectedGameObject != null && Mathf.Abs(diff) > 50)
                    EventSystem.current.SetSelectedGameObject(null);
               
                if (Mathf.Abs(diff) <= 500)
                    setChptrLoc(diff);
            }

            //Mouse Released
            if (m_Event.type == EventType.MouseUp && mouseClicked)
            {
                float diff = Input.mousePosition.x - mousePos;

                if (Mathf.Abs(diff) > 120)
                {
                    if (diff < 0 && page < Database.Btn_Chapters.Count - 1)
                    {
                        page++;
                    }
                    else if (diff > 0 && page > 0)
                    {
                        page--;
                    }
                }
                chptrBtnMoving = true;
                mouseClicked = false;
                StartCoroutine(MoveSmooth());
            }
        }
    }

    private void setChptrLoc(float diff)
    {
        int i = 0;
        foreach (GameObject Btn_Chapter in Database.Btn_Chapters)
        {
            Btn_Chapter.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3((800 * i) + diff - (800 * page), 20, 0);
            i++;
        }
    }

    public void loadChapter()
    {
        if (EventSystem.current.currentSelectedGameObject != null) {
            int index;
            for (index = 0; Database.Btn_Chapters[index] != EventSystem.current.currentSelectedGameObject; index++) ;
            Database.FocusChapter = index;

            ChangeScene_Stages();
        }
    }

    public void ChangeScene_Stages()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Btn_Chapters[0].transform.parent.parent.gameObject.SetActive(false);
        FadeOut(fadeGroup, "3_Stages");
    }

    public void ChangeScene_Start()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Btn_Chapters[0].transform.parent.parent.gameObject.SetActive(false);
        FadeOut(fadeGroup, "1_Start");
    }

    private void FadeIn(List<CanvasGroup> cg)
    {
        StartCoroutine(FadeCanvasGroup(cg, 0, 1));
    }
    private void FadeOut(List<CanvasGroup> cg, string SceneName)
    {
        StartCoroutine(FadeCanvasGroup(cg, 1, 0, SceneName));
    }

    public IEnumerator MoveSmooth(float lerpTime = 10f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = 0.5f * (Mathf.Log10(Time.time / lerpTime * 0.99f + 0.01f) + 2);

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = 0.5f*(Mathf.Log10(timeSinceStarted / lerpTime * 0.99f + 0.01f) + 2);

            for (int i = 0; i < Database.Btn_Chapters.Count; i++)
            {
                float currentValue = Mathf.Lerp(Database.Btn_Chapters[i].transform.parent.GetComponent<RectTransform>().anchoredPosition.x, 800 * (i - page), percentageComplete);
                Database.Btn_Chapters[i].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentValue, 20, 0);
            }
            if (percentageComplete >= 1 || Mathf.Abs(Database.Btn_Chapters[0].transform.parent.GetComponent<RectTransform>().anchoredPosition.x - 800 * -page) < 10) break;
            yield return new WaitForEndOfFrame();
        }
        chptrBtnMoving = false;
        setChptrLoc(0);
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

            for(int i = 0; i < cg.Count; i++)
                cg[i].alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneName);
    }
}