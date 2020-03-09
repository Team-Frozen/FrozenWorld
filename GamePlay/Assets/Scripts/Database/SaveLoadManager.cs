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
    PRT,
    WALL
}
// data
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

//data
[System.Serializable]
public class Data_clear
{
    public List<ChapterData_clear> chapters;
}

[System.Serializable]
public class ChapterData_clear
{
    public bool isClear;
    public List<StageData_clear> stages;
}

[System.Serializable]
public class StageData_clear
{
    public bool isClear;
    public int score;
}

public class SaveLoadManager : MonoBehaviour
{
//------------ prefabs -------------//
    public GameObject btn_Chapters; //
    public GameObject btn_Stages;   //
    public GameObject Img_Score;    //
    public GameObject wall;         //
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
        
    private Data data;
    private Data_clear data_clear;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Load();

        for (int i = 0; i < Database.Chapters.Count; i++)
            Database.isClearChapter.Add(false);

        Load_ClearData();
    }

    void Update()
    {
       if (Input.anyKeyDown)
            if (SceneManager.GetActiveScene().name == "0_Loading")
                SceneManager.LoadScene("1_Start");                    
    }

    private void Load_ClearData()
    {
        data_clear = DataManager.BinaryDeserialize<Data_clear>("Data_clear.sav");

        if (data_clear != null)
        {
            for (Database.FocusChapter = 0; Database.FocusChapter < data_clear.chapters.Count; Database.FocusChapter++)
            {
                //ClearChapter의 다음 Btn_Chapter까지 interactable
                if (data_clear.chapters[Database.FocusChapter].isClear)
                    if (Database.FocusChapter + 1 < data_clear.chapters.Count)
                        Database.Btn_Chapters[Database.FocusChapter + 1].GetComponent<Button>().interactable = true;

                //Chapter: Clear했는지 저장
                Database.IsClearChapter = data_clear.chapters[Database.FocusChapter].isClear;

                for (Database.FocusStage = 0; Database.FocusStage < data_clear.chapters[Database.FocusChapter].stages.Count; Database.FocusStage++)
                {
                    //ClearStage의 다음 Btn_Stage까지 interactable
                    if (data_clear.chapters[Database.FocusChapter].stages[Database.FocusStage].isClear)
                    {
                        if (Database.FocusStage + 1 < data_clear.chapters[Database.FocusChapter].stages.Count)
                        {
                            Database.Btn_AllStages[Database.FocusChapter][Database.FocusStage + 1].GetComponent<Button>().interactable = true;
                        }
                        else if (Database.FocusChapter + 1 < data_clear.chapters.Count)
                        {
                            Database.Btn_Chapters[Database.FocusChapter + 1].GetComponent<Button>().interactable = true;
                            Database.Btn_AllStages[Database.FocusChapter + 1][0].GetComponent<Button>().interactable = true;
                        }
                    }
                    //Stage: Clear했는지 저장
                    Database.Stage.GetComponent<Stage>().SetIsClear(data_clear.chapters[Database.FocusChapter].stages[Database.FocusStage].isClear);
                    
                    //Stage: Score 저장
                    int temp_score = data_clear.chapters[Database.FocusChapter].stages[Database.FocusStage].score;
                    Database.Stage.GetComponent<Stage>().SetScore(temp_score);

                    //StageScene에서 Score SetActive
                    Database.Stage.GetComponent<Stage>().SetActiveStageScore();
                }
            }
        }
        else
        {
            data_clear = new Data_clear();
        }
    }

    public static void Save_ClearData()
    {
        Data_clear data_clear = new Data_clear();
        data_clear.chapters = new List<ChapterData_clear>();

        //save chapter data
        for (int i = 0; i < Database.Chapters.Count; i++)
        {
            ChapterData_clear chapterData_clear = new ChapterData_clear();
            chapterData_clear.stages = new List<StageData_clear>();
            chapterData_clear.isClear = Database.isClearChapter[i];

            //save stage data
            for (int j = 0; j < Database.Chapters[i].Count; j++)
            {
                StageData_clear stageData_clear = new StageData_clear();
                stageData_clear.isClear = Database.Chapters[i][j].GetComponent<Stage>().IsClear();
                stageData_clear.score = Database.Chapters[i][j].GetComponent<Stage>().GetScore();
                chapterData_clear.stages.Add(stageData_clear);
            }

            data_clear.chapters.Add(chapterData_clear);
        }

        DataManager.BinarySerialize<Data_clear>(data_clear, "Data_clear.sav");
        Debug.Log("clear data save");
    }

    private void Load()
    {
        int index = 0;

        data = DataManager.BinaryDeserialize<Data>("Data.sav");
        if (data != null)
        {
            GameObject canvasLoadChptr = GameObject.Find("Canvas_Load_Chapters");

            canvasLoadChptr.SetActive(false);

            for (Database.FocusChapter = 0; Database.FocusChapter < data.chapterCount; Database.FocusChapter++)
            {
                //add chapter button
                GameObject newChptrBtn;
                newChptrBtn = Instantiate(btn_Chapters, new Vector3(87 + 300 * (Database.Btn_Chapters.Count), 0, 0), Quaternion.identity);
                newChptrBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("number/num" + (Database.FocusChapter + 1), typeof(Sprite));
                newChptrBtn.transform.SetParent(canvasLoadChptr.transform, false);
                newChptrBtn.GetComponent<Button>().interactable = false;
                Database.Btn_Chapters.Add(newChptrBtn);

                //add canvas
                GameObject newCanvas;
                newCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
                Database.Canvases.Add(newCanvas);

                Database.Chapters.Add(new List<GameObject>());
                Database.Btn_AllStages.Add(new List<GameObject>());

                for (Database.FocusStage = 0; Database.FocusStage < data.stageCount[Database.FocusChapter]; Database.FocusStage++)
                {
                    //add stage
                    GameObject newStage;
                    newStage = Instantiate(stage, new Vector3(0, 0, 0), Quaternion.identity);
                    newStage.GetComponent<Stage>().SetStageSize(data.stages[index].gameAreaSize);
                    newStage.GetComponent<Stage>().SetMinMove(data.stages[index].minMove);
                    Database.Stages.Add(newStage);
                    
                    //add stage button
                    GameObject newStageBtn;
                    newStageBtn = Instantiate(btn_Stages, new Vector3(50 + 100 * (Database.Btn_Stages.Count % 9), -150 - 100 * (Database.Btn_Stages.Count / 9), 0), Quaternion.identity);
                    newStageBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("number/num" + (Database.FocusStage + 1), typeof(Sprite));
                    newStageBtn.transform.SetParent(newCanvas.transform, false);
                    newStageBtn.GetComponent<Button>().interactable = false;
                    Database.Btn_Stages.Add(newStageBtn);

                    //add stage score
                    List<GameObject> stageScore = new List<GameObject>();
                    for (int i = 0; i < 3; i++)
                    {
                        stageScore.Add(Instantiate(Img_Score, new Vector3(i * 30 - 30, -30, 0), Quaternion.identity));
                        stageScore[i].transform.SetParent(newStageBtn.transform, false);
                    }

                    int prtBlockNum = 0;
                    for (int k = 0; k < data.stages[index].elements.Count; k++)
                    {
                        //add block
                        GameObject newBlock = null;
                        Vector3 position = new Vector3();

                        position.x = data.stages[index].elements[k].position.x;
                        position.z = data.stages[index].elements[k].position.z;

                        switch (data.stages[index].elements[k].blockType)
                        {
                            case BlockType.UNIT:
                                position.y = 0.5f + unit.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(unit, position, Quaternion.identity);
                                newBlock.GetComponent<Player>().SetInitPos(position);
                                break;
                            case BlockType.EXIT:
                                for (int i = 0; i < data.stages[index].gameAreaSize; i++)
                                {
                                    Vector3 wallPosition = new Vector3();

                                    wallPosition.x = (data.stages[index].gameAreaSize + 1) * 0.5f;
                                    wallPosition.y = 1.5f;
                                    wallPosition.z = (data.stages[index].gameAreaSize + 1) * 0.5f - (i + 1);

                                    if (position.x != wallPosition.x || position.z != wallPosition.z)
                                    {
                                        GameObject newWall;
                                        newWall = Instantiate(wall, wallPosition, Quaternion.identity);
                                        newWall.transform.SetParent(Database.Stage.GetComponent<Stage>().getGameArea().transform);
                                    }
                                }
                                position.y = 0.5f + exit.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(exit, position, Quaternion.identity);
                                break;
                            case BlockType.ORG:
                                position.y = 0.5f + orgBlock.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(orgBlock, position, Quaternion.identity);
                                break;
                            case BlockType.ARW:
                                position.y = 0.5f + arwBlock.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(arwBlock, position, Quaternion.identity);
                                break;
                            case BlockType.SLP:
                                position.y = 0.5f + slpBlock.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(slpBlock, position, Quaternion.identity);
                                break;
                            case BlockType.STP:
                                position.y = 0.5f + stpBlock.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(stpBlock, position, Quaternion.identity);
                                break;
                            case BlockType.PRT:
                                position.y = 0.5f + prtBlock.transform.localScale.y * 0.5f;
                                newBlock = Instantiate(prtBlock, position, Quaternion.identity);
                                if (prtBlockNum % 2 == 1)
                                {
                                    newBlock.GetComponent<BlockPortal>().SetLinkedPortal(Database.Stage.GetComponent<Stage>().GetElements()[k - 1].GetComponent<BlockPortal>());
                                    Database.Stage.GetComponent<Stage>().GetElements()[k - 1].GetComponent<BlockPortal>().SetLinkedPortal(newBlock.GetComponent<BlockPortal>());
                                }
                                prtBlockNum++;
                                break;
                        }
                        newBlock.GetComponent<Element>().setProperty(data.stages[index].elements[k].property);
                        Database.Stage.GetComponent<Stage>().GetElements().Add(newBlock);
                        Database.Stage.GetComponent<Stage>().SetParent(newBlock);
                    }
                    Database.Stage.SetActive(false);
                    index++;
                }
                newCanvas.SetActive(false);
            }
            Database.Btn_Chapters[0].GetComponent<Button>().interactable = true;
            Database.Btn_AllStages[0][0].GetComponent<Button>().interactable = true;
        }
        else
        {
            System.IO.Directory.CreateDirectory("C:/FrozenWorld_Data");
            data = new Data();
        }
    }
}
