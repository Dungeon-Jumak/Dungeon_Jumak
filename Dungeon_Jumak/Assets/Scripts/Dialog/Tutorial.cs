//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TMPro
using TMPro;

[DisallowMultipleComponent]
public class Tutorial : MonoBehaviour
{
    //Common
    [Header("Ʃ�丮�� ��ȭ ��ũ��Ʈ �迭")]
    [SerializeField] private DialogSystem[] dialogSystem;

    [Header("���� ��ȭ ��ũ��Ʈ �ε���")]
    [SerializeField] private int currentDialogSystemIndex;

    [Header("���̾�α� �Է� ���� �г�")]
    [SerializeField] private GameObject basePanel;

    [Header("�� Ʃ�丮�� ���� ��ȣ")]
    [SerializeField] private bool houseTutorial = false;

    [Header("Ʃ�丮�� ��ư ������Ʈ �迭")]
    [SerializeField] private Button[] tutorialButtons;

    [Header("��ȭâ �г�")]
    [SerializeField] private GameObject dialogPanel;

    [Header("���� ���� ���� Ȱ��ȭ ������Ʈ")]
    [SerializeField] private GameObject[] activeObject;

    Data data;

    //Story 1
    [Header("�г��� �Է� �˾�")]
    [SerializeField] private GameObject inputNamePopUp;

    [Header("�г��� �Է� �ʵ�")]
    [SerializeField] private TextMeshProUGUI nameInputField;

    [Header("�г��� Ȯ�� �˾�")]
    [SerializeField] private Text confirmPopupText;

    //House Tutorial
    [Header("�� Ʃ�丮�� ������Ʈ")]
    [SerializeField] private GameObject houseObject;

    //Jumak Tutorial
    [Header("�ָ� Ʃ�丮�� ������Ʈ")]
    [SerializeField] private GameObject jumakObject;

    //Dungeon Tutorial
    [Header("���� Ʃ�丮�� ������Ʈ")]
    [SerializeField] private GameObject dungeonObject;

    private void Start()
    {
        data = DataManager.Instance.data;

        //Start Story 1
        if (data.isFirstStart)
            StartStory1();
        else
        {
            dialogPanel.SetActive(false);

            for (int i = 0; i < activeObject.Length; i++)
                activeObject[i].SetActive(true);
        }


    }

    private void Update()
    {
        if (currentDialogSystemIndex == 1)
            confirmPopupText.text = nameInputField.text + "�̽Ű���?";

        //Check Finish Story 1
        switch (currentDialogSystemIndex)
        {
            case 0:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 1;

                    inputNamePopUp.SetActive(true);
                }
                break;

            case 1:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 2;

                    for (int i = 0; i < tutorialButtons.Length; i++)
                    {
                        if (i != 0) tutorialButtons[i].interactable = false;
                        else tutorialButtons[i].interactable = true;
                    }

                    basePanel.SetActive(false);
                }
                break;

            case 2:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 3;

                    houseObject.SetActive(false);

                    StartDialogue();
                }
                break;

            case 3:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 4;

                    for (int i = 0; i < tutorialButtons.Length; i++)
                    {
                        if (i != 1) tutorialButtons[i].interactable = false;
                        else tutorialButtons[i].interactable = true;
                    }

                    basePanel.SetActive(false);
                }
                break;

            case 4:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 5;

                    jumakObject.SetActive(false);

                    dialogSystem[currentDialogSystemIndex].UpdateDialog();
                }
                break;

            case 5:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 6;

                    for (int i = 0; i < tutorialButtons.Length; i++)
                    {
                        if (i != 2) tutorialButtons[i].interactable = false;
                        else tutorialButtons[i].interactable = true;
                    }

                    basePanel.SetActive(false);
                }
                break;

            case 6:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 7;

                    dungeonObject.SetActive(false);

                    dialogSystem[currentDialogSystemIndex].UpdateDialog();

                    for (int i = 0; i < tutorialButtons.Length; i++)
                    {
                        if (i != 3) tutorialButtons[i].interactable = false;
                        else tutorialButtons[i].interactable = true;
                    }
                }
                break;

            case 7:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 8;

                    dialogSystem[currentDialogSystemIndex].UpdateDialog();
                }
                break;

            case 8:
                if (dialogSystem[currentDialogSystemIndex].isFinDialog)
                {
                    currentDialogSystemIndex = 9;

                    data.isFirstStart = false;

                    SceneManager.LoadScene("WaitingScene");
                }
                break;

            default:
                break;
        }

    }

    public void ConfirmName()
    {
        //Save Player Name
        data.playerName = nameInputField.text;

        //Start Story 2
        StartStory2();
    }

    private void StartStory1()
    {
        //Only Run First Start
        if (data.isFirstStart)
        {
            currentDialogSystemIndex = 0;
            dialogSystem[currentDialogSystemIndex].UpdateDialog();
        }
    }

    private void StartStory2()
    {
        dialogSystem[currentDialogSystemIndex].UpdateDialog();
    }

    public void HouseStartTutorial()
    {
        basePanel.SetActive(true);

        houseObject.SetActive(true);

        dialogSystem[currentDialogSystemIndex].UpdateDialog();
    }

    public void JumakStartTutorial()
    {
        basePanel.SetActive(true);

        jumakObject.SetActive(true);

        dialogSystem[currentDialogSystemIndex].UpdateDialog();
    }

    public void DungeonStartTutorial()
    {
        basePanel.SetActive(true);

        dungeonObject.SetActive(true);

        dialogSystem[currentDialogSystemIndex].UpdateDialog();
    }

    public void StartDialogue()
    {
        dialogSystem[currentDialogSystemIndex].UpdateDialog();
    }

    public void SkipDialog()
    {
        dialogSystem[currentDialogSystemIndex].SkipDialog();
    }
}
