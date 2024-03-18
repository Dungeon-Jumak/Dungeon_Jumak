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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Debug.Log("������ ���̺� ���� �ö󰬽��ϴ�");
            isOnFood = true;
            //player.isPlace = false;
        }

    }
}
