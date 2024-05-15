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
            StartCoroutine(BeforeAttack());

            /*a.SetActive(true);
            b.SetActive(true);
            c.SetActive(true);

            if (DataManager.Instance.data.isSkillSuc == true)
            {
                MonsterController monsterController = collision.gameObject.GetComponent<MonsterController>();
                Debug.Log("Attack the monster with skill");
                monsterController.TakeDamage(1);

                a.SetActive(false);
                b.SetActive(false);
                c.SetActive(false);
                DataManager.Instance.data.isSkillSuc = false;
            }*/
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            a.SetActive(false);
            b.SetActive(false);
            c.SetActive(false);
        }
    }*/


    private IEnumerator BeforeAttack()
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            yield return new WaitForSeconds(1f);
            StartCoroutine("Attack");
        }
    }

    private IEnumerator Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPoint.transform.position;
        dir = monsterPos - bulletSpawnPoint.transform.position;
        bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}
