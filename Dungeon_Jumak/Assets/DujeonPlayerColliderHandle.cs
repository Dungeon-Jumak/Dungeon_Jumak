using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DujeonPlayerColliderHandle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //---Àå¾Ö¹°°ú ºÎµúÇûÀ» ¶§---//
            Debug.Log("Àå¾Ö¹°°ú ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            //---¸ó½ºÅÍ¶û ºÎµúÇûÀ» ¶§---//
            Debug.Log("¸ó½ºÅÍ¶û ºÎµúÈû ¤Ð");
            Destroy(other.gameObject);
        }/*else if (other.gameObject.CompareTag("RecoveryItem"))
        {
            //È¸º¹ÃÊ¶û ºÎµúÇûÀ» ¶§
        }*/
    }
}
