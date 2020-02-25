using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum BlockType
{
    UNIT,
    EXIT,
    ORG,
    ARW,
    SLP,
    STP,
    PRT
}

[System.Serializable]
public class Data
{
    public int chapterCount;
    public List<int> stageCount;
    public List<StageData> stages;
}

[System.Serializable]
public class StageData
{
    //int minMove;
    public int gameAreaSize;
    public List<ElementData> elements;
}

[System.Serializable]
public class ElementData
{
    public Vec3 position;
    public BlockType blocktype;
}

[System.Serializable]
public class Vec3
{
    public float x;
    public float y;
    public float z;
}

public class GameManagerChapter : MonoBehaviour
{
 
//-------------Prefabs--------------//
    public GameObject btn_Chapters; //
    public GameObject btn_Stages;   //
    public GameObject unit;         //
    public GameObject exit;         //
    public GameObject stage;        //
    public GameObject orgBlock;     //
    public GameObject arwBlock;     //
    public GameObject slpBlock;     //
    public GameObject stpBlock;     //
    public GameObject canvas;       //
//----------------------------------//

    private float mousePos;
    private int page;

    public void Save()
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

        data.chapterCount = Test.Chapters.Count;
        data.stages = new List<StageData>();

        for (int i = 0; i < Test.Chapters.Count; i++)
        {
            data.stageCount.Add(Test.Chapters[i].Count);

            for (int j = 0; j < Test.Chapters[i].Count; j++)
            {
                StageData stageData = new StageData();
                stageData.gameAreaSize = Test.Chapters[i][j].GetComponent<Stage>().getStageSize();
                stageData.elements = new List<ElementData>();

                for (int k = 0; k < Test.Chapters[i][j].GetComponent<Stage>().getElements().Count; k++)
                {
                    ElementData elementData = new ElementData();
                    Vec3 vec3 = new Vec3();
                    vec3.x = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().transform.position.x;
                    vec3.y = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().transform.position.y;
                    vec3.z = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().transform.position.z;
                    elementData.position = vec3;
                    elementData.blocktype = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().returnType();
                    stageData.elements.Add(elementData);
                }

                data.stages.Add(stageData);
            }
        }

        DataManager.BinarySerialize<Data>(data, "Data.sav");
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
