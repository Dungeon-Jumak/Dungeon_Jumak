using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject monsterPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("Shoot");
            MonsterController monster = collision.gameObject.GetComponent<MonsterController>();
            //monster.TakeDamage(1); // 몬스터의 체력을 1 감소시킵니다.
            Destroy(this.gameObject);

        }
    }
}
