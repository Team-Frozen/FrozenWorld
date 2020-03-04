using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private  List<GameObject> elements;
    private GameObject gameArea;
    private int stageSize;

    private bool isClear = false;
    private int minimumMoves = 3;   //임시값
    private int score = 0;

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
        if (GameManager.playerMoves <= minimumMoves)
        {
            score = 3;
        }
        else if (minimumMoves < GameManager.playerMoves && GameManager.playerMoves < minimumMoves + 3)
        {
            score = 2;
        }
        else
        {
            score = 1;
        }
    }

    private void CreateGameArea()
    {
        gameArea = Instantiate(pre_gameArea, new Vector3(0, 0, 0), Quaternion.identity);
        gameArea.transform.SetParent(this.transform);
        SetArea();
    }

    private void SetArea()
    {
        gameArea.transform.GetChild(0).localScale = new Vector3(stageSize, 1, stageSize);
        gameArea.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)stageSize * 0.5f, (float)stageSize * 0.5f);

        gameArea.transform.GetChild(1).position = new Vector3(-(float)stageSize * 0.5f - 0.001f, 0, 0);
        gameArea.transform.GetChild(1).localScale = new Vector3(0.1f, 1, (float)stageSize * 0.1f);

        gameArea.transform.GetChild(2).position = new Vector3(0, 0, -(float)stageSize * 0.5f - 0.001f);
        gameArea.transform.GetChild(2).localScale = new Vector3(0.1f, 1, (float)stageSize * 0.1f);

        gameArea.transform.GetChild(3).position = new Vector3(0, 2, -(float)stageSize * 0.5f - 0.5f);
        gameArea.transform.GetChild(4).position = new Vector3(-(float)stageSize * 0.5f - 0.5f, 2, 0);
        gameArea.transform.GetChild(5).position = new Vector3((float)stageSize * 0.5f + 0.5f, 2, 0);
        gameArea.transform.GetChild(6).position = new Vector3(0, 2, (float)stageSize * 0.5f + 0.5f);
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

    public int GetScore()
    {
        return this.score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public bool IsClear()
    {
        return this.isClear;
    }

    public void SetIsClear(bool isClear)
    {
        this.isClear = isClear;
    }
}
