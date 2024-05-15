using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private int hp = 3;

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
        }
    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0)
        {
            Destroy(transform.parent.gameObject);
            DataManager.Instance.data.ingredient[0]++;
            Debug.Log(DataManager.Instance.data.ingredient[0]);
        }
    }

    private void SetChildActive(int index, bool isActive)
    {
        if (transform.childCount > index)
        {
            transform.GetChild(index).gameObject.SetActive(isActive);
        }
    }
}
