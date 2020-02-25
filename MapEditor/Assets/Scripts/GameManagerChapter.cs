using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerChapter : MonoBehaviour
{
 
//-------------Prefabs--------------//
    public GameObject btn_Chapters; //
//----------------------------------//

    private float mousePos;
    private int page;

    void Awake()
    {
        page = Test.FocusChapter / 3;
        int i = 0;
        foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
        {
            Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) - (900 * page), 0, 0);
            i++;
        }

        if (Test.Btn_Chapters.Count != 0)
            Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(true);

        //Add Listener To All Btns
        foreach (GameObject Button in Test.Btn_Chapters)
        {
            Button.GetComponent<Button>().onClick.AddListener(loadChapter);
        }
    }

    void OnGUI()
    {
        Event m_Event = Event.current;

        //--------마우스 클릭----------//
        if (m_Event.type == EventType.MouseDown)
        {
            mousePos = Input.mousePosition.x;
        }
        //--------마우스 드래그----------//
        if (m_Event.type == EventType.MouseDrag)
        {
            float diff = Input.mousePosition.x - mousePos;
            if (Mathf.Abs(diff) <= 150)
            {
                int i = 0;
                foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
                {
                    Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) + diff - (900 * page), 0 ,0);
                    i++;
                }
            }
        }
        //--------마우스 놓음----------//
        if (m_Event.type == EventType.MouseUp)
        {
            float diff = Input.mousePosition.x - mousePos;
            if (Mathf.Abs(diff) <= 150)
            {
                int i = 0;
                foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
                {
                    Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) - (900 * page), 0, 0);
                    i++;
                }
            }
            else
            {
                if (diff < 0 && page < (Test.Btn_Chapters.Count - 1) / 3)
                {
                    page++;
                    int i = 0;
                    foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
                    {
                        Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) - (900 * page), 0, 0);
                        i++;
                    }
                }
                else if(diff > 0 && page > 0)
                {
                    page--;
                    int i = 0;
                    foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
                    {
                        Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) - (900 * page), 0, 0);
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    foreach (GameObject Btn_Chapter in Test.Btn_Chapters)
                    {
                        Btn_Chapter.GetComponent<RectTransform>().anchoredPosition = new Vector3(87 + (300 * i) - (900 * page), 0, 0);
                        i++;
                    }
                }
            }
        }
    }
    public void loadChapter()
    {
        int index;

        for (index = 0; Test.Btn_Chapters[index] != EventSystem.current.currentSelectedGameObject; index++) ;

        Test.FocusChapter = index;

        ChangeScene_Stages();
    }


    public void newChapter()
    {
        GameObject Button;

        Button = Instantiate(btn_Chapters, new Vector3(87 + 300 * (Test.Btn_Chapters.Count) - (900 * page), 0, 0), Quaternion.identity);// 수정
        Button.transform.SetParent(GameObject.Find("Canvas_Load_Chapters").transform, false);
        Test.Btn_Chapters.Add(Button);

        Test.FocusChapter = Test.Chapters.Count;
        Test.Chapters.Add(new List<GameObject>());
        Test.Btn_AllStages.Add(new List<GameObject>());

        ChangeScene_Stages();
    }

    private void ChangeScene_Stages()
    {
        Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene("Stages");
    }
}
