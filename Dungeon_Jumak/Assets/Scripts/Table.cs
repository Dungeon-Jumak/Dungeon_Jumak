using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool isOnFood;

    [SerializeField]
    private PlayerMovement player;

    private void Start()
    {
        isOnFood = false;
    }

    //--- ������ ���� �Ǹ� isOnFood�� True�� ��ȯ---//
    //���߿� �±� ���� �߰��ϴ°� ���� ��

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gukbap")
        {
            if (player.isPlace)
            {
                player.isPlace = false;
                Debug.Log("������ ���̺� ���� �ö󰬽��ϴ�");
                isOnFood = true;
            }
        }
    }


}
