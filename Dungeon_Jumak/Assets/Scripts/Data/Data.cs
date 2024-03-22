using System;

[Serializable]
public class Data
{
    //데이터 사용법
    //게임 진행에 있어 전반적으로 필요한 데이터는 Data.cs에 public접근 지정자로 변수로서 둘 것
    //ex) 레벨, 설정 값 등등
    //데이터에 있는 값을 다른 스크립트를 사용하기 위해서는 싱글톤으로서 사용하면 됨
    //ex) Data data = DataManager.Instance.data; => 이를 통해 Data.cs에 있는 변수값을 사용할 수 있음

    //---CustomerSystem---//
    public int maxSeatSize = 2;
    public int curSeatSize = 0;
    public bool[] isAllocated = new bool[12];
    public bool[] isCustomer = new bool[12]; // 고객 테이블에 도착했는지 체크하기 위한 변수

    //---자리 해금 레벨---//
    public int curUnlockLevel = 1;
    public int maxUnlockLevel = 6;

    //---메뉴 해금 레벨---//
    public int curMenuUnlockLevel = 1;
    public int maxMenuUnlockLevel = 3;

    //---테이블 배열---//
    public bool[] onTables = new bool[12]; //테이블 위에 음식을 체크하기 위한 변수
    public bool[] isFinEat = new bool[12]; //다 먹었음을 알리는 변수

    //---메뉴 관련---//
    public int[] menuNums = new int[12];

    //---사운드 관련---//
    public bool isPlayBGM = false;

    //---불 미니게임 관련---//
    public float fireSize = 100f;


}
