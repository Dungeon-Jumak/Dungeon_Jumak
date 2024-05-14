using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmFood : MonoBehaviour
{
    [SerializeField]
    private SetFood setFood;

    Data data;
    
    private void Start()
    {
        data = DataManager.Instance.data;
    }
    //�̹��� �ؽ�Ʈ �߰��� �Լ� �߰�
    public void ConfirmGukbab()
    {
        for (int i = 0; i < data.ingredient.Length; i++)
        {
            data.ingredient[i] -= setFood.subGukbabIngredient[i];
        }
    }
}
