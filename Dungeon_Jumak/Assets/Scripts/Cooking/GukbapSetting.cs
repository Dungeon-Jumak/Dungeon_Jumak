using System.Collections.Generic;
using UnityEngine; 

public class GukbapSetting : MonoBehaviour 
{
    public GameObject gukbapPrefab; // ������ �̹��� �������� ��� public ����

    public List<bool> gukbapList; // ���� ����Ʈ (true�� �ش� ��ġ�� ������ �ִ� ����)�� ��� public ����

    public Transform tableTransform; // ���̺� ������Ʈ�� Transform�� ��� public ����

    public CookGukbap cookGukbap; // �ٸ� ��ũ��Ʈ�� �ִ� CookGukbap ��ũ��Ʈ�� �ν��Ͻ��� ��� public ����

    [SerializeField] private int previousCount; // ���� �����ӿ����� ���� ī��Ʈ�� �����ϱ� ���� private ����

    void Start() // ���� ������Ʈ�� Ȱ��ȭ�� �� ȣ��Ǵ� �Լ�
    {
        previousCount = cookGukbap.gukbapCount; // ���� �������� ���� ī��Ʈ�� �ʱ�ȭ
        InitializeGukbapList(); // ���� ����Ʈ �ʱ�ȭ
    }

    void Update() // �� �����Ӹ��� ȣ��Ǵ� �Լ�
    {
        // ���� �����ӿ����� ���� ī��Ʈ�� ���� ī��Ʈ�� �ٸ� ������ �̹��� ������ ����
        if (cookGukbap.gukbapCount != previousCount)
        {
            if (cookGukbap.gukbapCount > previousCount)
            {
                // ���ο� ������ �߰��� ��쿡�� ����
                int index = GetNextAvailableIndex(); // �������� �߰��� �� �ִ� ������ �ε����� ������
                if (index != -1) // �߰��� �� �ִ� ��ġ�� �ִ��� Ȯ��
                {
                    float sectionWidth = tableTransform.localScale.x / gukbapList.Count; // ���̺��� �ʺ� ���� ����Ʈ�� ������ ������ �� ������ �ʺ� ���
                    float xPos = tableTransform.position.x - (tableTransform.localScale.x / 2) + sectionWidth * (index + 0.5f); // ������ ��ġ�� x ��ġ ���
                    GameObject newGukbap = Instantiate(gukbapPrefab, new Vector3(xPos, tableTransform.position.y, 0.0f), Quaternion.identity); // ���ο� ������ �����ϰ� ��ġ
                    newGukbap.transform.parent = transform; // ���ο� ������ �� ��ũ��Ʈ�� �ڽ����� ����
                    gukbapList[index] = true; // ���� ����Ʈ���� �ش� ��ġ�� ���� true�� �����Ͽ� ������ ������ ǥ��
                }
            }
            previousCount = cookGukbap.gukbapCount; // ���� �������� ���� ī��Ʈ�� ������Ʈ
        }
    }

    // ���� ����Ʈ �ʱ�ȭ
    void InitializeGukbapList()
    {
        for (int i = 0; i < 5; i++)
        {
            gukbapList.Add(false); // �ʱ⿡�� ��� ��ġ�� ������ ������ ǥ��
        }
    }

    // �������� �߰��� �� �ִ� ������ �ε��� ��ȯ
    int GetNextAvailableIndex()
    {
        for (int i = 0; i < gukbapList.Count; i++)
        {
            if (!gukbapList[i]) // �ش� ��ġ�� ������ ������ Ȯ��
            {
                return i; // ������ ���� ��ġ�� �ε��� ��ȯ
            }
        }
        return -1; // �߰��� �� �ִ� ��ġ�� ����
    }
}
