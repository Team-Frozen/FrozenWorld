using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int minMove;
    public int gameAreaSize;
    public List<ElementData> elements;
}

[System.Serializable]
public class ElementData
{
    public int property;
    public Vec3 position;
    public BlockType blockType;
}

[System.Serializable]
public class Vec3
{
    public float x;
    public float y;
    public float z;
}


public class SaveLoadManager : MonoBehaviour
{
    private Data data;
    private const int STAGES_IN_A_ROW = 7;
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
    public GameObject prtBlock;     //
    public GameObject canvas;       //
//----------------------------------//

    private GameObject canvasLoadChptr;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
       
        load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name == "1_Loading")
            {
                canvasLoadChptr.SetActive(true);
                SceneManager.LoadScene(1);
            }
            else
                if (Input.GetKey("s"))
                    Save();
        }
    }

    private void load()
    {
        int index = 0;

        data = DataManager.BinaryDeserialize<Data>("Data.sav");
        if (data != null)
        {
            canvasLoadChptr = GameObject.Find("Canvas_Load_Chapters");

            canvasLoadChptr.SetActive(false);

            for (Test.FocusChapter = 0; Test.FocusChapter < data.chapterCount; Test.FocusChapter++)
            {
                //add chapter button
                GameObject newChptrBtn;
                newChptrBtn = Instantiate(btn_Chapters, new Vector3(50 + 250 * (Test.Btn_Chapters.Count), 0, 0), Quaternion.identity);
                newChptrBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("buttons/num" + (Test.FocusChapter + 1), typeof(Sprite));
                newChptrBtn.transform.SetParent(canvasLoadChptr.transform, false);
                Test.Btn_Chapters.Add(newChptrBtn);

                //add canvas
                GameObject newCanvas;
                newCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
                Test.Canvases.Add(newCanvas);

                Test.Chapters.Add(new List<GameObject>());
                Test.Btn_AllStages.Add(new List<GameObject>());

                for (Test.FocusStage = 0; Test.FocusStage < data.stageCount[Test.FocusChapter]; Test.FocusStage++)
                {
                    //add stage
                    GameObject newStage;
                    newStage = Instantiate(stage, new Vector3(0, 0, 0), Quaternion.identity);
                    newStage.GetComponent<Stage>().setMinMove(data.stages[index].minMove);
                    newStage.GetComponent<Stage>().setStageSize(data.stages[index].gameAreaSize);
                    Test.Stages.Add(newStage);

                    //add stage button
                    GameObject newStageBtn;
                    newStageBtn = Instantiate(btn_Stages, new Vector3(50 + 100 * (Test.Btn_Stages.Count % STAGES_IN_A_ROW), -100 - 100 * (Test.Btn_Stages.Count / STAGES_IN_A_ROW), 0), Quaternion.identity);
                    newStageBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("buttons/num" + (Test.FocusStage + 1), typeof(Sprite));
                    newStageBtn.transform.SetParent(newCanvas.transform, false);
                    Test.Btn_Stages.Add(newStageBtn);

                    int prtBlockNum = 0;
                    for (int k = 0; k < data.stages[index].elements.Count; k++)
                    {
                        //add block
                        GameObject newBlock = null;
                        Vector3 position = new Vector3(data.stages[index].elements[k].position.x, data.stages[index].elements[k].position.y, data.stages[index].elements[k].position.z);

                        switch (data.stages[index].elements[k].blockType)
                        {
                            case BlockType.UNIT:
                                newBlock = Instantiate(unit, position, Quaternion.identity);
                                newBlock.AddComponent<Unit>();
                                break;
                            case BlockType.EXIT:
                                newBlock = Instantiate(exit, position, Quaternion.identity);
                                break;
                            case BlockType.ORG:
                                newBlock = Instantiate(orgBlock, position, Quaternion.identity);
                                break;
                            case BlockType.ARW:
                                newBlock = Instantiate(arwBlock, position, Quaternion.identity);
                                break;
                            case BlockType.SLP:
                                newBlock = Instantiate(slpBlock, position, Quaternion.identity);
                                break;
                            case BlockType.STP:
                                newBlock = Instantiate(stpBlock, position, Quaternion.identity);
                                break;
                            case BlockType.PRT:
                                newBlock = Instantiate(prtBlock, position, Quaternion.identity);
                                if (prtBlockNum % 2 == 1)
                                {
                                    newBlock.GetComponent<PrtBlock>().setLinkedPortal(Test.Stage.GetComponent<Stage>().getElements()[k - 1].GetComponent<PrtBlock>());
                                    Test.Stage.GetComponent<Stage>().getElements()[k - 1].GetComponent<PrtBlock>().setLinkedPortal(newBlock.GetComponent<PrtBlock>());
                                }
                                prtBlockNum++;
                                break;
                        }
                        newBlock.GetComponent<Element>().setProperty(data.stages[index].elements[k].property);
                        Test.Stage.GetComponent<Stage>().getElements().Add(newBlock);
                        Test.Stage.GetComponent<Stage>().setParent(newBlock);
                    }
                    Test.Stage.SetActive(false);
                    index++;
                }
                newCanvas.SetActive(false);
            }
        }
        else
        {
            data = new Data();
        }
    }

    public void Save()
    {
        data.chapterCount = Test.Chapters.Count;
        data.stages = new List<StageData>();

        data.stageCount = new List<int>();
        for (int i = 0; i < Test.Chapters.Count; i++)
        {
            data.stageCount.Add(Test.Chapters[i].Count);

            for (int j = 0; j < Test.Chapters[i].Count; j++)
            {
                StageData stageData = new StageData();
                stageData.minMove = Test.Chapters[i][j].GetComponent<Stage>().getMinMove();
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
                    elementData.property = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().getProperty();
                    elementData.blockType = Test.Chapters[i][j].GetComponent<Stage>().getElements()[k].GetComponent<Element>().returnType();
                    stageData.elements.Add(elementData);
                }
                data.stages.Add(stageData);
            }
        }

        DataManager.BinarySerialize<Data>(data, "Data.sav");
    }
}
