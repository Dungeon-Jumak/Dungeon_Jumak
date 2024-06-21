//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerHPController : MonoBehaviour
{
    [Header("�÷��̾��� ���� ü��")]
    [SerializeField] private float currentHP;

    [Header("�÷��̾��� �ִ� ü��")]
    [SerializeField] private float maxHP;

    [Header("ü�� �����̴�")]
    [SerializeField] private Slider hpSlider;

    [Header("ü�� �����̴� ��ġ")]
    [SerializeField] private Transform sliderTransform;

    private Coroutine coroutine;

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
        if(coroutine != null)
        {
            StopAllCoroutines();
            StartCoroutine(DeActiveSlider());
        }
        else if (coroutine == null)
        {
            coroutine = StartCoroutine(DeActiveSlider());
        }

        //Check HP
        if (currentHP > 0)
        {
            //Live
        }
        else
        {
            //Die
        }
    }

    //Coroutine for deactive hp slider
    IEnumerator DeActiveSlider()
    {
        yield return new WaitForSeconds(3f);

        hpSlider.gameObject.SetActive(false);
    }
}
