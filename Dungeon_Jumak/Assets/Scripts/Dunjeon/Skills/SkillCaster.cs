//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[System.Serializable]
public struct Skill
{
    [Header("스킬 이름 (확인용)")]
    public string name;

    [Header("스킬 ID")]
    public int id;

    [Header("프리팹 ID")]
    public int prefabId;

    [Header("스킬 데미지")]
    public float damage;

    [Header("스킬 관통력")]
    public int count;

    [Header("스킬 사용 가능 여부")]
    public bool canSkill;

    [Header("스킬 쿨타임")]
    public float coolTime;

    [Header("스킬 대기 시간")]
    public float timer;
}

[DisallowMultipleComponent]
public class SkillCaster : MonoBehaviour
{
    [Header("스킬 구조체 배열")]
    public Skill[] skills;

    [Header("스캐너")]
    [SerializeField] private Scanner scanner;

    [Header("풀링")]
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
            Debug.Log("주변에 적이 없다!");
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
            Debug.Log("스킬 쿨타임이 " + (skills[inedx].coolTime - skills[inedx].timer) + "초 남았습니다!");
        }
    }
}
