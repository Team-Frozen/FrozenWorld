using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private  List<GameObject> elements;
    private GameObject gameArea;
    private int stageSize;

    private Vector3 playerPos;
    private int playerProperty;

    private bool isClear = false;
    private int minMove;
    private int score = 0;
    private int currentScore = 0;

    //prefab//
    public GameObject pre_gameArea;

    void Awake()
    {
        elements = new List<GameObject>();
        CreateGameArea();
        DontDestroyOnLoad(this.gameObject);
    }

    public void CalcScore()
    {
        if (GameManager.playerMoves <= minMove)
            currentScore = 3;
        else if (minMove < GameManager.playerMoves && GameManager.playerMoves < minMove + 3)
            currentScore = 2;
        else
            currentScore = 1;

        if (currentScore > score)
            score = currentScore;
    }

    private void CreateGameArea()
    {
        gameArea = Instantiate(pre_gameArea, new Vector3(0, 0, 0), Quaternion.identity);
        gameArea.transform.SetParent(this.transform);
        gameArea.transform.localPosition = new Vector3(0, 0, 0);
        SetArea();
    }

    private void SetArea()
    {
        gameArea.transform.GetChild(0).localScale = new Vector3(stageSize, 0.1f, stageSize);
        gameArea.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)stageSize * 0.5f, (float)stageSize * 0.5f);

        gameArea.transform.GetChild(1).localPosition = new Vector3(-(float)stageSize * 0.5f - 0.001f, 0.1f, 0);
        gameArea.transform.GetChild(1).localScale = new Vector3((float)stageSize * 0.1f, 1, 0.08f);
        gameArea.transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)stageSize, 1);

        gameArea.transform.GetChild(2).localPosition = new Vector3(0, 0.1f, -(float)stageSize * 0.5f - 0.001f);
        gameArea.transform.GetChild(2).localScale = new Vector3((float)stageSize * 0.1f, 1, 0.08f);
        gameArea.transform.GetChild(2).GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)stageSize, 1);

        gameArea.transform.GetChild(3).localPosition = new Vector3(0, 2, -(float)stageSize * 0.5f - 0.5f);
        gameArea.transform.GetChild(4).localPosition = new Vector3(-(float)stageSize * 0.5f - 0.5f, 2, 0);
        gameArea.transform.GetChild(5).localPosition = new Vector3(0, 2, (float)stageSize * 0.5f + 0.5f);
    }

    public void SetActiveStageScore()
    {
        for (int i = 0; i < 3; i++)
            Database.Btn_AllStages[Database.FocusChapter][Database.FocusStage].transform.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < score; i++)
            Database.Btn_AllStages[Database.FocusChapter][Database.FocusStage].transform.GetChild(i).gameObject.SetActive(true);
    }

    public GameObject GetElementOn(Vector3 pos)
    {
        foreach (GameObject element in elements)
        {
            if (Mathf.Abs(element.transform.position.x - pos.x) < 10e-2 && Mathf.Abs(element.transform.position.z - pos.z) < 10e-2)
                return element;
        }
        return null;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }
    public Vector3 GetPlayerPos()
    {
        return playerPos;
    }

    public void SetPlayerProperty(int property)
    {
        playerProperty = property;
    }

    public int GetPlayerProperty()
    {
        return playerProperty;
    }

    public void SetParent(GameObject child)
    {
        child.transform.SetParent(this.transform);
    }

    public List<GameObject> GetElements()
    {
        return this.elements;
    }

    public int GetStageSize()
    {
        return this.stageSize;
    }

    public void SetStageSize(int stageSize)
    {
        this.stageSize = stageSize;
        SetArea();
    }

    public void SetMinMove(int minMove)
    {
        this.minMove = minMove;
    }

    public int GetScore()
    {
        return this.score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetCurrentScore()
    {
        return this.currentScore;
    }

    public bool IsClear()
    {
        return this.isClear;
    }

    public void SetIsClear(bool isClear)
    {
        this.isClear = isClear;
    }

    public GameObject getGameArea()
    {
        return gameArea;
    }
}
