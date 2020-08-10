﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Database
{
    private static GameObject player;
    private static List<Chapter> chapters = new List<Chapter>();
    private static List<List<GameObject>> stages = new List<List<GameObject>>();

    private static List<List<GameObject>> btn_Stages = new List<List<GameObject>>();
    private static List<GameObject> btn_Chapters = new List<GameObject>();

    private static List<GameObject> canvases = new List<GameObject>();

    private static int focusStage = 0;
    private static int focusChapter = 0;

    public static GameObject Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
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

    public static List<Chapter> Chapter_List
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

    public static Chapter Chapter
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

    public static List<List<GameObject>> Chapters
    {
        get
        {
            return stages;
        }
        set
        {
            stages = value;
        }
    }

    public static List<GameObject> Stages
    {
        get
        {
            return stages[focusChapter];
        }
        set
        {
            stages[focusChapter] = value;
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
            return canvases[focusChapter];
        }
        set
        {
            canvases[focusChapter] = value;
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