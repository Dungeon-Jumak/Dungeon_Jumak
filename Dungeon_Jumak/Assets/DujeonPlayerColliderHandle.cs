using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DujeonPlayerColliderHandle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //---��ֹ��� �ε����� ��---//
            Debug.Log("��ֹ��� �ε��� ��");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            //---���Ͷ� �ε����� ��---//
            Debug.Log("���Ͷ� �ε��� ��");
            Destroy(other.gameObject);
        }/*else if (other.gameObject.CompareTag("RecoveryItem"))
        {
            //ȸ���ʶ� �ε����� ��
        }*/
    }
}
