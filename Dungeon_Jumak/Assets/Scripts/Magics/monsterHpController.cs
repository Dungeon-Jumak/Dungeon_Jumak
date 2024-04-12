using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monsterHpController : MonoBehaviour
{
    public GameObject hpImagePrefab;
    public Transform startPosition;

    private List<Image> hpImages = new List<Image>();
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;

        InitializeHPImages();
    }

    void Update()
    {
        UpdateHPImages();
    }

    //===HP 이미지들을 초기화하는 함수===//
    private void InitializeHPImages()
    {
        //===기존에 생성된 HP 이미지가 있다면 모두 삭제===//
        foreach (Image image in hpImages)
        {
            Destroy(image.gameObject);
        }

        hpImages.Clear();

        //===data.monsterHP 값에 따라 HP 이미지들을 생성하고 리스트에 추가===//
        int imageCount = Mathf.CeilToInt(data.monsterHP);
        for (int i = 0; i < imageCount; i++)
        {
            Vector3 position = startPosition.position + Vector3.right * i * hpImagePrefab.GetComponent<RectTransform>().rect.width;
            GameObject hpImageObject = Instantiate(hpImagePrefab, position, Quaternion.identity, transform);
            Image hpImage = hpImageObject.GetComponent<Image>();
            hpImages.Add(hpImage);
        }
    }

    private void UpdateHPImages()
    {
        //===각 HP 이미지의 fillAmount 값을 설정===//
        for (int i = 0; i < hpImages.Count; i++)
        {
            float fillAmount = CalculateFillAmount(i);
            hpImages[i].fillAmount = fillAmount;
        }
    }

    //===각 HP 이미지의 fillAmount 값을 계산하는 함수===//
    private float CalculateFillAmount(int index)
    {
        float remainingHP = data.monsterHP - index;
        float fillAmount = Mathf.Clamp01(remainingHP);
        return fillAmount;
    }
}
