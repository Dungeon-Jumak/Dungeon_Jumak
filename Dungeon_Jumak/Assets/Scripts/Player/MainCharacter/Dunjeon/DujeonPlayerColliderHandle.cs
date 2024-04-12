using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DujeonPlayerColliderHandle : MonoBehaviour
{
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    private void OnTriggerEnter(Collider other)
    {
        //---��ֹ��� �ε����� ��---//
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("��ֹ��� �ε��� ��");
            Destroy(other.gameObject);

            if (data.playerHP != 0f)
            {
                data.playerHP -= 0.5f;
            }
        }
        //---���Ͷ� �ε����� ��---//
        else if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("���Ͷ� �ε��� ��");
            Destroy(other.gameObject);

            data.isMonster = false;
            data.monsterSpawn[0] = true;
            SceneManager.LoadScene("FightScene");//���Ͷ� �ε����� �� �Ѿ�� ��Ʈ. ���Ƿ� test�� ������
        }
        //---ȸ���ʶ� �ε����� ��---//
        else if (other.gameObject.CompareTag("Recovery"))
        {
            Debug.Log("ȸ���ʶ� �ε��� ��");
            Destroy(other.gameObject);

            if(data.playerHP != 3f)
            {
                data.playerHP += 0.5f;
            }
        }
    }
}
