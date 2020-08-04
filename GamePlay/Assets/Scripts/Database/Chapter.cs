using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter
{
    //private List<Stage> stages = new List<Stage>();
    private bool isClear = false;
    private int score = 0;  //모든 Stage의 점수 합

    private void Awake()
    {
        CalcScore(Database.FocusChapter);
    }

    public void UpdateChapterScoreTxt()
    {
        CalcScore(Database.FocusChapter);
        GameObject Score = Database.Btn_Chapters[Database.FocusChapter].transform.parent.GetChild(3).gameObject;    // Score = star_img + score_txt
        Score.transform.GetChild(1).gameObject.GetComponent<Text>().text = Database.Chapter_List[Database.FocusChapter].GetScore() + "/" + Database.Chapters[Database.FocusChapter].Count * 3;
        Score.SetActive(true);
    }

    public void UpdateChapterScoreTxt(int chapterNum)
    {
        CalcScore(chapterNum);
        GameObject Score = Database.Btn_Chapters[chapterNum].transform.parent.GetChild(3).gameObject;
        Score.transform.GetChild(1).gameObject.GetComponent<Text>().text = Database.Chapter_List[chapterNum].GetScore() + "/" + Database.Chapters[chapterNum].Count * 3;
        Score.SetActive(true);
    }

    public void CalcScore(int chapterNum)
    {
        score = 0;
        for (int i = 0; i < Database.Chapters[chapterNum].Count; i++)
            score += Database.Chapters[chapterNum][i].GetComponent<Stage>().GetScore();
    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool isClear)
    {
        this.isClear = isClear;
    }

    public int GetScore()
    {
        return score;
    }

    //public List<Stage> Stages
    //{
    //    get
    //    {
    //        return stages;
    //    }
    //    set
    //    {
    //        stages = value;
    //    }
    //}

    //public Stage Stage
    //{
    //    get
    //    {
    //        return stages[Database.FocusStage];
    //    }
    //    set
    //    {
    //        stages[Database.FocusStage] = value;
    //    }
    //}
}
