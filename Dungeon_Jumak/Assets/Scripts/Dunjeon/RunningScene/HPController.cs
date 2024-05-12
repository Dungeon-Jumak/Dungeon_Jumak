using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public GameObject hpImage1;
    public GameObject hpImage2;
    public GameObject hpImage3;

    private Image hpImg1;
    private Image hpImg2;
    private Image hpImg3;

    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Start()
    {
        hpImg1 = hpImage1.GetComponent<Image>();
        hpImg2 = hpImage2.GetComponent<Image>();
        hpImg3 = hpImage3.GetComponent<Image>();
    }

    void Update()
    {
        updateHPImg();
    }

    private void updateHPImg()
    {
        if (data.playerHP == 3)
        {
            hpImg1.fillAmount = 1f;
            hpImg2.fillAmount = 1f;
            hpImg3.fillAmount = 1f;
        }
        else if (data.playerHP == 2.5)
        {
            hpImg1.fillAmount = 1f;
            hpImg2.fillAmount = 1f;
            hpImg3.fillAmount = 0.5f;
        }
        else if (data.playerHP == 2)
        {
            hpImg1.fillAmount = 1f;
            hpImg2.fillAmount = 1f;
            hpImg3.fillAmount = 0f;
        }
        else if (data.playerHP == 1.5)
        {
            hpImg1.fillAmount = 1f;
            hpImg2.fillAmount = 0.5f;
            hpImg3.fillAmount = 0f;
        }
        else if (data.playerHP == 1)
        {
            hpImg1.fillAmount = 1f;
            hpImg2.fillAmount = 0f;
            hpImg3.fillAmount = 0f;
        }
        else if (data.playerHP == 0.5)
        {
            hpImg1.fillAmount = 0.5f;
            hpImg2.fillAmount = 0f;
            hpImg3.fillAmount = 0f;
        }
        else
        {
            hpImg1.fillAmount = 0f;
            hpImg2.fillAmount = 0f;
            hpImg3.fillAmount = 0f;
        }
    }
}
