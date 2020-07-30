using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    //prefab
    public List<GameObject> pre_characters;

    public List<GameObject> characterContents;
    public List<GameObject> characters;
    public Canvas UI_Option;
    public List<Button> Btns_Canvas;
    public List<Button> Btns_Option;
    public Image img_selected;
    private static int focusCharacter = 0;
    public static int selectedCharacter = 0;

    private void Awake()
    {
        SetVisible(focusCharacter);
        Check_BtnInteractable();
        Check_isSelected();
    }

    private void SetVisible(int charcNum)
    {
        characters[charcNum].SetActive(true);
    }

    private void SetInvisible(int charcNum)
    {
        characters[charcNum].SetActive(false);
    }

    public void Btn_Left()
    {
        SetInvisible(focusCharacter--);
        SetVisible(focusCharacter);
        Check_BtnInteractable();
        Check_isSelected();
    }

    public void Btn_Right()
    {
        SetInvisible(focusCharacter++);
        SetVisible(focusCharacter);
        Check_BtnInteractable();
        Check_isSelected();
    }

    public void Btn_Select()
    {
        selectedCharacter = focusCharacter;
        Check_isSelected();

        Destroy(Database.Player.gameObject);
        Database.Player = Instantiate(pre_characters[selectedCharacter], Vector3.zero, Quaternion.identity);
        Database.Player.SetActive(false);
    }

    private void Check_BtnInteractable()
    {
        if (focusCharacter == 0)
            Btns_Canvas[0].gameObject.SetActive(false);    //left btn
        else if (focusCharacter == (characters.Count - 1))
            Btns_Canvas[1].gameObject.SetActive(false);    //right btn
        else
        {
            Btns_Canvas[0].gameObject.SetActive(true);
            Btns_Canvas[1].gameObject.SetActive(true);
        }
    }

    private void Check_isSelected()
    {
        if (focusCharacter == selectedCharacter)
            img_selected.gameObject.SetActive(true);
        else
            img_selected.gameObject.SetActive(false);
    }

    public void Change_CharacterFeature(int selectNum)
    {
        switch (selectNum)
        {
            case 1:
                Debug.Log("red");
                characters[focusCharacter].GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 2:
                Debug.Log("green");
                characters[focusCharacter].GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case 3:
                Debug.Log("blue");
                characters[focusCharacter].GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
        }
    }

    public void ShowOptionPanel()
    {
        for (int i = 0; i < Btns_Canvas.Count; i++)
            Btns_Canvas[i].interactable = false;
        UI_Option.gameObject.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        for (int i = 0; i < Btns_Canvas.Count; i++)
            Btns_Canvas[i].interactable = true;
        UI_Option.gameObject.SetActive(false);
    }

    public void ChangeScene_Start()
    {
        AudioManager.Instance.playSound(AudioType.BUTTON_SOUND);
        SceneManager.LoadScene("1_Start");
    }
}