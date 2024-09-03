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
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI seasonText;

    private void Start()
    {
        //DataManager
        data = DataManager.Instance.data;

        //Active calender
        if (!data.isFirstStart)
        {
            if (data.lastday != data.day)
            {
                calenderPopup.SetActive(true);
                data.lastday = data.day;
            }
        }

        //Update the calender's texts
        if (data.year >= 1)
            yearText.text = "경영 " + data.year.ToString() + "년차";

        dayText.text = data.day.ToString() + "일";
        seasonText.text = data.season[data.seasonNum];

        //Nonactive calender
        StartCoroutine(HideCalenderPanel(2f));
    }

    //Coroutine for hide calender
    private IEnumerator HideCalenderPanel(float delay)
    {
        yield return new WaitForSeconds(delay);

        calenderPopup.SetActive(false);
    }
}
