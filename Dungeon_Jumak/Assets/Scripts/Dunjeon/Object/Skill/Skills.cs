//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Skills : MonoBehaviour
{
    [Header("스킬 데미지")]
    public float damage;

    [Header("스킬 관통력")]
    public int per;

    [SerializeField] private Rigidbody2D rigid;

    public void Init(float damage, int per, Vector3 direction)
    {
        //initialize
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            rigid.velocity = direction * 10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster") || per == -1)
            return;

        //decrease per
        per--;

        if (per == -1)
        {
            //init velocity
            rigid.velocity = Vector2.zero;

            //setactive false
            gameObject.SetActive(false);
        }

    }
}
