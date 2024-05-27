//Newtonsoft
using Newtonsoft.Json.Bson;

//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectPool : MonoBehaviour
{
    //Static This Obeject
    [Header("������Ʈ Ǯ �ν��Ͻ�")]
    public static ObjectPool Instance;

    //Pooling Object Prefab
    [Header("Ǯ���� ����� �մ� ������Ʈ ������")]
    [SerializeField] private GameObject poolingCustomerPrefab;

    //Parent of Pooling Object
    [Header("Ǯ���� �մ��� �θ� ������Ʈ")]
    [SerializeField] private static Transform parentOfActivedCustomer;

    //Pooling Queue
    Queue<CustomerMovement> poolingObjectQueue = new Queue<CustomerMovement>();

    //Awake
    private void Awake()
    {
        //Get Instance to this obejct
        Instance = this;

        //Initialize object pool (10 is object count)
        Initialize(10);
    }

    private void Start()
    {
        parentOfActivedCustomer = GameObject.Find("Customer's Parent").transform;
    }

    //Initialize Pool
    private void Initialize(int intCount)
    {
        //Enqueue by count
        for (int i = 0; i < intCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    //Create new object
    private CustomerMovement CreateNewObject()
    {
        //Instantiate Pool Object
        var newObj = Instantiate(poolingCustomerPrefab).GetComponent<CustomerMovement>();

        //Setactive is false
        newObj.gameObject.SetActive(false);

        //Set Parent
        newObj.transform.SetParent(transform);

        //Return Created Obejct
        return newObj;
    }

    //Get Object
    public static CustomerMovement GetObject()
    {
        //If pool have object
        if(Instance.poolingObjectQueue.Count > 0)
        {
            //dequeue in pool queue
            var obj = Instance.poolingObjectQueue.Dequeue();

            //SetParent
            obj.transform.SetParent(parentOfActivedCustomer);

            //Active SetActive
            obj.gameObject.SetActive(true);

            //Debug.log
            Debug.Log("������Ʈ Ǯ�� ��ť.");

            //Return dequeued obeject
            return obj;
        }
        //If pool have not object
        else
        {
            //Create New Object
            var newObj = Instance.CreateNewObject();

            //Active SetActive
            newObj.gameObject.SetActive(true);

            //Set Parent
            newObj.transform.SetParent(parentOfActivedCustomer);

            //Return new object
            return newObj;
        }
    }

    //Return Object
    public static void ReturnObject(CustomerMovement obj)
    {
        //Inactive SetActive
        obj.gameObject.SetActive(false);

        //Change Parent -> pool
        obj.transform.SetParent(Instance.transform);

        //Enqueue in pool queue
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
