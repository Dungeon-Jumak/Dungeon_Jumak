using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public Text sizeText; 
    public float fireSize = 100f; // 불의 초기 크기
    public Image fireImage;

    public bool cook = true;

    void Start()
    {
        ResetAnim();
    }

    void Update()
    {
        // 불의 크기를 시간에 따라 조절

        if (cook == true)
        {
            fireSize -= (1f / 15f) * Time.deltaTime;
            fireImage.transform.localScale = Vector3.one * (fireSize / 100f);
        }

        if (fireSize <= 0f)
        {
            fireImage.gameObject.SetActive(false);
        }

        sizeText.text = Mathf.FloorToInt(fireSize) + "%";

        if (fireSize < 0)
        {
            fireSize = 0; 
            sizeText.text = "0%";
        }

        if (fireSize > 0) 
        {
            fireImage.gameObject.SetActive(true);
        }
    }

    public void ResetAnim()
    {
        // 시간 및 불의 크기를 초기화
        fireSize = 100f;
        fireImage.transform.localScale = Vector3.one;
        fireImage.gameObject.SetActive(true);
    }

    public void IncreaseFireSize()
    {
        fireSize += 1f;
    }
}
