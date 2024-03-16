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

    //---�ڸ� ���� boolean �� ---//
    [SerializeField]
    private bool isFull = false; //�ڸ��� �������ִ��� �ƴ��� �����ϴ� ����
    [SerializeField]
    private bool isArrive = false; //���� ���� �Ǵ� ���� (�ڸ�)
    [SerializeField]
    private bool isReturn = false; //�ڸ����� ���ư� �� ����� ���� (�ߺ� ���� ����)
    [SerializeField]
    private bool isJustreturn = false;

    [SerializeField]
    private Vector3 CurPosition;
    [SerializeField]
    private int WayPointIndex = 0;
    [SerializeField]
    private float speed = 3f;

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
        //--- ���� ��ġ�� ---//
        CurPosition = transform.position;

        //---�ڸ��� ������ ���� �ʴٸ� ������ ����---//
        if (!isFull)
        {
            //--- ���� waypointIndex�� ���� ����� ���̺��� �۴ٸ� ---//
            //--- �̵����� �迭�� �ε��� 0���� �迭ũ�� -1���� ---//
            if (WayPointIndex < FinRoute.Count && !isArrive)
            {
                //---�����Ӵ� �̵�---//
                CustomerMove(FinRoute[WayPointIndex]);

                //---waypoint�� �����ϸ� �ε��� ����---//
                if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                    WayPointIndex++;
            }
            else if(!isArrive)
            {
                isArrive = true;
                //---������ ���� �� ���ư���---//
                Invoke("ReturnSeatToOut", 3f);
            }

            //---�������� ��� 3���� ���ư� Invoke Return�� ���� isArrive�� True�� ��ȯ(�Ŀ� �������� �ٲ�ߵ�)---//
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
                        //Destroy(this.gameObject);
                        this.gameObject.SetActive(false);
                    }

                }
            }
        }
        else if(isFull && !isArrive)
        {
            CustomerMove(StopPoint);

            //---���� point�� �Ÿ��� 0�� �ƴ϶�� ---//
            if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
            {
                isArrive = true;
                //�ִϸ��̼� �߰� (�θ��� �θ���?)
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

    //---�մ� �⺻ ������---//
    void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(CurPosition, TargetTransform.position, step);
    }


    //---�ڸ����� ������ ���ư���---//
    void ReturnSeatToOut()
    {
        isReturn = true;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;
    }

    //---�ڸ��� ��� �׳� ������ ���ư���---//
    void ReturnStopToOut()
    {
        isJustreturn = true;
    }

    //---���� �ڸ� ����---//
    void MovingSystem()
    {
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (!data.isAllocated[i])
            {
                seatIndex = i;
                data.isAllocated[i] = true; //�ڸ� �Ҵ�
                return;
            }
        }
        isFull = true;
        Debug.Log("�ڸ��� ����");
    }

}
