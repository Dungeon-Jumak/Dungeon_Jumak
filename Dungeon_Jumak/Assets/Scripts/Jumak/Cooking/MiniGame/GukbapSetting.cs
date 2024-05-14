using System.Collections.Generic;
using UnityEngine; 

public class GukbapSetting : MonoBehaviour 
{
    public GameObject gukbapPrefab; // ������ �̹��� �������� ��� public ����

    public List<bool> gukbapList; // ���� ����Ʈ (true�� �ش� ��ġ�� ������ �ִ� ����)�� ��� public ����

    public CookGukbap cookGukbap; // �ٸ� ��ũ��Ʈ�� �ִ� CookGukbap ��ũ��Ʈ�� �ν��Ͻ��� ��� public ����

    [SerializeField] private int previousCount; // ���� �����ӿ����� ���� ī��Ʈ�� �����ϱ� ���� private ����
    [SerializeField] Transform[] idxs;

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
                    Debug.Log("���� �ε��� : " + index);
                    GameObject newGukbap = Instantiate(gukbapPrefab, idxs[index].position, Quaternion.identity); // ���ο� ������ �����ϰ� ��ġ
                    newGukbap.transform.parent = transform; // ���ο� ������ �� ��ũ��Ʈ�� �ڽ����� ����
                    gukbapList[index] = true; // ���� ����Ʈ���� �ش� ��ġ�� ���� true�� �����Ͽ� ������ ������ ǥ��
                }
            }
            previousCount = cookGukbap.gukbapCount; // ���� �������� ���� ī��Ʈ�� ������Ʈ
        }

        CheckGukbapPresence(); // ������ �ִ��� Ȯ���ϰ� ������ �ش� ��ġ�� false�� ����
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

    void CheckGukbapPresence()
    {
        for (int i = 0; i < gukbapList.Count; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(idxs[i].transform.position);
            bool gukbapPresent = false;
            

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Gukbab"))
                {
                    gukbapPresent = true;

                    break;
                }
            }

            gukbapList[i] = gukbapPresent;
        }
    }
}
