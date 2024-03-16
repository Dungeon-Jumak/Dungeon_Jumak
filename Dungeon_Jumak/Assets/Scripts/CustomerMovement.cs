using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public List<List<Transform>> RouteList = new List<List<Transform>>();

    public List<Transform> FinRoute;

    //---특수 waypoint---//
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform StopPoint; // 자리가 꽉찼을 경우 멈출 포인트

    [SerializeField]
    private bool isFull;

    [SerializeField]
    private Vector3 CurPosition;
    [SerializeField]
    private int WayPointIndex = 0;
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private bool isArrive = false;
    [SerializeField]
    private int seatIndex = 0;

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
        if (!isFull)
        {
            //--- 이동 중인 현재 변수 ---//
            CurPosition = transform.position;

            //--- 이동지점 배열의 인덱스 0부터 배열크기 -1까지 ---//
            if (WayPointIndex < FinRoute.Count && !isArrive)
            {
                //---프레임당 이동---//
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(CurPosition, FinRoute[WayPointIndex].position, step);

                //---waypoint에 도착하면 인덱스 증가---//
                if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                    WayPointIndex++;
            }
            else
            {
                Invoke("Return", 3f);
            }

            //---도착했을 경우 3초후 돌아감 Invoke Return을 통해 isArrive를 True로 전환---//
            if (isArrive)
            {
                if (WayPointIndex >= 0)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(CurPosition, FinRoute[WayPointIndex].position, step);

                    if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                        WayPointIndex--;
                }
                else
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(CurPosition, StartPoint.position, step);

                    if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
                    {
                        //Destroy(this.gameObject);
                        this.gameObject.SetActive(false);
                    }

                }
            }
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(CurPosition, StopPoint.position, step);

            //if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
                
        }
    }

    //---자리에서 밖으로 돌아가기---//
    void Return()
    {
        if (!isArrive)
        {
            isArrive = true;
            data.isAllocated[seatIndex] = false; //자리 할당 해제
            WayPointIndex--;
        }
    }

    //---목적지 설정---//
    void MovingSystem()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i])
            {
                seatIndex = i;
                data.isAllocated[i] = true; //자리 할당
                break;
            }
        }
    }

    //---자리가 꽉 찼을 경우 돌아감 SeatFull -> ToBack---//
    void SeatFull()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i]) return;   
        }
        isFull = true;
    }

}
