using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DujeonPlayerColliderHandle : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //---��ֹ��� �ε����� ��---//
            Debug.Log("��ֹ��� �ε��� ��");
            Destroy(other.gameObject);
            data.isObstacle = false;
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            //---���Ͷ� �ε����� ��---//
            Debug.Log("���Ͷ� �ε��� ��");
            Destroy(other.gameObject);
            data.isMonster = false;
        }else if (other.gameObject.CompareTag("Recovery"))
        {
            //---ȸ���ʶ� �ε����� ��---//
            Debug.Log("ȸ���ʶ� �ε��� ��");
            Destroy(other.gameObject);
            data.isObstacle = false;
        }
    }
}
