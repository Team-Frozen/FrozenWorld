using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private List<GameObject> elements;
    private GameObject gameArea;
    private int stageNum;
    private int stageSize;
    private int minMove;

    //prefab
    public GameObject pre_gameArea;

    void Awake()
    {
        stageSize = 9;
        elements = new List<GameObject>();
        createGameArea();
        DontDestroyOnLoad(this.gameObject);
    }

    public List<GameObject> getElements()
    {
        return elements;
    }

    private void createGameArea()
    {
        gameArea = Instantiate(pre_gameArea, new Vector3(0, 0, 0), Quaternion.identity);
        gameArea.transform.SetParent(this.transform);
        modifyArea();
    }
    private void modifyArea()
    {
        gameArea.transform.GetChild(0).localScale = new Vector3(stageSize, 1, stageSize);
        gameArea.transform.GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)stageSize * 0.5f, (float)stageSize * 0.5f);

        gameArea.transform.GetChild(1).position = new Vector3(-(float)stageSize * 0.5f - 0.001f, 0, 0);
        gameArea.transform.GetChild(1).localScale = new Vector3(0.1f, 1, (float)stageSize * 0.1f);

        gameArea.transform.GetChild(2).position = new Vector3(0, 0, -(float)stageSize * 0.5f - 0.001f);
        gameArea.transform.GetChild(2).localScale = new Vector3(0.1f, 1, (float)stageSize * 0.1f);
    }

    public bool hasElementOn(Vector3 pos)
    {
        foreach (GameObject element in elements)
        {
            if (element.transform.position.x == pos.x && element.transform.position.z == pos.z)
                return true;
        }
        return false;
    }

    public int getStageSize()
    {
        return stageSize;
    }

    public void setStageSize(int stageSize)
    {
        this.stageSize = stageSize;
        clearElements();
        modifyArea();
    }

    public int getMinMove()
    {
        return minMove;
    }

    public void setMinMove(int minMove)
    {
        this.minMove = minMove;
    }

    public void setParent(GameObject child)
    {
        child.transform.SetParent(this.transform);
    }

    public void clearElements()
    {
        foreach (GameObject element in elements)
        {
            Destroy(element);
        }
        elements.Clear();
    }
}
