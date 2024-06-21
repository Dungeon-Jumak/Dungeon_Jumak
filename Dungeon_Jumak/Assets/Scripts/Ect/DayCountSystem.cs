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

    void FixedUpdate()
    {
        if (data.dayCount && !hasRunOnce)
        {
            data.dayCount = false;
            hasRunOnce = true;
            daysUI.SetActive(true);
            data.Countday += 1;
            dayTmp.text = data.Countday.ToString() + "일";
            StartCoroutine(HideDaysUI(1f));
        }
    }
    private IEnumerator HideDaysUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        daysUI.SetActive(false);
        hasRunOnce = false;
    }
}
