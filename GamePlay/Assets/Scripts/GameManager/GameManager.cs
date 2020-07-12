using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{ 
    public GameObject StageClearUI;     //Clear 했을 때 나타나는 Panel
    public GameObject MenuUI;
    public GameObject Minimap;

    public List<Image> img_Score;       //점수
    public Button btn_Back;             //뒤로가기 버튼
    public Text txt_PlayerMoves;        //Player 이동 횟수
    public List<CanvasGroup> fadeGroup;
    public static int playerMoves;

    private Exit exit;
    private int clickNum = 0;
    private float clicktime = 0;
    private const float clickdelay = 0.5f;

    private float pressedPoint_x;
    private float pressedPoint_y;


    private void Awake()
    {
        AudioManager.Instance.playBGM(AudioType.GAMEPLAY_BGM);
        
        InitStage();
        Database.Stage.SetActive(true);
        Database.Player.SetActive(true);
        exit = Database.Stage.GetComponent<Stage>().GetElements()[0].GetComponent<Exit>();
    }

    //Player 이동횟수 업데이트
    void Update()
    {
        if (exit.isCollide)
        {
            exit.isCollide = false;
            ShowStageClearUI();
        }

        txt_PlayerMoves.text = playerMoves.ToString();
    }

    void OnGUI()
    {
        //Button Control Mode가 아닐 때 터치로 Control
        if (!SettingData.ControlMode_Button)
        {
            Event m_Event = Event.current;

            //Mouse Pressed
            if (m_Event.type == EventType.MouseDown)
            {
                clickNum++;
                if (clickNum == 1)
                    clicktime = Time.time;
                if (clickNum > 1)
                {
                    if (Time.time - clicktime < clickdelay)
                    {
                        SettingData.Camera_Zoom = !SettingData.Camera_Zoom;
                        Minimap.SetActive(SettingData.Camera_Zoom);
                        clickNum = 0;
                        clicktime = 0;
                    }
                    else
                    {
                        clickNum = 1;
                        clicktime = Time.time;
                    }
                }

                pressedPoint_x = Input.mousePosition.x;
                pressedPoint_y = Input.mousePosition.y;
            }

            //Mouse Released
            if (m_Event.type == EventType.MouseUp && EventSystem.current.currentSelectedGameObject == null)
            {
                Vector3 direction = Vector3.zero;
                
                //Rhombus일 때
                if (!SettingData.CameraAngle_Rectangle)
                {
                    if (pressedPoint_x < Input.mousePosition.x - 50)    // 오른쪽 위 or 오른쪽 아래일 때
                    {
                        if (pressedPoint_y < Input.mousePosition.y - 50)
                            direction = new Vector3(1, 0, 0);
                        else
                            direction = new Vector3(0, 0, -1);
                    }
                    else if(Input.mousePosition.x < pressedPoint_x - 50) // 왼쪽 위 or 왼쪽 아래일 때
                    {
                        if (pressedPoint_y < Input.mousePosition.y - 50) // 왼쪽 위
                            direction = new Vector3(0, 0, 1);
                        else                                             // 왼쪽 아래
                            direction = new Vector3(-1, 0, 0);
                    }
                }
                //Rectangle일 때
                else
                {
                    if (Mathf.Abs(pressedPoint_x - Input.mousePosition.x) > Mathf.Abs(pressedPoint_y - Input.mousePosition.y)) // x축의 변화가 더 클 때 - 좌우일 때
                    {
                        if (pressedPoint_x < Input.mousePosition.x - 50)  // 왼쪽
                            direction = new Vector3(1, 0, 0);
                        else                                         // 오른쪽
                            direction = new Vector3(-1, 0, 0);
                    }
                    else if (Mathf.Abs(pressedPoint_x - Input.mousePosition.x) < Mathf.Abs(pressedPoint_y - Input.mousePosition.y))
                    {
                        if (pressedPoint_y < Input.mousePosition.y - 50)
                            direction = new Vector3(0, 0, 1);
                        else
                            direction = new Vector3(0, 0, -1);
                    }
                }
                if (Player.canMove && direction != Vector3.zero)
                {
                    Database.Player.GetComponent<Player>().SetDirection(direction);
                    Database.Player.GetComponent<Player>().move(direction);
                    clickNum = 0;
                    clicktime = 0;
                }
            }
        }
    }

    //Stage Clear 했을 때 나타나는 UI(Panel)
    private void ShowStageClearUI()
    {
        Player.canMove = false;
        btn_Back.interactable = false;
        StageClearUI.SetActive(true);

        Database.Stage.GetComponent<Stage>().CalcScore();   //점수 계산
        SaveLoadManager.Save_ClearData();                   //StageClear 저장

        StartCoroutine(popUp());

        Database.Stage.GetComponent<Stage>().SetActiveStageScore();     //StageScene에서 점수 표시
        Database.Chapter.SetActiveChapterScore();                       //ChapterScene에서 점수 표시
    }

    //Menu Button 눌렀을 때 나타나는 UI(Panel)
    public void ShowMenuUI()
    {
        Time.timeScale = 0;
        Player.canMove = false;
        btn_Back.interactable = false;
        MenuUI.SetActive(true);
        MenuUI.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = (Sprite)Resources.Load("button/soundButton" + SettingData.SoundOn, typeof(Sprite));
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Player.canMove = true;
        btn_Back.interactable = true;
        MenuUI.SetActive(false);
    }

    public void SoundOnOff()
    {
        SettingData.SoundOn = !SettingData.SoundOn;
        SaveLoadManager.Save_SettingData();
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("button/soundButton" + SettingData.SoundOn, typeof(Sprite));
    }

    public void BackToHome()
    {
        ResumeGame();
        ChangeScene_Stages();
    }

    //StageClear UI에서 NextStage 버튼 클릭 시
    public void Btn_NextStage()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);

        //다음 Stage가 같은 Chapter인 경우
        if (Database.FocusStage + 1 < Database.Stages.Count)
        {
            Database.Stage.SetActive(false);
            OpenNextStageBtn();
            InitStage();
            Database.FocusStage++;
            Database.Stage.SetActive(true);
        }
        //다음 Stage가 다른 Chapter인 경우
        else if (Database.FocusChapter + 1 < Database.Chapters.Count)
        {
            OpenNextStageBtn();
            ChangeScene_Chapters();
        }
        //모든 Stage를 클리어했을 경우
        else
        {
            OpenNextStageBtn();
            InitStage();
            ChangeScene_Chapters();
        }
        InitStage();
        exit = Database.Stage.GetComponent<Stage>().GetElements()[0].GetComponent<Exit>();
    }

    private void OpenNextStageBtn()
    {
        //다음 Stage가 같은 Chapter인 경우
        if (Database.FocusStage + 1 < Database.Stages.Count)
        {
            Database.Btn_AllStages[Database.FocusChapter][Database.FocusStage + 1].GetComponent<Button>().interactable = true;
        }
        //다음 Stage가 다른 Chapter인 경우
        else if (Database.FocusChapter + 1 < Database.Chapters.Count)
        {
            Database.Btn_Chapters[Database.FocusChapter + 1].GetComponent<Button>().interactable = true;
            Database.Btn_AllStages[Database.FocusChapter + 1][0].GetComponent<Button>().interactable = true;

            Database.Chapter.SetActiveChapterScore(Database.FocusChapter + 1);
        }
        //모든 stage를 깼을 경우
        else
        {

        }
    }

    private void InitStage()
    {
        playerMoves = 0;
        Player.canMove = true;
        Player.isCollide = false;
        btn_Back.interactable = true;
        Database.Player.GetComponent<Player>().SetVelocityZero();
        Database.Player.GetComponent<Player>().MoveToInitPos();
        Database.Player.GetComponent<Player>().initUnitImage();

        img_Score[0].gameObject.SetActive(false);
        img_Score[1].gameObject.SetActive(false);
        img_Score[2].gameObject.SetActive(false);
        StageClearUI.SetActive(false);
    }

    public void ChangeScene_Stages()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        if (Database.Stage.GetComponent<Stage>().IsClear())
            OpenNextStageBtn();

        Database.Stage.SetActive(false);
        Database.Player.SetActive(false);
        SceneManager.LoadScene("3_Stages");
    }

    public void ChangeScene_Chapters()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        Database.Stage.SetActive(false);
        Database.Player.SetActive(false);
        SceneManager.LoadScene("2_Chapters");
    }

    public void Btn_RestartStage()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        InitStage();
    }

    private void FadeIn(List<CanvasGroup> cg)
    {
        StartCoroutine(FadeCanvasGroup(cg, 0, 1));
    }
    private void FadeOut(List<CanvasGroup> cg, string SceneName)
    {
        StartCoroutine(FadeCanvasGroup(cg, 1, 0, SceneName));
    }

    public IEnumerator FadeCanvasGroup(List<CanvasGroup> cg, float start, float end, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            for (int i = 0; i < cg.Count; i++)
                cg[i].alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeCanvasGroup(List<CanvasGroup> cg, float start, float end, string SceneName, float lerpTime = 0.2f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            for (int i = 0; i < cg.Count; i++)
                cg[i].alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator popUp(int iterateTime = 0, float lerpTime = 0.3f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted;// = Time.time - _timeStartedLerping;
        float percentageComplete;// = timeSinceStarted / lerpTime;
        float currentValue;
        float end;

        if (iterateTime < Database.Stage.GetComponent<Stage>().GetCurrentScore())
        {
            end = img_Score[iterateTime].transform.localScale.x;
            img_Score[iterateTime].gameObject.SetActive(true);
            while (true)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = Mathf.Pow(timeSinceStarted / lerpTime, 2);
                currentValue = Mathf.Lerp(0, end * 1.3f, percentageComplete);

                img_Score[iterateTime].transform.localScale = new Vector2(currentValue, currentValue);

                if (percentageComplete >= 1) break;
                yield return null;
            }

            _timeStartedLerping = Time.time;
            lerpTime /= 1.5f;

            while (true)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = Mathf.Pow(timeSinceStarted / lerpTime, 2);
                currentValue = Mathf.Lerp(end * 1.3f, end, percentageComplete);

                img_Score[iterateTime].transform.localScale = new Vector2(currentValue, currentValue);

                if (percentageComplete >= 1) break;
                yield return null;
            }

            StartCoroutine(popUp(iterateTime + 1));
        }
    }
}