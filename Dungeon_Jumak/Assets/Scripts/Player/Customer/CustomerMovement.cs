using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMovement : MonoBehaviour
{
    //각 경로로 가는 Waypoints를 담을 리스트 
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

    //---자리 관련 변수---//
    [SerializeField]
    private int seatIndex = 0;

    //---UI 관련 변수 (Speech_Box)---//
    [SerializeField]
    private GameObject speech_Box_Full; //사람이 가득찼을 때 나올 말풍선 프리팹 

    //---데이터---//
    [SerializeField]
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;

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

        MovingSystem();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    private void Update()
    {

        //--- 현재 위치값 ---//
        CurPosition = transform.position;

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
            else if(!isArrive)
            {
                isArrive = true;
                //---국밥을 먹은 후 돌아가기---//
                Invoke("ReturnSeatToOut", 3f);
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
                        //수정완료
                        ObjectPool.ReturnObject(this);
                    }

                }
            }
        }
        //---자리가 없을 때 그냥 돌아감---//
        else if(isFull && !isArrive)
        {
            CustomerMove(StopPoint);

            //---정지 point와 거리가 0이 아니라면 ---//
            if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
            {
                isArrive = true;
                //애니메이션 추가 (두리번 두리번?)
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("If_Full").transform);
                Invoke("ReturnStopToOut", 2f);
            }     
        }

        if (isJustreturn)
        {
            CustomerMove(StartPoint);

            if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
                this.gameObject.SetActive(false);
        }
    }

    //---손님 기본 움직임---//
    void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(CurPosition, TargetTransform.position, step);
    }


    //---자리에서 밖으로 돌아가기---//
    void ReturnSeatToOut()
    {
        isReturn = true;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;
    }

    //---자리가 없어서 그냥 밖으로 돌아가기---//
    void ReturnStopToOut()
    {
        //***수정 필요***//
        GameObject.Find("UI_Speech_Box_Full(Clone)").SetActive(false);
        isJustreturn = true;
    }

    //---앉을 자리 결정---//
    void MovingSystem()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i])
            {
                seatIndex = i;
                data.isAllocated[i] = true; //자리 할당
                return;
            }
        }
        isFull = true;
        Debug.Log("자리가 없음");
    }




}
