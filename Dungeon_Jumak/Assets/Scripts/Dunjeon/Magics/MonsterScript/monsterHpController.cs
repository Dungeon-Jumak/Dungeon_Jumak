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

    //===HP �̹������� �ʱ�ȭ�ϴ� �Լ�===//
    private void InitializeHPImages()
    {
        //===������ ������ HP �̹����� �ִٸ� ��� ����===//
        foreach (Image image in hpImages)
        {
            Destroy(image.gameObject);
        }

        hpImages.Clear();

        //===data.monsterHP ���� ���� HP �̹������� �����ϰ� ����Ʈ�� �߰�===//
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
        //===�� HP �̹����� fillAmount ���� ����===//
        for (int i = 0; i < hpImages.Count; i++)
        {
            float fillAmount = CalculateFillAmount(i);
            hpImages[i].fillAmount = fillAmount;
        }
    }

    //===�� HP �̹����� fillAmount ���� ����ϴ� �Լ�===//
    private float CalculateFillAmount(int index)
    {
        float remainingHP = data.monsterHP - index;
        float fillAmount = Mathf.Clamp01(remainingHP);
        return fillAmount;
    }
}
