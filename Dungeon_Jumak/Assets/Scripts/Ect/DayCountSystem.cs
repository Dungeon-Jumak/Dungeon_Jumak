//System
using System.Collections;
using System.Collections.Generic;

//Engine
using UnityEngine;

//Text
using TMPro;

public class DayCountSystem : MonoBehaviour
{
    private Data data;

    [SerializeField] private GameObject calenderPopup;

    //Date variable
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI seasonText;

    private void Start()
    {
        //DataManager
        data = DataManager.Instance.data;
    }

    private void OnEnable()
    {
        //Update the calender's texts
        dayText.text = data.Countday.ToString() + "일";
        seasonText.text = data.CountSeason;

        //Active calender
        calenderPopup.SetActive(true);

        //Nonactive calender
        StartCoroutine(HideCalenderPanel(1f));
    }

    //Coroutine for hide calender
    private IEnumerator HideCalenderPanel(float delay)
    {
        yield return new WaitForSeconds(delay);

        calenderPopup.SetActive(false);
    }
}
