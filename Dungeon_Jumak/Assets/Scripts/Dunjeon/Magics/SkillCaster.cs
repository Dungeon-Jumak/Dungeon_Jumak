//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class SkillCaster : MonoBehaviour
{
    [Header("��ų ��� ���� ����")]
    public bool canSkill;

    [Header("��ų ID")]
    [SerializeField] private int id;

    [Header("������ ID")]
    [SerializeField] private int prefabId;

    [Header("��ų ������")]
    [SerializeField] private float damage;

    [Header("��ų �����")]
    [SerializeField] private int count;

    [Header("��ų ��Ÿ��")]
    [SerializeField] private float coolTime;

    [Header("��ĳ��")]
    [SerializeField] private Scanner scanner;

    [Header("Ǯ��")]
    [SerializeField] private MonsterPoolManager pool;

    //Timer
    private float timer;

    private void Update()
    {
        //Check Skill
        switch (id)
        {
            //Fire Ball
            case 0:
                if (!canSkill)
                {
                    //Increase Timer
                    timer += Time.deltaTime;

                    if (timer > coolTime)
                    {
                        canSkill = true;

                        timer = 0f;
                    }
                }
                break;
        }

        if(Input.GetKey(KeyCode.Space)) FireBall();
    }

    //Fire Ball Casting Method
    public void FireBall()
    {
        //if can't search target
        if (!scanner.nearestTarget)
        {
            Debug.Log("�ֺ��� ���� ����!");
            return;
        }


        if (canSkill)
        {
            //Skill Cool Time
            canSkill = false;

            //Get Tartget Position
            Vector3 targetPos = scanner.nearestTarget.position;

            //Get Direction
            Vector3 direction = targetPos - transform.position;

            //Nomalized
            direction = direction.normalized;

            //pooling fire ball
            Transform fireball = pool.Get(prefabId).transform;

            //reposition
            fireball.position = transform.position;

            //rotation
            fireball.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            //Init
            fireball.GetComponent<Skills>().Init(damage, count, direction);
        }
        else
        {
            Debug.Log("��ų ��Ÿ���� " + (coolTime - timer) + "�� ���ҽ��ϴ�!");
        }
    }
}
