//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[System.Serializable]
public struct Skill
{
    [Header("��ų �̸� (Ȯ�ο�)")]
    public string name;

    [Header("��ų ID")]
    public int id;

    [Header("������ ID")]
    public int prefabId;

    [Header("��ų ������")]
    public float damage;

    [Header("��ų �����")]
    public int count;

    [Header("��ų ��� ���� ����")]
    public bool canSkill;

    [Header("��ų ��Ÿ��")]
    public float coolTime;

    [Header("��ų ��� �ð�")]
    public float timer;
}

[DisallowMultipleComponent]
public class SkillCaster : MonoBehaviour
{
    [Header("��ų ����ü �迭")]
    public Skill[] skills;

    [Header("��ĳ��")]
    [SerializeField] private Scanner scanner;

    [Header("Ǯ��")]
    [SerializeField] private MonsterPoolManager pool;

    private void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (!skills[i].canSkill)
            {
                skills[i].timer += Time.deltaTime;

                if (skills[i].timer > skills[i].coolTime)
                {
                    skills[i].canSkill = true;

                    skills[i].timer = 0f;
                }
            }
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

        int inedx = skills[0].id;

        if (skills[inedx].canSkill)
        {
            //Skill Cool Time
            skills[inedx].canSkill = false;

            //Get Tartget Position
            Vector3 targetPos = scanner.nearestTarget.position;

            //Get Direction
            Vector3 direction = targetPos - transform.position;

            //Nomalized
            direction = direction.normalized;

            //pooling fire ball
            Transform fireball = pool.Get(skills[inedx].prefabId).transform;

            //reposition
            fireball.position = transform.position;

            //rotation
            fireball.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            //Init
            fireball.GetComponent<Skills>().Init(skills[inedx].damage, skills[inedx].count, direction);
        }
        else
        {
            Debug.Log("��ų ��Ÿ���� " + (skills[inedx].coolTime - skills[inedx].timer) + "�� ���ҽ��ϴ�!");
        }
    }
}
