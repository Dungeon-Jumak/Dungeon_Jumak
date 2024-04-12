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
        //---Àå¾Ö¹°°ú ºÎµúÇûÀ» ¶§---//
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Àå¾Ö¹°°ú ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);

            if (data.playerHP != 0f)
            {
                data.playerHP -= 0.5f;
            }
        }
        //---¸ó½ºÅÍ¶û ºÎµúÇûÀ» ¶§---//
        else if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("¸ó½ºÅÍ¶û ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);

            data.isMonster = false;
            data.monsterSpawn[0] = true;
            SceneManager.LoadScene("FightScene");//¸ó½ºÅÍ¶û ºÎµúÈ÷¸é ¾À ³Ñ¾î°¡´Â ÆÄÆ®. ÀÓÀÇ·Î test¾À ¸¸µé¾îµÒ
        }
        //---È¸º¹ÃÊ¶û ºÎµúÇûÀ» ¶§---//
        else if (other.gameObject.CompareTag("Recovery"))
        {
            Debug.Log("È¸º¹ÃÊ¶û ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);

            if(data.playerHP != 3f)
            {
                data.playerHP += 0.5f;
            }
        }
    }
}
