//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerHPController : MonoBehaviour
{
    [Header("플레이어의 현재 체력")]
    [SerializeField] private float currentHP;

    [Header("플레이어의 최대 체력")]
    [SerializeField] private float maxHP;

    [Header("체력 슬라이더")]
    [SerializeField] private Slider hpSlider;

    [Header("체력 슬라이더 위치")]
    [SerializeField] private Transform sliderTransform;

    [Header("게임 오버 팝업")]
    [SerializeField] private GameObject gameOver;

    private Coroutine hitCoroutine;
    private Coroutine deactiveCoroutine;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        //Reposition
        hpSlider.transform.position = sliderTransform.position;

        //Update Slider's Value
        hpSlider.value = currentHP / maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Monster"))
            return;

        //Show Slider
        hpSlider.gameObject.SetActive(true);

        currentHP -= collision.transform.GetComponent<MonsterController>().attackPower;

        //Check Coroutine
        if (deactiveCoroutine != null)
        {
            StopCoroutine(DeActiveSlider());
            StartCoroutine(DeActiveSlider());
        }
        else if (deactiveCoroutine == null)
        {
            deactiveCoroutine = StartCoroutine(DeActiveSlider());
        }

        //Check HP
        if (currentHP > 0)
        {
            //Live : Add Animation

            GameManager.Sound.Play("[S] Player Hit", Define.Sound.Effect, false);
        }
        else
        {
            //Die
            Time.timeScale = 0f;

            gameOver.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Monster"))
            return;

        //Show Slider
        hpSlider.gameObject.SetActive(true);

        if (hitCoroutine == null)
        {
            hitCoroutine = StartCoroutine(HitDelay(collision));
        }

    }

    IEnumerator HitDelay(Collision2D _collision)
    {
        yield return new WaitForSeconds(1f);

        currentHP -= _collision.transform.GetComponent<MonsterController>().attackPower;

        hitCoroutine = null;
    }

    //Coroutine for deactive hp slider
    IEnumerator DeActiveSlider()
    {
        yield return new WaitForSeconds(3f);

        hpSlider.gameObject.SetActive(false);
    }
}
