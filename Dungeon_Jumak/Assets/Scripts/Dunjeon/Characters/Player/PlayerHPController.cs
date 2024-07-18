// System
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

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

    [Header("���� ���� �˾�")]
    [SerializeField] private GameObject gameOver;

    [Header("����� Ȱ��ȭ�� �÷��̾� ��������Ʈ")]
    [SerializeField] private GameObject deadSprite;    
    
    [Header("����� Ȱ��ȭ�� �÷��̾� ��������Ʈ")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("����� Ȱ��ȭ�� �÷��̾� ��������Ʈ")]
    [SerializeField] private ParticleSystem plarticleForPlayer;

    private Coroutine hitCoroutine;
    private Coroutine deactiveCoroutine;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Reposition
        hpSlider.transform.position = sliderTransform.position + new Vector3(0f, 1.3f, 0f);

        // Update Slider's Value
        hpSlider.value = currentHP / maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Monster"))
            return;

        plarticleForPlayer.Play();

        // Show Slider
        hpSlider.gameObject.SetActive(true);

        currentHP -= collision.transform.GetComponent<MonsterController>().attackPower;

        // Check Coroutine
        if (deactiveCoroutine != null)
        {
            StopCoroutine(DeActiveSlider());
            StartCoroutine(DeActiveSlider());
        }
        else if (deactiveCoroutine == null)
        {
            deactiveCoroutine = StartCoroutine(DeActiveSlider());
        }

        // Check HP
        if (currentHP > 0)
        {
            // Live : Add Animation
            GameManager.Sound.Play("[S] Player Hit", Define.Sound.Effect, false);
        }
        else
        {
            // Die
            hpSlider.gameObject.SetActive(false);
            spriteRenderer.enabled = false;

            deadSprite.SetActive(true);

            StartCoroutine(Die());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Monster"))
            return;

        // Show Slider
        hpSlider.gameObject.SetActive(true);

        if (hitCoroutine == null)
        {
            hitCoroutine = StartCoroutine(HitDelay(collision));
        }
    }

    private IEnumerator HitDelay(Collision2D _collision)
    {
        yield return new WaitForSeconds(1f);

        currentHP -= _collision.gameObject.GetComponent<MonsterController>().attackPower;

        hitCoroutine = null;
    }

    private IEnumerator DeActiveSlider()
    {
        yield return new WaitForSeconds(1.5f);

        hpSlider.gameObject.SetActive(false);
    }

    private IEnumerator Die()
    { 
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Show Game Over Panel
        gameOver.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }
}
