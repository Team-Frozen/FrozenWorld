using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Test
{
    private static List<GameObject> stages = new List<GameObject>();
    private static List<GameObject> buttons = new List<GameObject>();
    private static int focusStage = 0;

    public static List<GameObject> Buttons
    {
        get
        {
            return buttons;
        }
        set
        {
            buttons = value;
        }
    }
    public static List<GameObject> Stages
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
    public static GameObject Stage
    {
        get
        {
            return stages[focusStage];
        }

        set
        {
            stages[focusStage] = value;
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

}