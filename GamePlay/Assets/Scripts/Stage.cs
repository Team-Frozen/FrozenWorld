using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    List<GameObject> elements;
    GameObject gameArea;
    Transform tf_player;

    [SerializeField] int stageNum { get; }
    [SerializeField] int stageSize = 9;
    [SerializeField] int minimumMoves { get; }
    int score = 0;

    public void CalcScore()
    {
        if (GameManager.playerMoves == minimumMoves)
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

    public List<GameObject> GetElements()
    {
        return elements;
    }

    public int GetStageSize()
    {
        return stageSize;
    }

    public int GetScore()
    {
        return score;
    }
}
