using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] Seats; //손님들이 앉을 자리
    [SerializeField]
    private bool[] checkSeat; //자리 할당 여부

    //[SerializeField]
    //private GameObject[] customerPrefab; //손님 프리팹

    [SerializeField]
    private GameObject cutomerPrefab;

    [SerializeField]
    private float cutomerSpeed;




}
