using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject poolingCustomerPrefab;

    Queue<CustomerMovement> poolingObjectQueue = new Queue<CustomerMovement>();

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private void Initialize(int intCount)
    {
        for (int i = 0; i < intCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private CustomerMovement CreateNewObject()
    {
        var newObj = Instantiate(poolingCustomerPrefab).GetComponent<CustomerMovement>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static CustomerMovement GetObject()
    {
        if(Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            Debug.Log("오브젝트 풀링 디큐.");
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(CustomerMovement obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
