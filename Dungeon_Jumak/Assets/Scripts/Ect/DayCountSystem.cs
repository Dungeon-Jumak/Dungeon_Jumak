using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCountSystem : MonoBehaviour
{
    private Data data;
    [SerializeField] private TextMeshProUGUI dayTmp;
    [SerializeField] private GameObject daysUI;
    private bool hasRunOnce = false;

    void Start()
    {
        data = DataManager.Instance.data;
        hasRunOnce = false;
}

    // Update is called once per frame
    void Update()
    {
        if(data.dayCount && !hasRunOnce)
        {
            data.days += 1;
            daysUI.SetActive(true);
            dayTmp.text = data.days.ToString() + "일";

            data.dayCount = false;
            hasRunOnce = true;
        }
    }
}
