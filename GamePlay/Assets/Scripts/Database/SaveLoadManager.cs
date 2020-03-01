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

//------------ prefabs -------------//
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            if (SceneManager.GetActiveScene().name == "0_Loading")
                SceneManager.LoadScene("1_Start");
            //else
            //    if (Input.GetKey("s"))
            //        Save();
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
                newChptrBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("number/num" + (Database.FocusChapter + 1), typeof(Sprite)); //이미지 추가
                newChptrBtn.transform.SetParent(canvasLoadChptr.transform, false);
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
                    Database.Stages.Add(newStage);
                    
                    //add stage button
                    GameObject newStageBtn;
                    newStageBtn = Instantiate(btn_Stages, new Vector3(50 + 100 * (Database.Btn_Stages.Count % 9), -150 - 100 * (Database.Btn_Stages.Count / 9), 0), Quaternion.identity);
                    newStageBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("number/num" + (Database.FocusStage + 1), typeof(Sprite)); //이미지 추가
                    newStageBtn.transform.SetParent(newCanvas.transform, false);
                    Database.Btn_Stages.Add(newStageBtn);

                    for (int k = 0; k < data.stages[index].elements.Count; k++)
                    {
                        //add block
                        GameObject newBlock = null;
                        Vector3 position = new Vector3(data.stages[index].elements[k].position.x, data.stages[index].elements[k].position.y, data.stages[index].elements[k].position.z);

                        switch (data.stages[index].elements[k].blockType)
                        {
                            case BlockType.UNIT:
                                newBlock = Instantiate(unit, position, Quaternion.identity);
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
                                newBlock = null;
                                break;
                        }
                        Database.Stage.GetComponent<Stage>().GetElements().Add(newBlock);
                        Database.Stage.GetComponent<Stage>().SetParent(newBlock);
                    }
                    Database.Stage.SetActive(false);
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
        data.chapterCount = Database.Chapters.Count;
        data.stages = new List<StageData>();
        data.stageCount = new List<int>();

        for (int i = 0; i < Database.Chapters.Count; i++)
        {
            data.stageCount.Add(Database.Chapters[i].Count);

            //save stages
            for (int j = 0; j < Database.Chapters[i].Count; j++)
            {
                StageData stageData = new StageData();
                stageData.gameAreaSize = Database.Chapters[i][j].GetComponent<Stage>().GetStageSize();
                stageData.elements = new List<ElementData>();

                //save elements
                for (int k = 0; k < Database.Chapters[i][j].GetComponent<Stage>().GetElements().Count; k++)
                {
                    ElementData elementData = new ElementData();
                    Vec3 vec3 = new Vec3();
                    vec3.x = Database.Chapters[i][j].GetComponent<Stage>().GetElements()[k].GetComponent<Element>().transform.position.x;
                    vec3.y = Database.Chapters[i][j].GetComponent<Stage>().GetElements()[k].GetComponent<Element>().transform.position.y;
                    vec3.z = Database.Chapters[i][j].GetComponent<Stage>().GetElements()[k].GetComponent<Element>().transform.position.z;
                    elementData.position = vec3;
                    elementData.blockType = Database.Chapters[i][j].GetComponent<Stage>().GetElements()[k].GetComponent<Element>().ReturnType();
                    stageData.elements.Add(elementData);
                }
                data.stages.Add(stageData);
            }
        }

        DataManager.BinarySerialize<Data>(data, "Data.sav");
    }
}
