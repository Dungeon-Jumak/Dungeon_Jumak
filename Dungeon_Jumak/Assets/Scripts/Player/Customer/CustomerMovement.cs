using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMovement : MonoBehaviour
{
    //---�� ��η� ���� Waypoints�� ���� ����Ʈ---// 
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

    //---�پ��� ��θ� ������ ��� ����Ʈ---//
    public List<List<Transform>> RouteList = new List<List<Transform>>();

    //---���� ������ Route---//
    public List<Transform> FinRoute;

    //---�ڸ� ���� ����---//
    public int seatIndex = 0;

    //---�Ա� ������ �˸��� ����---//
    public bool isEat = false;

    //---Ÿ�� �ƿ��� �˸��� ����---//
    public bool isTimeOut = false;

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


    //---������Ʈ Ǯ�� �ʱ�ȭ�� ���� boolean ��---//
    [SerializeField]
    private bool isInitialize = false;

    //---�մ� ������ ���� ����---//
    [SerializeField]
    private Vector3 CurPosition;
    [SerializeField]
    private int WayPointIndex = 0;
    [SerializeField]
    private float speed = 3f;

    //---UI ���� ���� (Speech_Box)---//
    [SerializeField]
    private GameObject speech_Box_Full; //����� ����á�� �� ���� ��ǳ�� ������ 

    //---������---//
    [SerializeField]
    private Data data;

    //---�ִϸ��̼� ������ �׽�Ʈ�� ��������Ʈ---//
    [SerializeField]
    private Sprite sitSprite;
    [SerializeField]
    private Sprite standSprite;

    //---���� �մ��� ������ ������ ����---//
    [SerializeField]
    private Vector2 dir;

    //---�ֹ� ����---//
    [SerializeField]
    private OrderMenu orderMenu;

    private void Start()
    {
        data = DataManager.Instance.data;
        orderMenu = GetComponent<OrderMenu>();

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
        //---���� �� �ʱ�ȭ---//
        if (isInitialize) Initialize();


        //--- ���� ��ġ�� ---//
        CurPosition = transform.position;

        //---������ �Ա� �������� ��---//
        if (isEat)
        {
            isEat = false;
            Invoke("ReturnSeatToOut", 3f);
        }

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

                //***���߿� �ִϸ��̼����� ����***//
                /*this.gameObject.GetComponent<SpriteRenderer>().sprite = sitSprite;
                if(seatIndex % 2 != 0)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                */

                orderMenu.OrderNewMenu();
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
                        ObjectPool.ReturnObject(this);
                        isInitialize = true;
                    }                  
                }
            }
        }
        //---�ڸ��� ���� �� �׳� ���ư�---//
        else if(isFull && !isArrive)
        {
            CustomerMove(StopPoint);

            //---���� point�� �Ÿ��� 0�� �ƴ϶�� ---//
            if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
            {
                isArrive = true;
                //�ִϸ��̼� �߰� (�θ��� �θ���?)
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("If_Full").transform);
                Invoke("ReturnStopToOut", 2f);
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

    //---�մ� �⺻ ������---//
    void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(CurPosition, TargetTransform.position, step);
    }


    //---�ڸ����� ������ ���ư���---//
    public void ReturnSeatToOut()
    {
        isReturn = true;
        data.curSeatSize--;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;

        data.onTables[seatIndex] = false;
        data.isFinEat[seatIndex] = true;

        //�Ͼ�� �ִϸ��̼� �߰�
        //this.gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
    }

    //---�ڸ��� ��� �׳� ������ ���ư���---//
    void ReturnStopToOut()
    {
        //***���� �ʿ�***//
        GameObject.Find("UI_Speech_Box_Full(Clone)").SetActive(false);
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
                isFull = false;
                return;
            }
        }
        isFull = true;
        Debug.Log("�ڸ��� ����");
    }

    //---���� �ʱ�ȭ---//
    void Initialize()
    {
        WayPointIndex = 0;

        MovingSystem();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }

        isEat = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;
        isInitialize = false;
        orderMenu.isRun = true;
    }

    public void TimeOut()
    {
        Debug.Log("����Ÿ�Ӿƿ����");


        isReturn = true;
        data.curSeatSize--;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;
    }


}
