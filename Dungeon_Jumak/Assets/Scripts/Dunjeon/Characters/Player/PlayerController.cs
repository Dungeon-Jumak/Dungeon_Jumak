using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject effectImg;

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
                resultPanel.SetActive(true);
                break;
        }

    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
    }

    private void SetChildActive(int index, bool isActive)
    {
        transform.parent.GetChild(3).transform.GetChild(index).gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            effectImg.SetActive(true);
            TakeDamage(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            effectImg.SetActive(false);
        }
    }
}