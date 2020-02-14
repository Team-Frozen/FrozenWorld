using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerChapter : MonoBehaviour
{
    public GameObject button;

    void Awake()
    {
        if (Test.Btn_Chapters.Count != 0)
            Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadChapter()
    {
        int index;

        for (index = 0; Test.Btn_Chapters[index] != this.gameObject; index++) ;

        Test.FocusChapter = index;

        ChangeScene_Stages();
    }


    public void newChapter()
    {
        GameObject Button;

        Button = Instantiate(button, new Vector3(50 + 300 * (Test.Btn_Chapters.Count), 0, 0), Quaternion.identity);
        Button.transform.SetParent(GameObject.Find("Canvas_Load_Chapters").transform, false);
        Test.Btn_Chapters.Add(Button);

        Test.FocusChapter = Test.Chapters.Count;
        Test.Chapters.Add(new List<GameObject>());
        Test.Btn_AllStages.Add(new List<GameObject>());

        ChangeScene_Stages();
    }

    private void ChangeScene_Stages()
    {
        Test.Btn_Chapters[0].transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene("Stages");
    }
}
