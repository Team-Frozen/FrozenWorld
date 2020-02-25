﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    static List<List<GameObject>> chapters = new List<List<GameObject>>();

    static List<List<GameObject>> btn_Stages = new List<List<GameObject>>();
    static List<GameObject> btn_Chapters = new List<GameObject>();

    static List<GameObject> canvases = new List<GameObject>();

    static int focusStage = 0;
    static int focusChapter = 0;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public static List<List<GameObject>> Chapters
    {
        get
        {
            return chapters;
        }
        set
        {
            chapters = value;
        }
    }

    public static List<GameObject> Stages
    {
        get
        {
            return chapters[focusChapter];
        }
        set
        {
            chapters[focusChapter] = value;
        }
    }

    public static GameObject Stage
    {
        get
        {
            return Stages[focusStage];
        }

        set
        {
            Stages[focusStage] = value;
        }
    }


    public static List<GameObject> Canvases
    {
        get
        {
            return canvases;
        }
        set
        {
            canvases = value;
        }
    }

    public static GameObject Canvas
    {
        get
        {
            return canvases[FocusChapter];
        }
        set
        {
            canvases[FocusChapter] = value;
        }
    }


    public static List<List<GameObject>> Btn_AllStages
    {
        get
        {
            return btn_Stages;
        }
        set
        {
            btn_Stages = value;
        }
    }

    public static List<GameObject> Btn_Chapters
    {
        get
        {
            return btn_Chapters;
        }
        set
        {
            btn_Chapters = value;
        }
    }

    public static List<GameObject> Btn_Stages
    {
        get
        {
            return btn_Stages[focusChapter];
        }
        set
        {
            btn_Stages[focusChapter] = value;
        }
    }
    

    public static int FocusStage
    {
        get
        {
            return focusStage;
        }
        set
        {
            focusStage = value;
        }
    }

    public static int FocusChapter
    {
        get
        {
            return focusChapter;
        }
        set
        {
            focusChapter = value;
        }
    }
}