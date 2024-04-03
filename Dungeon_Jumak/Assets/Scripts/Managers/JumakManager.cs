using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumakManager : MonoBehaviour
{
    //---�ر� �� �ܻ� �迭---//
    [SerializeField]
    private GameObject[] Dansangs;

    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    // Update is called once per frame
    void Update()
    {
        unlockTable();
    }

    //---���� �ε�� ������ ���� ���� �ر�---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            if (Dansangs[i] != null) // �ش� ���� ������Ʈ�� �ı����� �ʾҴ��� Ȯ��
            {
                Dansangs[i].SetActive(true);
            }
        }
    }
}
