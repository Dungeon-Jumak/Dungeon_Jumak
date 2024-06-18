//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class SkillCaster : MonoBehaviour
{
    [Header("스킬 사용 가능 여부")]
    public bool canSkill;

    [Header("스킬 ID")]
    [SerializeField] private int id;

    [Header("프리팹 ID")]
    [SerializeField] private int prefabId;

    [Header("스킬 데미지")]
    [SerializeField] private float damage;

    [Header("스킬 관통력")]
    [SerializeField] private int count;

    [Header("스킬 쿨타임")]
    [SerializeField] private float coolTime;

    [Header("스캐너")]
    [SerializeField] private Scanner scanner;

    [Header("풀링")]
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
            Debug.Log("주변에 적이 없다!");
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
            Debug.Log("스킬 쿨타임이 " + (coolTime - timer) + "초 남았습니다!");
        }
    }
}
