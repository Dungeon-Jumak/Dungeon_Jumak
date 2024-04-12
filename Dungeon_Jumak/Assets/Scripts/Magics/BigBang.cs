using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BigBang : MonoBehaviour
{
    public MonsterBuChu monsterBuChu;
    public Image disable;

    private float coolTime = 4.0f;
    private float coolTime_max = 4.0f;
    private bool CoolCheck = false;
    private Data data;

    private AudioManager audioManager;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (coolTime <= 0.0f)
        {
            CoolCheck = false;
        }
    }

    IEnumerator BigBangCoolTimeFunc()
    {
        while (coolTime > 0.0f)
        {
            coolTime -= 0.01f;

            disable.fillAmount = coolTime / coolTime_max;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void BigBangCoolStart()
    {

        if (CoolCheck == false)
        {
            coolTime = coolTime_max;
            disable.fillAmount = 1.0f;

            StartCoroutine(BigBangCoolTimeFunc());

            StartCoroutine(BigBangMagic());
            CoolCheck = true;
        }
    }

    IEnumerator BigBangMagic()
    {
        if (data.isSound)
        {
            audioManager.Play("magicSketch");
            yield return new WaitForSeconds(1f);
            audioManager.Stop("magicSketch");

            audioManager.Play("bigBang");
        }
        data.monsterHP -= 3f;
        yield return null;
    }
}
