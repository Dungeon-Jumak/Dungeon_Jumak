using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int hp = 3;

    [SerializeField]
    private GameObject resultPanel;

    private void Update()
    {
        switch (hp)
        {
            case 2:
                SetChildActive(2, false);
                break;
            case 1:
                SetChildActive(1, false);
                break;
            case 0:
                SetChildActive(0, false);
                break;
        }

    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0)
        {
            this.gameObject.SetActive(false);
            resultPanel.SetActive(true);
        }
    }

    private void SetChildActive(int index, bool isActive)
    {
        transform.parent.GetChild(3).transform.GetChild(index).gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            TakeDamage(1);
        }
    }
}