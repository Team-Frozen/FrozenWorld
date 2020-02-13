using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMain : MonoBehaviour
{
    public GameObject stage;
    public GameObject button;

    void Awake()
    {
        if(Test.Buttons.Count != 0)
            Test.Buttons[0].transform.parent.gameObject.SetActive(true);
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

        for (index = 0; Test.Buttons[index] != this.gameObject; index++) ;

        Test.FocusStage = index;
        
        ChangeScene_MapEdit();
    }


    public void newStage()
    {
        GameObject Button;
        
        Button = Instantiate(button, new Vector3(50 + 100 * (Test.Buttons.Count % 9), -140 - 100 * (Test.Buttons.Count / 9), 0), Quaternion.identity);
        Button.transform.SetParent(GameObject.Find("Canvas_Load").transform, false);
        Test.Buttons.Add(Button);

        Test.FocusStage = Test.Stages.Count;

        ChangeScene_MapEdit();
    }

    private void ChangeScene_MapEdit()
    {
        Test.Buttons[0].transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene("MapEdit");
    }

}

