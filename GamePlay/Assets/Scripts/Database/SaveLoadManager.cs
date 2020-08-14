using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

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

//data_load/save
[System.Serializable]
public class Data
{
    public int chapterCount;        // chpater 수
    public List<int> stageCount;    // stage 수
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

//data_clear
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

//Setting Data
[System.Serializable]
public class Data_Setting
{
    public int character;
    public bool control_Button;
    public float BGMVolume;
    public float soundVolume;
    public bool soundOn;
}

public class SaveLoadManager : MonoBehaviour
{
    //prefabs---------------------------//
    public GameObject btn_Chapters;
    public GameObject btn_Stages;
    public GameObject img_stageScore;
    public GameObject wall;
    public GameObject unit1;
    public GameObject unit2;
    public GameObject unit3;
    public GameObject exit;
    public GameObject stage;
    public GameObject orgBlock;
    public GameObject arwBlock;
    public GameObject slpBlock;
    public GameObject stpBlock;
    public GameObject prtBlock;
    public GameObject canvas;
    //-----------------------------------//

    private Data data;
    private Data_clear data_clear;
    private Data_Setting data_setting;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Load();
        Load_ClearData();
        Load_SettingData();
        Database.FocusChapter = 0;
    }

    void Update()
    {
       if (Input.touchCount > 0 || Input.anyKeyDown)
            if (SceneManager.GetActiveScene().name == "0_Loading")
                SceneManager.LoadScene("1_Start");                    
    }

    private void Load_ClearData()
    {
        data_clear = DataManager.BinaryDeserialize<Data_clear>(Application.persistentDataPath + "/Data_Clear.sav");

        if (data_clear != null)
        {
            for (Database.FocusChapter = 0; Database.FocusChapter < data_clear.chapters.Count; Database.FocusChapter++)
            {
                for (Database.FocusStage = 0; Database.FocusStage < data_clear.chapters[Database.FocusChapter].stages.Count; Database.FocusStage++)
                {
                    //Stage: Score 저장
                    Database.Stage.GetComponent<Stage>().SetScore(data_clear.chapters[Database.FocusChapter].stages[Database.FocusStage].score);

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

                        //StageScene에서 Score SetActive
                        Database.Stage.GetComponent<Stage>().SetActiveStageScore();
                    }

                    //Stage: Clear했는지 저장
                    Database.Stage.GetComponent<Stage>().SetIsClear(data_clear.chapters[Database.FocusChapter].stages[Database.FocusStage].isClear);     
                }

                //Chapter 점수 계산
                Database.Chapter.CalcScore(Database.FocusChapter);

                if (Database.FocusChapter == 0 || data_clear.chapters[Database.FocusChapter].isClear || data_clear.chapters[Database.FocusChapter - 1].isClear == true)
                {
                    //Btn 활성화
                    Database.Btn_Chapters[Database.FocusChapter].GetComponent<Button>().interactable = true;
                }

                //ChapterScene에서 clear한 Chapter의 Score txt 설정
                Database.Chapter.UpdateChapterScoreTxt();

                //Chapter: Clear했는지 저장
                Database.Chapter.SetIsClear(data_clear.chapters[Database.FocusChapter].isClear);
            }
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
            chapterData_clear.isClear = Database.Chapter_List[i].GetIsClear();

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

        DataManager.BinarySerialize<Data_clear>(data_clear, Application.persistentDataPath + "/Data_Clear.sav");
    }

    private void Load()
    {
        string tempPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data_Map.sav");

        // Android only use WWW to read file
        WWW reader = new WWW(tempPath);
        while (!reader.isDone) { }

        string filePath = Application.persistentDataPath + "/Data_Map.sav";
        System.IO.File.WriteAllBytes(filePath, reader.bytes);

        data = DataManager.BinaryDeserialize<Data>(filePath);

        int index = 0;

        if (data != null)
        {
            GameObject canvasLoadChptr = GameObject.Find("Canvas_Load_Chapters");

            canvasLoadChptr.SetActive(false);
            for (Database.FocusChapter = 0; Database.FocusChapter < data.chapterCount; Database.FocusChapter++)
            {
                //add chapter button
                GameObject newChptrBtn;

                newChptrBtn = Instantiate(btn_Chapters, new Vector3(800 * (Database.Btn_Chapters.Count), 20, 0), Quaternion.identity);
                newChptrBtn.transform.GetChild(0).GetComponent<Image>().sprite = (Sprite)Resources.Load("UI/chapter/cave" + (Database.FocusChapter + 1), typeof(Sprite));
                newChptrBtn.transform.GetChild(0).GetComponent<Button>().interactable = false;
                newChptrBtn.transform.GetChild(2).GetComponent<Image>().sprite = (Sprite)Resources.Load("UI/string/cave" + (Database.FocusChapter + 1), typeof(Sprite));
                newChptrBtn.transform.GetChild(2).GetComponent<Image>().SetNativeSize();
                newChptrBtn.transform.SetParent(canvasLoadChptr.transform, false);

                Database.Btn_Chapters.Add(newChptrBtn.transform.GetChild(0).gameObject);
                
                //add canvas
                GameObject newCanvas;
                newCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
                Database.Canvases.Add(newCanvas);

                Database.Chapter_List.Add(new Chapter());
                Database.Chapters.Add(new List<GameObject>());
                Database.Btn_AllStages.Add(new List<GameObject>());
                
                for (Database.FocusStage = 0; Database.FocusStage < data.stageCount[Database.FocusChapter]; Database.FocusStage++)
                {
                    //add stage
                    GameObject newStage;
                    newStage = Instantiate(stage, new Vector3(0, -3f, 0), Quaternion.identity);
                    newStage.GetComponent<Stage>().SetStageSize(data.stages[index].gameAreaSize);
                    newStage.GetComponent<Stage>().SetMinMove(data.stages[index].minMove);
                    Database.Stages.Add(newStage);
                    
                    //add stage button
                    GameObject newStageBtn;
                    newStageBtn = Instantiate(btn_Stages, new Vector3(-400 + 200 * (Database.Btn_Stages.Count % 5), 450 - 200 * (Database.Btn_Stages.Count / 5), 0), Quaternion.identity);
                    newStageBtn.GetComponent<Image>().sprite = (Sprite)Resources.Load("UI/stage/num" + (Database.FocusStage + 1), typeof(Sprite));
                    newStageBtn.transform.SetParent(newCanvas.transform, false);
                    newStageBtn.GetComponent<Button>().interactable = false;
                    Database.Btn_Stages.Add(newStageBtn);

                    //add stage score
                    List<GameObject> stageScore = new List<GameObject>();
                    for (int i = 0; i < 3; i++)
                    {
                        stageScore.Add(Instantiate(img_stageScore, new Vector3((1 - i) * 50, 30, 0), Quaternion.identity));
                        stageScore[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f * i, 0);
                        stageScore[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f * i, 0);
                        stageScore[i].transform.SetParent(newStageBtn.transform, false);
                        stageScore[i].SetActive(false);
                    }

                    int prtBlockNum = 0;
                    for (int k = 0; k < data.stages[index].elements.Count; k++)
                    {
                        //add block
                        GameObject newBlock = null;
                        Vector3 position = new Vector3();

                        position.x = data.stages[index].elements[k].position.x;
                        position.z = data.stages[index].elements[k].position.z;
                        position.y = data.stages[index].elements[k].position.y;

                        switch (data.stages[index].elements[k].blockType)
                        {
                            case BlockType.UNIT:
                                position.y = position.y - 3f;
                                Database.Stage.GetComponent<Stage>().SetPlayerPos(position);
                                Database.Stage.GetComponent<Stage>().SetPlayerProperty(data.stages[index].elements[k].property);
                                break;
                            case BlockType.EXIT:
                                for (int i = 0; i < data.stages[index].gameAreaSize; i++)
                                {
                                    Vector3 wallPosition = new Vector3();

                                    wallPosition.x = (data.stages[index].gameAreaSize + 1) * 0.5f;
                                    wallPosition.y = 2.0f;
                                    wallPosition.z = (data.stages[index].gameAreaSize + 1) * 0.5f - (i + 1);

                                    if (position.x != wallPosition.x || position.z != wallPosition.z)
                                    {
                                        GameObject newWall;
                                        newWall = Instantiate(wall, wallPosition, Quaternion.identity);
                                        newWall.transform.SetParent(Database.Stage.GetComponent<Stage>().getGameArea().transform);
                                        newWall.transform.localPosition = wallPosition;
                                    }
                                }
                                newBlock = Instantiate(exit, position, Quaternion.identity);
                                break;
                            case BlockType.ORG:
                                position.y = position.y - 0.05f;
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
                                newBlock.GetComponent<BlockPortal>().SetColor(prtBlockNum / 2);
                                if (prtBlockNum % 2 == 1)
                                {
                                    newBlock.GetComponent<BlockPortal>().SetLinkedPortal(Database.Stage.GetComponent<Stage>().GetElements()[k - 2].GetComponent<BlockPortal>());
                                    Database.Stage.GetComponent<Stage>().GetElements()[k - 2].GetComponent<BlockPortal>().SetLinkedPortal(newBlock.GetComponent<BlockPortal>());
                                }
                                prtBlockNum++;
                                break;
                        }

                        
                        if (data.stages[index].elements[k].blockType != BlockType.UNIT)
                        {
                            newBlock.GetComponent<Element>().setProperty(data.stages[index].elements[k].property);
                            Database.Stage.GetComponent<Stage>().GetElements().Add(newBlock);
                            Database.Stage.GetComponent<Stage>().SetParent(newBlock);
                            newBlock.transform.localPosition = position;
                        }
                    }
                    Database.Stage.SetActive(false);
                    index++;
                }
                newCanvas.SetActive(false);
            }

            Database.Chapter_List[0].UpdateChapterScoreTxt(0);     //Chapter1_Score active
            Database.Btn_Chapters[0].GetComponent<Button>().interactable = true;        //Chapter1_Btn interactable
            Database.Btn_AllStages[0][0].GetComponent<Button>().interactable = true;    //Chapter1_Stage1_Btn interactable
        }
        else
        {
            //data = new Data();
            //DataManager.BinarySerialize<Data>(data, "Data.sav");
        }
    }

    private void Load_SettingData()
    {
        data_setting = DataManager.BinaryDeserialize<Data_Setting>(Application.persistentDataPath + "/Data_Setting.sav");

        if (data_setting != null)
        {
            CharacterSelectManager.selectedCharacter = data_setting.character;
            if (data_setting.character == 0) Database.Player = Instantiate(unit1, Vector3.zero, Quaternion.identity);
            else if (data_setting.character == 1) Database.Player = Instantiate(unit2, Vector3.zero, Quaternion.identity);
            else if (data_setting.character == 2) Database.Player = Instantiate(unit3, Vector3.zero, Quaternion.identity);
            else Debug.Log("character prefab error");
            Database.Player.SetActive(false);

            SettingData.BGMVolume = data_setting.BGMVolume;
            SettingData.SoundVolume = data_setting.soundVolume;
            SettingData.ControlMode_Button = data_setting.control_Button;
            SettingData.SoundOn = data_setting.soundOn;
        }
        else
        {
            data_setting = new Data_Setting();

            data_setting.character = 0;
            Database.Player = Instantiate(unit1, Vector3.zero, Quaternion.identity);
            Database.Player.SetActive(false);

            SettingData.BGMVolume = 0.5f;
            SettingData.SoundVolume = 0.5f;
            SettingData.SoundOn = true;
        }            
    }

    public static void Save_SettingData()
    {
        Data_Setting data_setting = new Data_Setting();

        data_setting.character = CharacterSelectManager.selectedCharacter;       //선택된 캐릭터 정보 저장

        data_setting.BGMVolume = SettingData.BGMVolume;
        data_setting.soundVolume = SettingData.SoundVolume;
        data_setting.control_Button = SettingData.ControlMode_Button;
        data_setting.soundOn = SettingData.SoundOn;

        DataManager.BinarySerialize<Data_Setting>(data_setting, Application.persistentDataPath + "/Data_Setting.sav");
    }
}