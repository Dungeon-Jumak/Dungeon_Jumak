//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Bson;

[DisallowMultipleComponent]
public class DialogSystem : MonoBehaviour
{
    [Header("대화에 참여하는 캐릭터들의 UI 배열")]
    [SerializeField] private Speaker[] speakers;

    [Header("현재 분기의 대사 목록 배열")]
    [SerializeField] private DialogData[] dialogs;

    [Header("자동 시작 여부")]
    [SerializeField] private bool isAutoStart = true;

    //For Check First Time
    private bool isFirst = true;

    //For Check Button Click
    private bool isButtClick = false;

    //Current Dialog Index
    [SerializeField] private int currentDialogIndex = -1;

    //Current Speaker Index
    [SerializeField] private int currentSpeakerIndex = 0;

    //Set Up Method
    private void SetUp()
    {
        for (int i = 0; i < speakers.Length; i++)
        {
            //Set Active False
            SetActiveObjects(speakers[i], false);

            //Character Set Acitve is true
            speakers[i].image.gameObject.SetActive(true);
        }
    }

    //Update Dialog Method
    public void UpdateDialog()
    {
        //If isFirst is true
        if (isFirst)
        {
            //Init
            SetUp();

            //If isAutoStart is ture, Set next dialog
            if (isAutoStart)
                SetNextDialog();

            //Convert isFirst sign
            isFirst = false;
        }
    }

    //Next Button Method
    public void SkipDialog()
    {
        //if there is dialog left...
        if (dialogs.Length > currentDialogIndex + 1)
            SetNextDialog();
        //if there is not dialog left...
        else
        {
            //SetActive false
            for (int i = 0; i < speakers.Length; i++)
            {
                //Set Active False
                SetActiveObjects(speakers[i], false);

                //Character Set Active is False
                speakers[i].image.gameObject.SetActive(false);
            }
        }
    }

    private void SetNextDialog()
    {
        //SetActive false current speaker's objects
        SetActiveObjects(speakers[currentSpeakerIndex], false);

        //Increase Current Dialog Index
        currentDialogIndex ++;

        //Set Current Speaker Index
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

        //SetActiveObjects
        SetActiveObjects(speakers[currentSpeakerIndex], true);

        //Set Cureent Speaker name text
        speakers[currentSpeakerIndex].name.text = dialogs[currentDialogIndex].name;

        //Set Current Speaker dialog text
        speakers[currentSpeakerIndex].dialog.text = dialogs[currentDialogIndex].dialogeue;
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        //Set Active
        speaker.frame.gameObject.SetActive(visible);
        speaker.name.gameObject.SetActive(visible);
        speaker.dialog.gameObject.SetActive(visible);
        speaker.nextButton.gameObject.SetActive(visible);

        //Get Speaker's color
        Color color = speaker.image.color;

        //Change Alpha value
        color.a = visible == true ? 1 : 0.2f;

        //Change Color
        speaker.image.color = color;
    }
}

//Speaker Struture
[System.Serializable]
public struct Speaker
{
    //Speaker Character Image
    public Image image;

    //Frame Image
    public Image frame;

    //Speaker Name
    public TextMeshProUGUI name;

    //Speaker dialog
    public TextMeshProUGUI dialog;

    //Next Button
    public Button nextButton;
}

//Dialog Data
[System.Serializable]
public struct DialogData
{
    //Speaker index
    public int speakerIndex;

    //Character Name
    public string name;

    //Dialogue
    [TextArea(3, 5)]
    public string dialogeue;

}
