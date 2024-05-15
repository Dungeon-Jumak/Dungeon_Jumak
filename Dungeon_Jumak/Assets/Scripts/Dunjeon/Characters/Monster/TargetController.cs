using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    Rigidbody2D rb;//몬스터 rigid

    [SerializeField]
    [Range(1f, 4f)] float speed = 3f;

    [SerializeField]
    [Range(0f, 3f)] float distance = 0.3f;

    private Transform targetPlayer;//플레이어 게임 오브젝트

    [SerializeField]
    private bool isFollow = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            targetPlayer = playerObject.transform;
        }
    }

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (targetPlayer != null)
        {
            if (Vector2.Distance(transform.position, targetPlayer.position) > distance && isFollow)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollow = false;
        }
    }
}
