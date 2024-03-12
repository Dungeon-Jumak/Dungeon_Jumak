using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Data
{
    //데이터 사용법
    //게임 진행에 있어 전반적으로 필요한 데이터는 Data.cs에 public접근 지정자로 변수로서 둘 것
    //ex) 레벨, 설정 값 등등
    //데이터에 있는 값을 다른 스크립트를 사용하기 위해서는 싱글톤으로서 사용하면 됨
    //ex) Data data = DataManager.Instance.data; => 이를 통해 Data.cs에 있는 변수값을 사용할 수 있음


    // --- 손님 이동 관련 데이터 --- //
    public Transform[] seats;
    public bool[] isAllocated; //자리 할당 여부
}
