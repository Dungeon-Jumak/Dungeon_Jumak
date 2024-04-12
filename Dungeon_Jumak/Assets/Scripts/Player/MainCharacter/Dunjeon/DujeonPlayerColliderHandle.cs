using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DujeonPlayerColliderHandle : MonoBehaviour
{
    private Data data;

    private AudioManager audioManager;
    private BGMManager bgmManager;

    void Awake()
    {
        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
        bgmManager = FindObjectOfType<BGMManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //---��ֹ��� �ε����� ��---//
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if(data.isSound)
                audioManager.Play("obstacleSound");

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

            bgmManager.Stop();
            audioManager.AllStop();

            SceneManager.LoadScene("FightScene");//���Ͷ� �ε����� �� �Ѿ�� ��Ʈ. ���Ƿ� test�� ������
        }
        //---ȸ���ʶ� �ε����� ��---//
        else if (other.gameObject.CompareTag("Recovery"))
        {
            if (data.isSound)
                audioManager.Play("healSound");

            Debug.Log("ȸ���ʶ� �ε��� ��");
            Destroy(other.gameObject);

            if(data.playerHP != 3f)
            {
                data.playerHP += 0.5f;
            }
        }
    }
}
