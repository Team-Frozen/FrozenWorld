using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStages : MonoBehaviour
{
//----------- Prefabs-------------//
    public GameObject button;     //
    public GameObject canvas;     //
//--------------------------------//

    private static GameObject focusButton;

    void Awake()
    {
        focusButton = null;

        if (Test.Canvases.Count > Test.FocusChapter)
            Test.Canvas.SetActive(true);
        else
            Test.Canvases.Add(Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity));

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkBtnState()
    {
        var colors = GetComponent<Button>().colors;

        if (focusButton == null || focusButton != this.gameObject)
        {
            if (focusButton != null)
                focusButton.GetComponent<Image>().color = Color.white;
            focusButton = this.gameObject;
            focusButton.GetComponent<Image>().color = Color.red;
        }
        else
            loadStage();
    }


    public void loadStage()
    {
        int index;

        for (index = 0; Test.Btn_Stages[index] != this.gameObject; index++) ;

        Test.FocusStage = index;
        focusButton.GetComponent<Image>().color = Color.white;
        ChangeScene_MapEdit();
    }

    public void newStage()
    {
        GameObject Button;

        if (focusButton == null)
            Test.FocusStage = Test.Stages.Count;
        else {
            int index;
            for (index = 0; Test.Btn_Stages[index] != focusButton; index++) ;
            Test.FocusStage = index + 1;
            focusButton.GetComponent<Image>().color = Color.white;
        }
        Button = Instantiate(button, new Vector3(50 + 100 * (Test.Btn_Stages.Count % 9), -140 - 100 * (Test.Btn_Stages.Count / 9), 0), Quaternion.identity);
        Button.transform.SetParent(Test.Canvas.transform, false);
        Test.Btn_Stages.Add(Button);

        ChangeScene_MapEdit();
    }

    private void ChangeScene_MapEdit()
    {
        Test.Canvas.SetActive(false);
        SceneManager.LoadScene("MapEdit");
    }

    public void ChangeScene_Chapters()
    {
        if (focusButton != null)
            focusButton.GetComponent<Image>().color = Color.white;

        if (Test.Btn_Stages.Count != 0)
            Test.Canvas.SetActive(false);
        SceneManager.LoadScene("Chapters");
    }

    public void removeStage()
    {
        if (focusButton != null)
        {
            int index;

            for (index = 0; Test.Btn_Stages[index] != focusButton; index++) ;

            Destroy(Test.Btn_Stages[Test.Btn_Stages.Count - 1]);
            Test.Btn_Stages.RemoveAt(Test.Btn_Stages.Count - 1);

            Destroy(Test.Stages[index]);
            Test.Stages.RemoveAt(index);

            focusButton.GetComponent<Image>().color = Color.white;
            focusButton = null;
        }
    }
}

