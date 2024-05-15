using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DunjeonScene : MonoBehaviour
{
    private GameObject secTextObj;
    private GameObject minTextObj;

    private TextMeshProUGUI secText;
    private TextMeshProUGUI minText;

    private float time = 120;

    void Start()
    {
        //---타이머 관련 변수 할당---//
        secTextObj = GameObject.Find("SecText");
        secText = secTextObj.GetComponent<TextMeshProUGUI>();
        minTextObj = GameObject.Find("MinText");
        minText = minTextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        countTime();
    }

    private void countTime()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            int min = (int)time / 60;
            int sec = ((int)time - min * 60) % 60;

            secText.text = sec.ToString("00");
            minText.text = min.ToString("00");
        }
        else
        {
            // 타이머가 0 이하가 되면 타이쿤 파트처럼 결과나오는 팝업창이 나오고 거기서 터치하면 날짜 넘어가도록
        }
    }

}
