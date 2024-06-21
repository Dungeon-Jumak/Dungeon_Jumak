//System
using System;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;


[DisallowMultipleComponent]
public class SkillCaster : MonoBehaviour
{
    [Header("스킬 이름 (확인용)")]
    public string _name;

    [Header("스킬 ID")]
    public int id;

    [Header("프리팹 ID")]
    public int prefabId;

    [Header("스킬 데미지")]
    public float damage;

    [Header("스킬 투사체 수")]
    public int count;

    [Header("스킬 관통력")]
    public int per;

    [Header("스킬 넉백력")]
    public float knockBack;

    [Header("스킬 속도")]
    public float speed;

    [Header("스킬 사용 가능 여부")]
    public bool canSkill;

    [Header("스킬 쿨타임")]
    public float coolTime;

    [Header("스킬 대기 시간")]
    public float timer;

    [Header("스킬 지속 시간")]
    public float duration;

    private float currentDuration = 0;

    [Header("스캐너")]
    [SerializeField] private Scanner scanner;

    [Header("스킬 투사체 풀링")]
    [SerializeField] private SkillPoolManager pool;

    [Header("스킬 하이드 이미지")]
    [SerializeField] private Image hideImage;

    [Header("스킬 쿨타임 텍스트")]
    [SerializeField] private Text text;

    private void Update()
    {
        if (hideImage.gameObject.activeSelf)
        {
            text.text = Mathf.FloorToInt(coolTime - timer).ToString();
            hideImage.fillAmount = timer / coolTime;
        }



        switch (id)
        {
            //Fire Ball
            case 0:
                break;

            //Fire Shield
            case 1:
                if (transform.childCount != 0)
                {
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);

                    currentDuration += Time.deltaTime;

                    if(currentDuration >= duration)
                    {
                        //init current duration
                        currentDuration = 0f;

                        Demolition();
                    }
                }
                else if (transform.childCount == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                break;

            default:
                break;
        }

        CoolTime();
    }

    #region Common Method

    //!Common Method : Init
    public void Init()
    {
        switch (id)
        {
            //Fire Ball
            case 0:
                break;

            //Fire Shield
            case 1:
                break;

            default:
                break;
        }
    }

    //!Common Method : CoolTime
    private void CoolTime()
    {
        if (!canSkill)
        {
            timer += Time.deltaTime;

            if (timer > coolTime)
            {
                //Can Skill
                canSkill = true;

                hideImage.gameObject.SetActive(false);

                timer = 0f;
            }
        }
    }

    #endregion

    #region Fire Ball

    //Fire Ball : Fire Ball Casting Method
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

            GameManager.Sound.Play("[S] Fire Ball", Define.Sound.Effect, false);

            //Hide Skill Image
            hideImage.gameObject.SetActive(true);

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
            fireball.GetComponent<Skills>().Init(damage, per, knockBack, direction);
        }
        else
        {
            Debug.Log("스킬 쿨타임이 " + (coolTime - timer) + "초 남았습니다!");
        }
    }

    #endregion Fire Ball

    #region Fire Shield

    //Fire Shield : Fire Shield Batch Method
    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            //Pooling
            Transform skill = pool.Get(prefabId).transform;

            //Change Transform
            skill.parent = transform;

            //Init
            skill.localPosition = Vector3.zero;
            skill.localRotation = Quaternion.Euler(0, 0, 0);

            //Get Rotation Vector
            Vector3 rotVec = Vector3.forward * 360 * i / count;

            //Rotation
            skill.Rotate(rotVec);

            //Translate
            skill.Translate(skill.up * 2f, Space.World);

            //Init
            skill.GetComponent<Skills>().Init(damage, -1, knockBack, Vector3.zero); // -1 is Infinity Per
        }
    }

    //Fire Shield : Fire Shield Demolition Method
    private void Demolition()
    {
        Transform[] childs = GetComponentsInChildren<Transform>();

        //Un Pool
        foreach (var child in childs)
        {
            if (child.gameObject == transform.gameObject) continue;
            else
            {
                child.gameObject.SetActive(false);
                child.parent = pool.transform;
            }
        }
    }

    //Fire Shield : Fire Shield Casteing Method
    public void FireShield()
    {
        if (canSkill)
        {
            canSkill = false;

            GameManager.Sound.Play("[S] Fire Shield", Define.Sound.Effect, false);

            hideImage.gameObject.SetActive(true);

            Batch();
        }
    }

    #endregion
}
