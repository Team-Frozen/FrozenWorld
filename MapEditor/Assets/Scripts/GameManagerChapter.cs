using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject button;
    public Data data = new Data();

    public void Save()
    {
        void Awake()
        {
            if (Test.Btn_Chapters.Count != 0)
                Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(true);
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

    public void loadChapter()
    {
        int index;

        for (index = 0; Test.Btn_Chapters[index] != this.gameObject; index++) ;

        Test.FocusChapter = index;

        ChangeScene_Stages();
    }


    public void newChapter()
    {
        GameObject Button;

        Button = Instantiate(button, new Vector3(50 + 300 * (Test.Btn_Chapters.Count), 0, 0), Quaternion.identity);
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
