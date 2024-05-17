using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    public GameObject bulletPrefab;//ÃÑ¾Ë ÇÁ¸®ÆÕ
    public Transform bulletSpawnPoint;//ÃÑ¾Ë ½ºÆù À§Ä¡

    public GameObject a;
    public GameObject b;
    public GameObject c;

    private float bulletSpeed = 4f;
    private Vector3 monsterPos;
    private Vector3 dir;
    private bool isAttacking = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            monsterPos = collision.transform.position;

            if (!isAttacking)
            {
                //StartCoroutine(Attack());
            }

            a.SetActive(true);
            b.SetActive(true);
            c.SetActive(true);

            Debug.Log(DataManager.Instance.data.isSkillSuc);
            if (DataManager.Instance.data.isSkillSuc)
            {
                MonsterController monsterController = collision.gameObject.GetComponent<MonsterController>();
                Debug.Log("Attack the monster with skill");
                monsterController.TakeDamage(1);
                DataManager.Instance.data.isSkillSuc = false;

                a.SetActive(false);
                b.SetActive(false);
                c.SetActive(false);
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);  // Or however long you want the delay to be

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        dir = (monsterPos - bulletSpawnPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(10f);  // Delay after shooting, before another shot can be fired
        isAttacking = false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            a.SetActive(false);
            b.SetActive(false);
            c.SetActive(false);
        }
    }
}
