using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    //�� ��η� ���� Waypoints�� ���� ����Ʈ 
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

    //---Ư�� waypoint---//
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform StopPoint; // �ڸ��� ��á�� ��� ���� ����Ʈ

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
            //--- �̵� ���� ���� ���� ---//
            CurPosition = transform.position;

            //--- �̵����� �迭�� �ε��� 0���� �迭ũ�� -1���� ---//
            if (WayPointIndex < FinRoute.Count && !isArrive)
            {
                //---�����Ӵ� �̵�---//
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(CurPosition, FinRoute[WayPointIndex].position, step);

                //---waypoint�� �����ϸ� �ε��� ����---//
                if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                    WayPointIndex++;
            }
            else
            {
                Invoke("Return", 3f);
            }

            //---�������� ��� 3���� ���ư� Invoke Return�� ���� isArrive�� True�� ��ȯ---//
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

    //---�ڸ����� ������ ���ư���---//
    void Return()
    {
        if (!isArrive)
        {
            isArrive = true;
            data.isAllocated[seatIndex] = false; //�ڸ� �Ҵ� ����
            WayPointIndex--;
        }
    }

    //---������ ����---//
    void MovingSystem()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i])
            {
                seatIndex = i;
                data.isAllocated[i] = true; //�ڸ� �Ҵ�
                break;
            }
        }
    }

    //---�ڸ��� �� á�� ��� ���ư� SeatFull -> ToBack---//
    void SeatFull()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i]) return;   
        }
        isFull = true;
    }

}
