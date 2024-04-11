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
            //---Àå¾Ö¹°°ú ºÎµúÇûÀ» ¶§---//
            Debug.Log("Àå¾Ö¹°°ú ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);
            data.isObstacle = false;
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            //---¸ó½ºÅÍ¶û ºÎµúÇûÀ» ¶§---//
            Debug.Log("¸ó½ºÅÍ¶û ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);
            data.isMonster = false;
        }else if (other.gameObject.CompareTag("Recovery"))
        {
            //---È¸º¹ÃÊ¶û ºÎµúÇûÀ» ¶§---//
            Debug.Log("È¸º¹ÃÊ¶û ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);
            data.isObstacle = false;
        }
    }
}
