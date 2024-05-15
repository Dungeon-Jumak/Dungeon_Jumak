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
        //---Ÿ�̸� ���� ���� �Ҵ�---//
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
            // Ÿ�̸Ӱ� 0 ���ϰ� �Ǹ� Ÿ���� ��Ʈó�� ��������� �˾�â�� ������ �ű⼭ ��ġ�ϸ� ��¥ �Ѿ����
        }
    }

}
