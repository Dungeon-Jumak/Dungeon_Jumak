using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaPageControl : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] pagePopup;

    [SerializeField] 
    private int pageNum;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button prevButton;

    public void NextPageDisplay()
    {
        if (pageNum >= pagePopup.Length) 
        {
            pageNum = pagePopup.Length - 1;
            pagePopup[pageNum].SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else if (pageNum >= 0 && pageNum < pagePopup.Length)
        {
            pagePopup[pageNum - 1].SetActive(false);
            pagePopup[pageNum].SetActive(true);
        }
    }

    public void NextPage()
    {
        pageNum++;
        NextPageDisplay();
        UpdateButtonState();
    }

    public void PreviousPageDisplay()
    {
        if (pageNum < 0)
        {
            pageNum = 0;
            pagePopup[pageNum].SetActive(true);
            prevButton.gameObject.SetActive(false);
        }
        else if (pageNum >= 0 && pageNum < pagePopup.Length)
        {
            pagePopup[pageNum + 1].SetActive(false);
            pagePopup[pageNum].SetActive(true);
        }
    }

    public void PreviousPage()
    {
        pageNum--;
        PreviousPageDisplay();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(pageNum > 0);
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(pageNum < pagePopup.Length - 1);
        }
    }


}
