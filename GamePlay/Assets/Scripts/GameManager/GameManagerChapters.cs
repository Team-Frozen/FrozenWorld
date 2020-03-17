using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerChapters : MonoBehaviour
{
    //prefab
    public GameObject btn_Chapters;
    //

    float mousePos;
    int page;

    void Awake()
    {
        page = Database.FocusChapter / 3;
        setChptrLoc(0);

        if (Database.Btn_Chapters.Count != 0)
            Database.Btn_Chapters[0].transform.parent.gameObject.SetActive(true);

        //Add Listener To All Btns
        foreach (GameObject Button in Database.Btn_Chapters)
        {
            Button.GetComponent<Button>().onClick.AddListener(loadChapter);
        }
    }

    void OnGUI()
    {
        Event m_Event = Event.current;

        //Mouse Pressed
        if (m_Event.type == EventType.MouseDown)
        {
            mousePos = Input.mousePosition.x;
        }

        //Mouse Draged
        if (m_Event.type == EventType.MouseDrag)
        {
            float diff = Input.mousePosition.x - mousePos;
            if (Mathf.Abs(diff) <= 150)
                setChptrLoc(diff);
        }

        //Mouse Released
        if (m_Event.type == EventType.MouseUp)
        {
            float diff = Input.mousePosition.x - mousePos;

            if (Mathf.Abs(diff) > 150)
            {
                if (diff < 0 && page < (Database.Btn_Chapters.Count - 1) / 3)
                    page++;
                else if (diff > 0 && page > 0)
                    page--;
            }

            setChptrLoc(0f);
        }
    }

    private void setChptrLoc(float diff)
    {
        int i = 0;
        foreach (GameObject Btn_Chapter in Database.Btn_Chapters)
        {
            Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) + diff - (900 * page), 0, 0);
            i++;
        }
    }

    public void loadChapter()
    {
        int index;
        for (index = 0; Database.Btn_Chapters[index] != EventSystem.current.currentSelectedGameObject; index++) ;
        Database.FocusChapter = index;

        ChangeScene_Stages();
    }

    public void ChangeScene_Stages()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Btn_Chapters[0].transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Start()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Btn_Chapters[0].transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene("1_Start");
    }
}