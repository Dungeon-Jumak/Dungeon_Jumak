//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class DropItem : MonoBehaviour
{
    [Header("드랍 아이템 이름 (확인용")]
    [SerializeField] private string _name;

    [Header("드랍 아이템 ID = Data ID")]
    [SerializeField] private int id;

    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    //Pick Up Item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check Player
        if (!collision.CompareTag("Player"))
            return;

        //Increase Ingredient
        data.ingredient[id]++;

        //De Pool
        gameObject.SetActive(false);
    }




}
