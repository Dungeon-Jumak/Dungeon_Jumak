using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool isOnFood;

    [SerializeField]
    private PlayerMovement player;

    private void Start()
    {
        isOnFood = false;
    }


    //--- 음식이 감지 되면 isOnFood를 True로 변환---//
    //나중에 태그 따로 추가하는게 좋을 듯
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Debug.Log("음식이 테이블 위에 올라갔습니다");
            isOnFood = true;
            //player.isPlace = false;
        }

    }
}
