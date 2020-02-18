using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStages : MonoBehaviour
{
    public GameObject button;
    public GameObject canvas;

    void Awake()
    {
        if (Test.Canvases.Count > Test.FocusChapter)
        {
            Test.Canvas.SetActive(true);
        }
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

    public void loadStage()
    {
        int index;

        for (index = 0; Test.Btn_Stages[index] != this.gameObject; index++) ;

        Test.FocusStage = index;

        ChangeScene_MapEdit();
    }

    public void newStage()
    {
        GameObject Button;

        Test.FocusStage = Test.Stages.Count;
        
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
        if (Test.Btn_Stages.Count != 0)
            Test.Canvas.SetActive(false);
        SceneManager.LoadScene("Chapters");
    }
}

