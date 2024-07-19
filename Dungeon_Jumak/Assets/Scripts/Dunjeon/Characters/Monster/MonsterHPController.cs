//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MonsterHPController : MonoBehaviour
{
    [Header("슬라이더 프리팹")]
    [SerializeField] private GameObject sliderPrefab;

    private GameObject slider;

    private MonsterPoolManager pool;

    private MonsterController monster;

    private Coroutine coroutine;

    [Header("체력바 위치")]
    [SerializeField] private Transform sliderTransform;

    private void OnEnable()
    {
        //Get Pool
        pool = FindObjectOfType<MonsterPoolManager>();
    }

    private void Start()
    {
        //Get Component
        monster = GetComponent<MonsterController>();
    }

    private void Update()
    {
        //Reposition and Update Value
        if (slider != null && monster.isLive)
        {
            slider.transform.position = sliderTransform.position;

            slider.GetComponent<Slider>().value = monster.health / monster.maxHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Attack_Shield") || collision.CompareTag("Attack_Ball") || collision.CompareTag("Attack_Floor")))
            return;

        if (slider == null)
        {
            slider = Instantiate(sliderPrefab);
            slider.transform.SetParent(GameObject.Find("[Slider] Monster HP Bars").transform);
            slider.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            slider.gameObject.SetActive(true);
        }

        if (monster.health > 0)
        {
            if (coroutine != null)
            {
                //Stop Coroutine
                StopAllCoroutines();
                //Start Coroutine
                StartCoroutine(DeActiveSlider());
            }
            else
            {
                coroutine = StartCoroutine(DeActiveSlider());
            }
        }
        else
        {
            Destroy(slider);
        }
    }

    //DeActive Slider
    IEnumerator DeActiveSlider()
    {
        yield return new WaitForSeconds(1.5f);

        //Init null
        slider.gameObject.SetActive(false);
    }

}
