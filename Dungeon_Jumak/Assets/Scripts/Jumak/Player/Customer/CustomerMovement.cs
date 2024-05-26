// System
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;          // 시스템 런타임 컴파일러 서비스 사용

// Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CustomerMovement : MonoBehaviour
{
    //---자리 관련 변수---//
    [Header("배정받은 자리 인덱스(Don't touch!)")]
    public int seatIndex;

    [SerializeField, Header("손님의 이동속도")]
    private float speed = 3f;

    //---UI 관련 변수 (Speech_Box)---//
    [SerializeField, Header("사람이 가득 찼을 때 나올 말풍선 프리팹")]
    private GameObject speech_Box_Full; //사람이 가득찼을 때 나올 말풍선 프리팹 

    //---자리 관련 boolean 값 ---//
    private bool isFull = false;         //자리가 가득차있는지 아닌지 결정하는 변수
    private bool isArrive = false;      //도착 여부 판단 변수 (자리)
    private bool isReturn = false;      //자리에서 돌아갈 때 사용할 변수 (중복 실행 방지)
    private bool isJustreturn = false;  //자리가 가득차 그냥 돌아갈 때 사용할 변수

    //---손님 움직임 관련 변수---//
    private int wayPointIndex;      //경로 WayPoint의 Index 손님의 다음 움직임에 관여하기 위함

    //---각 경로로 가는 Waypoints를 담을 리스트---// 

    ///////////////////////////////////////////////////////////////////
    ///                                                             ///
    ///                      [Serving Table]                        ///
    ///                                                             ///
    ///     Seat0 [Dansang1] Seat1      Seat2 [Dansang2] Seat3      ///
    ///                                                             ///
    ///     Seat4 [Dansang3] Seat5      Seat6 [Dansang4] Seat7      ///
    ///                                                             ///
    ///     Seat8 [Dansang5] Seat9     Seat10 [Dansang6] Seat11     ///
    ///                                                             ///
    ///////////////////////////////////////////////////////////////////

    private List<Transform> Route1_Left;        //Seat0
    private List<Transform> Route1_Right;       //Seat1

    private List<Transform> Route2_Left;        //Seat2
    private List<Transform> Route2_Right;       //Seat3

    private List<Transform> Route3_Left;        //Seat4
    private List<Transform> Route3_Right;       //Seat5

    private List<Transform> Route4_Left;        //Seat6
    private List<Transform> Route4_Right;       //Seat7

    private List<Transform> Route5_Left;        //Seat8
    private List<Transform> Route5_Right;       //Seat9

    private List<Transform> Route6_Left;        //Seat10
    private List<Transform> Route6_Right;       //Seat11

    //---다양한 경로를 저장할 경로 리스트---//
    private List<List<Transform>> RouteList = new List<List<Transform>>();

    //---최종 결정된 Route---//
    private List<Transform> FinRoute;

    //---특수 waypoint---//
    private Transform StartPoint;
    private Transform StopPoint; // 자리가 꽉찼을 경우 멈출 포인트

    //---데이터---//
    private Data data;

    //---주문 관련---//
    private OrderMenu orderMenu;

    //---렌더링 변경할 스프라이트렌더러---//
    private SpriteRenderer spriteRenderer;

    //---손님 애니메이터---//
    private Animator animator;

    //---주막 씬 컴포넌트 불러오기 위한 변수---//
    private JumakScene jumakScene;

    //---위치 계산을 할 이전 벡터---//
    private Vector3 curPosition;        //손님의 현재 위치 (방향 계산을 위함)
    private Vector3 lastPosition;       //손님의 가장 최근 위치 (방향 계산을 위함)
    private Vector3 currentDir;         //현재 손님의 방향 (애니메이션을 위함)

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        //--- Initialize Customer System's Variables ---//
        seatIndex = 0;

        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //--- 경로를 초기화 하기 위한 인라인 함수 ---//
        InitializeRoute();

        StartPoint = GameObject.Find("StartPoint").transform;   //손님의 시작 위치
        StopPoint = GameObject.Find("StopPoint").transform;     //손님의 정지 위치 (자리가 가득 찼을 경우 멈추는 Point)

        //--- Load Data ---//
        data = DataManager.Instance.data;

        //--- Get Component ---//
        orderMenu = GetComponent<OrderMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        jumakScene = GameObject.Find("@Scene").GetComponent<JumakScene>();

        //--- 방향을 불러오기 위해 lastPosition을 현재 Transform 위치로 초기화 ---//
        lastPosition = transform.position;

        //--- 새로운 경로를 불러오기 위한 인라인 함수 ---//
        SetNewSeat();                                          
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        //--- Customer Moving System ---//
        CustomerMovingSystem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- 셋팅을 초기화하기 위한 인라인 함수 ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
    {
        //--- Initialize Customer Movement's Variables ---//
        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //--- Initialize Other Component Variables ---//
        orderMenu.isEat = false;

        //--- 새로운 자리 결정하는 인라인 함수 ---//
        SetNewSeat();

        //--- 초기화 진행한 후 풀링 리턴 오브젝트 ---//
        ObjectPool.ReturnObject(this);
    }

    //--- 손님들의 동선 Waypoints를 초기화하는 인라인 함수 ---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)] //AggressiveInlinng : 가능할 경우 인라인
    private void InitializeRoute()
    {
        //Route1 = Table1 왼쪽 자리
        Route1_Left.Add(GameObject.Find("Left_4").transform);
        Route1_Left.Add(GameObject.Find("Left_1").transform);
        Route1_Left.Add(GameObject.Find("Seat_L1").transform);

        //Route2 = Table1 오른쪽
        Route1_Right.Add(GameObject.Find("Center_4").transform);
        Route1_Right.Add(GameObject.Find("Center_1").transform);
        Route1_Right.Add(GameObject.Find("Seat_R1").transform);

        //Route3 = Table2 왼쪽 자리
        Route2_Left.Add(GameObject.Find("Center_4").transform);
        Route2_Left.Add(GameObject.Find("Center_1").transform);
        Route2_Left.Add(GameObject.Find("Seat_L2").transform);

        //Route4 = Table2 오른쪽 자리
        Route2_Right.Add(GameObject.Find("Right_4").transform);
        Route2_Right.Add(GameObject.Find("Right_1").transform);
        Route2_Right.Add(GameObject.Find("Seat_R2").transform);

        //Route5 = Table3 왼쪽 자리
        Route3_Left.Add(GameObject.Find("Left_4").transform);
        Route3_Left.Add(GameObject.Find("Left_2").transform);
        Route3_Left.Add(GameObject.Find("Seat_L3").transform);

        //Route6 = Table3 오른쪽 자리
        Route3_Right.Add(GameObject.Find("Center_4").transform);
        Route3_Right.Add(GameObject.Find("Center_2").transform);
        Route3_Right.Add(GameObject.Find("Seat_R3").transform);

        //Route7 = Table4 왼쪽 자리
        Route4_Left.Add(GameObject.Find("Center_4").transform);
        Route4_Left.Add(GameObject.Find("Center_2").transform);
        Route4_Left.Add(GameObject.Find("Seat_L4").transform);

        //Route8 = Table4 오른쪽 자리
        Route4_Right.Add(GameObject.Find("Right_4").transform);
        Route4_Right.Add(GameObject.Find("Right_2").transform);
        Route4_Right.Add(GameObject.Find("Seat_R4").transform);

        //Route9 = Table5 왼쪽 자리
        Route5_Left.Add(GameObject.Find("Left_4").transform);
        Route5_Left.Add(GameObject.Find("Left_3").transform);
        Route5_Left.Add(GameObject.Find("Seat_L5").transform);

        //Route10 = Table5 오른쪽 자리
        Route5_Right.Add(GameObject.Find("Center_4").transform);
        Route5_Right.Add(GameObject.Find("Center_3").transform);
        Route5_Right.Add(GameObject.Find("Seat_R5").transform);

        //Route11 = Table6 왼쪽 자리
        Route6_Left.Add(GameObject.Find("Center_4").transform);
        Route6_Left.Add(GameObject.Find("Center_3").transform);
        Route6_Left.Add(GameObject.Find("Seat_L6").transform);

        //Route12 = Table6 오른쪽 자리
        Route6_Right.Add(GameObject.Find("Right_4").transform);
        Route6_Right.Add(GameObject.Find("Right_3").transform);
        Route6_Right.Add(GameObject.Find("Seat_R6").transform);

        //RouteList에 순서대로 Add
        RouteList.Add(Route1_Left);
        RouteList.Add(Route1_Right);
        RouteList.Add(Route2_Left);
        RouteList.Add(Route2_Right);
        RouteList.Add(Route3_Left);
        RouteList.Add(Route3_Right);
        RouteList.Add(Route4_Left);
        RouteList.Add(Route4_Right);
        RouteList.Add(Route5_Left);
        RouteList.Add(Route5_Right);
        RouteList.Add(Route6_Left);
        RouteList.Add(Route6_Right);
    }

    //--- 앉을 자리 결정하는 인라인 함수 ---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)]
    private void SetNewSeat()
    {
        int count = 0; //반복 횟수를 따질 변수

        //--- 자리가 가득찼는지 검사 ---//
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (data.isAllocated[i])
                count++;
        }

        //--- 자리만큼 반복을 했을 경우 isFull = true 하고 리턴 ---//
        if (count == data.maxSeatSize)
        {
            isFull = true;
            return;
        }
        else isFull = false;

        //--- 랜덤으로 자리 결정 ---//
        int randomSeat = Random.Range(0, data.maxSeatSize);

        //--- 할당이 안되어 있는 자리를 찾을 때 까지 반복 ---//
        while (data.isAllocated[randomSeat])
        {
            randomSeat = Random.Range(0, data.maxSeatSize);
        }

        //--- 자리를 찾았다면 할당 ---//
        seatIndex = randomSeat;
        data.isAllocated[seatIndex] = true;

        //FinRoute에 할당된 경로를 입힘
        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    //---손님 기본 움직임에 관여하는 인라인 함수---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)]
    private void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(curPosition, TargetTransform.position, step);
    }

    //--- 손님 기본 움직임 시스템을 총괄하는 인라인 함수 ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CustomerMovingSystem()
    {
        //--- 게임 오버시 초기화 ---//
        if (!jumakScene.isStart) Initialize();

        //--- 애니메이션을 불러오기 위해 현재 손님이 향하고 있는 방향을 알아보기 위한 인라인 함수 ---//
        SetDirection();

        //---자리가 가득차 있지 않다면 움직임 수행---//
        if (!isFull)
        {
            //--- 현재 wayPointIndex의 값이 경로의 길이보다 작고, 도착하지 않은 상태라면---//
            if (wayPointIndex < FinRoute.Count && !isArrive)
            {
                //--- 손님 이동 '인라인' 함수 ---//
                CustomerMove(FinRoute[wayPointIndex]);

                //--- 목표했던 waypoint에 도착하면 인덱스 증가 ---//
                if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                    wayPointIndex++;
            }
            else if (!isArrive) //--- WayPoint Index를 모두 선회했을 때---//
            {
                //--- 중복 실행 방지를 위한 bool 변수 변경 ---//
                isArrive = true;

                //--- 애니메이션 실행 ---//
                animator.SetBool("isSit", true);

                //--- 손님이 앉았음을 표시하는 isCustomer bool 변수 변경 ---//
                data.isCustomer[seatIndex] = true;

                //--- 메뉴 주문 함수 실행 ---//
                orderMenu.OrderNewMenu();
            }

            //--- 음식을 다 먹었을 때 isReturn 활성화 ---//
            if (isReturn)
            {
                //--- WayPoint 역순회 함 ---//
                if (wayPointIndex >= 0)
                {
                    CustomerMove(FinRoute[wayPointIndex]);

                    if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                        wayPointIndex--;

                }
                //--- 모든 WayPoint를 역순회 했을 때 ---//
                else
                {
                    //--- 스폰 지점으로 되돌아감 ---//
                    CustomerMove(StartPoint);

                    //--- 스폰지점에 도착했을 때 초기화 ---//
                    if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();
                }
            }
        }
        //--- 자리가 없을 때 그냥 돌아가는 분기 ---//
        else if (isFull && !isArrive)
        {
            //--- 정지 구간까지 이동 ---//
            CustomerMove(StopPoint);

            //---정지 point와 거리가 0이라면 ---//
            if (Vector3.Distance(StopPoint.position, curPosition) == 0f)
            {
                //--- 중복 실행 방지 bool 변수 전환 ---//
                isArrive = true;
                //--- 애니메이션 실행 ---//
                animator.SetBool("isStop", true);
                //--- 자리가 가득 찼음을 알리는 SpeechBox 프리팹 생성 ---//
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("Speech_Boxes").transform);
                //--- 2초후 그냥 돌아가기 위한 JustOut Method 실행 ---//
                Invoke("JustOut", 2f);
            }
        }

        //--- 그냥 돌아가라는 신호가 인식 되었을 때 ---//
        if (isJustreturn)
        {
            //--- 시작 지점을 향해 이동 ---//
            CustomerMove(StartPoint);

            //--- 시작 지점에 도착했을 때 초기화 ---//
            if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();

        }
    }

    //--- 현재 손님 오브젝트의 방향 설정 및 렌더링 변경하기 위한 인라인 함수 ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetDirection()
    {
        //--- 현재 위치값을 불러옴 ---//
        curPosition = transform.position;

        //--- 방향 벡터 표준화 ---//
        currentDir = (curPosition - lastPosition).normalized;

        //--- 현재 방향이 0벡터가 아닐 경우 현재 방향에 맞게 애니메이션 변경 후 lastPostion Update ---//
        if (currentDir != Vector3.zero)
        {
            //--- 애니메이션 변경 ---//
            animator.SetFloat("dirX", currentDir.x);
            animator.SetFloat("dirY", currentDir.y);

            //--- 현재 위치를 최근 위치로 업데이트 ---//
            lastPosition = curPosition;
        }
        //--- 0벡터일 경우 기본 애니메이션 실행 ---//
        else animator.SetFloat("dirY", -1f);


        //---렌더링 변경---//
        if (curPosition.y < GameObject.Find("Chr_Player").transform.position.y)     // 손님이 플레이어보다 아래에 있다면
            spriteRenderer.sortingLayerName = "UpThanPlayer";                       // 플레이어보다 위에 렌더링
        else                                                                        // 손님이 플레이어보다 위에 있다면
            spriteRenderer.sortingLayerName = "Player";                             // 플레이어보다 아래에 렌더링
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- 음식을 먹기 시작했을 때 함수 --//
    public void EatFood()
    {
        //--- 음식 먹는 애니메이션 실행 ---//
        animator.SetBool("isEat", true);
        //--- 음식 먹고 되돌아가는 EanTandLeave Method 3f초후 실행 ---//
        Invoke("EatAndLeave", 3f);
    }

    //--- 음식을 다 먹고 밖으로 돌아가는 함수 ---//
    public void EatAndLeave()
    {
        //--- 일어나는 애니메이션 실행 ---//
        animator.SetBool("isEat", false);
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", -currentDir.x);

        //--- 돌아가기 위해 bool 변수 변환 ---//
        isReturn = true;
        //--- 자리 할당 해제 ---//
        data.isAllocated[seatIndex] = false;
        //--- 돌아가기 위해 wayPointIndex 1감소 ---//
        wayPointIndex--;
        //--- 손님 수 감소 ---//
        data.customerHeadCount--;
        //--- 해당 테이블에서 손님 감지 bool 신호 변환 ---//
        data.isCustomer[seatIndex] = false;
        //--- 테이블 위 음식 감지 bool 신호 변환 ---//
        data.onTables[seatIndex] = false;
        //--- 음식 다 먹음을 감지하는 bool 신호 변환 ---//
        data.isFinEat[seatIndex] = true;
    }

    //--- 자리가 없어서 그냥 밖으로 돌아가기 위해 신호를 바꾸는 함수 ---//
    private void JustOut()
    {
        //--- 애니메이션 실행(정지 해제) ---//
        animator.SetBool("isStop", false);
        //--- SpeechBox 프리팹 Destroy ---//
        Destroy(GameObject.Find("UI_Speech_Box_Full(Clone)"));
        //--- 그냥 돌아가기 위해 bool 변수 전환 ---//
        isJustreturn = true;
    }

    //--- 주문 대기 시간이 모두 지났을 때 한번 실행할 함수 ---//
    public void TimeOut()
    {
        //--- 애니메이션 실행 ---//
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", currentDir.x * -1);

        //--- 역순회를 하기 위해 bool 변수 변환 ---//
        isReturn = true;
        //--- 손님 수 감소 ---//
        data.customerHeadCount--;
        //--- 자리 할당 해제 ---//
        data.isAllocated[seatIndex] = false;
        //--- wayPointIndex 1 감소 ---//
        wayPointIndex--;
        //--- 해당 테이블에서 손님 감지 bool 신호 변환 ---//
        data.isCustomer[seatIndex] = false;
    } 
}
