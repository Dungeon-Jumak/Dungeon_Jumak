//System
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


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

    [Header("스킬 대기 시간 (진행 중)")]
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

    private void Update()
    {
        if (hideImage.gameObject.activeSelf)
        {
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
                    if (currentDuration >= duration)
                    {
                        //init current duration
                        currentDuration = 0f;
                        Demolition();
                        hideImage.gameObject.SetActive(true);
                    }
                }
                else if (transform.childCount == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                break;
            //Fire Flooring
            case 2:
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

            //Fire Flooring
            case 3:
                break;

            default:
                break;
        }
    }

    //!Common Method : CoolTime
    private void CoolTime()
    {
        if (!canSkill && hideImage.gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > coolTime)
            {
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

            hideImage.gameObject.SetActive(true);
        }
    }

    #endregion Fire Ball

    #region Fire Shield

    public void FireShield()
    {
        if (canSkill)
        {
            canSkill = false;

            GameManager.Sound.Play("[S] Fire Shield", Define.Sound.Effect, false);
            Batch();
        }
    }

    private void Batch()
    {
        GameObject skillRound = pool.Get(prefabId+1);

        skillRound.transform.parent = transform;

        skillRound.transform.localPosition = Vector3.zero;
        skillRound.transform.localRotation = Quaternion.Euler(90, 0, 0);

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

    #endregion

    #region Fire Flooring

    //Fire Flooring : Fire Flooring Casting Method
    public void FireFlooring()
    {
        if (canSkill)
        {
            SpawnObjects();
        }
    }

    void SpawnObjects()
    {
        canSkill = false;

        //Player Position
        Vector3 basePosition = transform.position;

        //Spawn Fire Flooring object in random position around the player
        for (int i = 0; i < 2; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(basePosition.x - 5f, basePosition.x + 5f),
                Random.Range(basePosition.y - 5f, basePosition.y + 5f),
                basePosition.z
            );

            //Pooling skill water
            GameObject skillWater = pool.Get(prefabId);

            //Change position by random
            skillWater.transform.position = randomPosition;
            skillWater.transform.rotation = Quaternion.identity;

            StartCoroutine(ReturnToPoolSkillWater(skillWater, 1f, prefabId, randomPosition));
        }
    }

    private IEnumerator ReturnToPoolSkillWater(GameObject skill, float delay, int index, Vector3 randomPos)
    {
        yield return new WaitForSeconds(delay);

        //Return
        pool.ReturnToPool(skill, index);

        //Pooling
        GameObject skillFloor = pool.Get(prefabId + 1);

        //Change position by random
        skillFloor.transform.position = randomPos;
        skillFloor.transform.rotation = Quaternion.identity;

        //Coroutine
        StartCoroutine(ReturnToPoolSkillFloor(skillFloor, 3f, prefabId + 1));
    }

    private IEnumerator ReturnToPoolSkillFloor(GameObject skill, float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        //Return
        pool.ReturnToPool(skill, index);

        hideImage.gameObject.SetActive(true);
    }

    #endregion
}
