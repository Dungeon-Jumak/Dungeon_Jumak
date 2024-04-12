using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monsterHpController : MonoBehaviour
{
    public GameObject hpImagePrefab; // ���������� ����� HP �̹���
    public Transform startPosition; // �̹��� ������ ������ ��ġ

    private List<Image> hpImages = new List<Image>(); // HP �̹������� ���� ����Ʈ
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;

        // �ʱ�ȭ�� �� HP �̹������� �����ϰ� ����Ʈ�� �߰�
        InitializeHPImages();
    }

    void Update()
    {
        UpdateHPImages();
    }

    // HP �̹������� �ʱ�ȭ�ϴ� �Լ�
    private void InitializeHPImages()
    {
        // ������ ������ HP �̹����� �ִٸ� ��� ����
        foreach (Image image in hpImages)
        {
            Destroy(image.gameObject);
        }

        hpImages.Clear(); // ����Ʈ �ʱ�ȭ

        // data.monsterHP ���� ���� HP �̹������� �����ϰ� ����Ʈ�� �߰�
        int imageCount = Mathf.CeilToInt(data.monsterHP);
        for (int i = 0; i < imageCount; i++)
        {
            Vector3 position = startPosition.position + Vector3.right * i * hpImagePrefab.GetComponent<RectTransform>().rect.width; // �̹����� ��ġ ����
            GameObject hpImageObject = Instantiate(hpImagePrefab, position, Quaternion.identity, transform);
            Image hpImage = hpImageObject.GetComponent<Image>();
            hpImages.Add(hpImage);
        }
    }

    // HP �̹������� ������Ʈ�ϴ� �Լ�
    private void UpdateHPImages()
    {
        // �� HP �̹����� fillAmount ���� ����
        for (int i = 0; i < hpImages.Count; i++)
        {
            float fillAmount = CalculateFillAmount(i);
            hpImages[i].fillAmount = fillAmount;
        }
    }

    // �� HP �̹����� fillAmount ���� ����ϴ� �Լ�
    private float CalculateFillAmount(int index)
    {
        float remainingHP = data.monsterHP - index;
        float fillAmount = Mathf.Clamp01(remainingHP); // fillAmount�� 0~1 ������ ����
        return fillAmount;
    }
}
