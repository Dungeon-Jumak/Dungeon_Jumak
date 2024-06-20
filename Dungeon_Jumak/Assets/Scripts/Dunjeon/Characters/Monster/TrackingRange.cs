//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class TrackingRange : MonoBehaviour
{
    //monster controller script
    private MonsterController monsterController;

    [Header("추적 범위 콜라이더")]
    [SerializeField] private CircleCollider2D circle;

    [Header("추적 범위 반지름")]
    [SerializeField] private float range;

    [Header("추적 시 보여줄 신호")]
    [SerializeField] private GameObject trackingSign;

    private void Start()
    {
        //get component
        monsterController = GetComponent<MonsterController>();

        circle.radius = range;
    }

    //ontrigger enter 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player
        if (collision.CompareTag("Player"))
        {
            monsterController.isTrack = true;
            StartCoroutine(DisplayTrackingSign());

            circle.enabled = false;
        }
    }

    //Display Tracking Sign Coroutine
    IEnumerator DisplayTrackingSign()
    {
        //Display Sign
        for (int i = 0; i < 3; i++)
        {
            trackingSign.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            trackingSign.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
