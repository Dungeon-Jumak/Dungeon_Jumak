//System
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

    //Fire Shield : Fire Shield Batch Method
    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            //Pooling
            GameObject skill = pool.Get(prefabId);

            //Change Transform
            skill.transform.parent = transform;

            //Init
            skill.transform.localPosition = Vector3.zero;
            skill.transform.localRotation = Quaternion.Euler(90, 0, 0);

            StartCoroutine(ReturnToPoolFireFlooring(skill, 5f, prefabId));
        }
    }

    //Fire Shield : Fire Shield Return Method
    private IEnumerator ReturnToPoolFireFlooring(GameObject skill, float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        //Return
        pool.ReturnToPool(skill, index);
    }

    #endregion

    #region fire flooring

    //Fire Flooring : Fire Flooring Casting Method
    public void FireFlooring()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
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
    }

    #endregion
}
