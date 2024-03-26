using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMovement : MonoBehaviour
{
    //---각 경로로 가는 Waypoints를 담을 리스트---// 
    public List<Transform> Route1_Left;
    public List<Transform> Route1_Right;

    public List<Transform> Route2_Left;
    public List<Transform> Route2_Right;

    public List<Transform> Route3_Left;
    public List<Transform> Route3_Right;

    public List<Transform> Route4_Left;
    public List<Transform> Route4_Right;

    public List<Transform> Route5_Left;
    public List<Transform> Route5_Right;

    public List<Transform> Route6_Left;
    public List<Transform> Route6_Right;

    //---다양한 경로를 저장할 경로 리스트---//
    public List<List<Transform>> RouteList = new List<List<Transform>>();

    //---최종 결정된 Route---//
    public List<Transform> FinRoute;

    //---자리 관련 변수---//
    public int seatIndex = 0;

    //---특수 waypoint---//
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform StopPoint; // 자리가 꽉찼을 경우 멈출 포인트

    //---자리 관련 boolean 값 ---//
    [SerializeField]
    private bool isFull = false; //자리가 가득차있는지 아닌지 결정하는 변수
    [SerializeField]
    private bool isArrive = false; //도착 여부 판단 변수 (자리)
    [SerializeField]
    private bool isReturn = false; //자리에서 돌아갈 때 사용할 변수 (중복 실행 방지)
    [SerializeField]
    private bool isJustreturn = false;

    //---손님 움직임 관련 변수---//
    [SerializeField]
    private Vector3 CurPosition;
    [SerializeField]
    private int WayPointIndex = 0;
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private bool isInitialize = false;

    //---UI 관련 변수 (Speech_Box)---//
    [SerializeField]
    private GameObject speech_Box_Full; //사람이 가득찼을 때 나올 말풍선 프리팹 

    //---데이터---//
    [SerializeField]
    private Data data;

    //---주문 관련---//
    [SerializeField]
    private OrderMenu orderMenu;

    //---렌더링 변경할 스프라이트렌더러---//
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    //---손님 애니메이터---//
    private Animator animator;

    //---위치 계산을 할 이전 벡터---//
    private Vector3 lastPosition;
    private Vector3 currentDir;

    //---소리 관련---//
    private AudioManager audioManager;
    [SerializeField]
    private string eatSound; //국밥 먹는 소리

    private void Start()
    {
        data = DataManager.Instance.data;
        orderMenu = GetComponent<OrderMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        audioManager = FindObjectOfType<AudioManager>();

        lastPosition = transform.position;

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

        SetNewSeat();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    private void Update()
    {
        if (isInitialize) Initialize();

        SetDirection();

        //---자리가 가득차 있지 않다면 움직임 수행---//
        if (!isFull)
        {
            //--- 현재 waypointIndex의 값이 경로의 길이보다 작다면 ---//
            //--- 이동지점 배열의 인덱스 0부터 배열크기 -1까지 ---//
            if (WayPointIndex < FinRoute.Count && !isArrive)
            {
                //---프레임당 이동---//
                CustomerMove(FinRoute[WayPointIndex]);

                //---waypoint에 도착하면 인덱스 증가---//
                if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                    WayPointIndex++;
            }
            else if (!isArrive)
            {
                isArrive = true;

                data.isCustomer[seatIndex] = true; // 손님이 테이블 도착 체크

                animator.SetBool("isSit", true);

                orderMenu.OrderNewMenu();
            }

            //---도착했을 경우 3초후 돌아감 Invoke Return을 통해 isArrive를 True로 전환(후에 국밥으로 바꿔야됨)---//
            if (isReturn)
            {
                if (WayPointIndex >= 0)
                {
                    CustomerMove(FinRoute[WayPointIndex]);

                    if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                        WayPointIndex--;

                }
                else
                {
                    CustomerMove(StartPoint);

                    if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
                    {
                        ObjectPool.ReturnObject(this);
                        isInitialize = true;
                    }
                }
            }
        }
        //---자리가 없을 때 그냥 돌아감---//
        else if (isFull && !isArrive)
        {
            CustomerMove(StopPoint);

            //---정지 point와 거리가 0이 아니라면 ---//
            if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
            {
                isArrive = true;

                animator.SetBool("isStop", true);

                GameObject.Instantiate(speech_Box_Full, GameObject.Find("If_Full").transform);
                Invoke("JustOut", 2f);
            }
        }

        if (isJustreturn)
        {
            CustomerMove(StartPoint);

            if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
            {
                ObjectPool.ReturnObject(this);
                isInitialize = true;
            }

        }
    }

    //---셋팅 초기화---//
    void Initialize()
    {
        WayPointIndex = 0;

        SetNewSeat();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }

        isInitialize = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;
        orderMenu.isRun = true;
    }

    //---손님 기본 움직임---//
    void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(CurPosition, TargetTransform.position, step);
    }

    //---음식을 다 먹고 밖으로 돌아가기---//
    public void EatAndLeave()
    {
        //사운드 종료
        audioManager.Stop(eatSound);

        //일어나는 애니메이션 추가
        animator.SetBool("isEat", false);
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", -currentDir.x);

        isReturn = true;
        data.curSeatSize--;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;

        data.onTables[seatIndex] = false;
        data.isFinEat[seatIndex] = true;

        data.isCustomer[seatIndex] = false; // 손님 테이블에서 나가는 것 체크
    }

    //---자리가 없어서 그냥 밖으로 돌아가기---//
    void JustOut()
    {
        animator.SetBool("isStop", false);
        //***수정 필요***//
        GameObject.Find("UI_Speech_Box_Full(Clone)").SetActive(false);
        isJustreturn = true;
    }

    //---앉을 자리 결정---//
    void SetNewSeat()
    {
        int count = 0; //반복 횟수를 따질 변수

        //자리가 가득찼는지 검사
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (data.isAllocated[i])
                count++;
        }

        //자리만큼 반복을 했을 경우 isFull = true
        if (count == data.maxSeatSize)
        {
            isFull = true;
            return;
        }
        else isFull = false;

        //랜덤으로 자리 결정
        int randomSeat = Random.Range(0, data.maxSeatSize);

        //할당이 안되어 있는 자리를 찾을 때 까지 반복
        while (data.isAllocated[randomSeat])
        {
            randomSeat = Random.Range(0, data.maxSeatSize);
        }

        //자리를 찾았다면 할당
        seatIndex = randomSeat;
        data.isAllocated[seatIndex] = true;
    }

    

    public void TimeOut()
    {
        isReturn = true;
        data.curSeatSize--;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;

        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", currentDir.x * -1);

        data.isCustomer[seatIndex] = false; // 손님 테이블에서 나가는 것 체크
    }
    
    //---현재 손님 오브젝트의 방향 설정---//
    void SetDirection()
    {
        //--- 현재 위치값 ---//
        CurPosition = transform.position;

        currentDir = (CurPosition - lastPosition).normalized;

        if (currentDir != Vector3.zero)
        {
            animator.SetFloat("dirX", currentDir.x);
            animator.SetFloat("dirY", currentDir.y);
            lastPosition = CurPosition;
        }
        else animator.SetFloat("dirY", -1f);

        //---렌더링 변경---//
        if (CurPosition.y < GameObject.Find("Chr_Player").transform.position.y) //손님이 아래에 있다면
            spriteRenderer.sortingOrder = 2; //플레이어보다 위에 렌더링
        else
            spriteRenderer.sortingOrder = 0; //플레이어보다 아래 렌더링
    }

    //---음식을 먹기 시작했을 때--//
    public void EatFood()
    {
        if (!audioManager.IsPlaying(eatSound))
        {
            audioManager.SetLoop(eatSound);
            audioManager.Play(eatSound);
        }

        animator.SetBool("isEat", true);
        Invoke("EatAndLeave", 3f);
    }
}
