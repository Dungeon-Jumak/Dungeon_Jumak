using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public Text sizeText;
    public Image fireImage;

    public bool cook = true;

    [SerializeField]
    private Data data;



    void Start()
    {
        data = DataManager.Instance.data;
        ResetAnim();
    }

    void Update()
    {
        // 불의 크기를 시간에 따라 조절

        if (cook == true)
        {
            data.fireSize -= (1f / 1f) * Time.deltaTime;
            fireImage.transform.localScale = Vector3.one * (data.fireSize / 100f);
        }

        sizeText.text = Mathf.FloorToInt(data.fireSize) + "%";

        if (data.fireSize < 0)
        {
            data.fireSize = 0; 
            sizeText.text = "0%";
        }

        if (data.fireSize > 0) 
        {
            fireImage.gameObject.SetActive(true);
        }
    }

    public void ResetAnim()
    {
        // 시간 및 불의 크기를 초기화
        fireImage.transform.localScale = Vector3.one;
        fireImage.gameObject.SetActive(true);
    }

    public void IncreaseFireSize()
    {
        data.fireSize += 5f;
    }
}
