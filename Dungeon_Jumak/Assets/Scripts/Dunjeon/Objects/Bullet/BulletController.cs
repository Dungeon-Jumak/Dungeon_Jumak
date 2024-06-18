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
            //monster.TakeDamage(1); // ������ ü���� 1 ���ҽ�ŵ�ϴ�.
            Destroy(this.gameObject);

        }
    }
}
