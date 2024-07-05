//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class DropItemPoolManager : MonoBehaviour
{
    //Prfabs GameObject
    public GameObject[] prefabs;

    [Header("풀 담당을 하는 리스트")]
    [SerializeField] List<GameObject>[] pools;

    private void Awake()
    {
        //Initialize
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    //Get Moethod
    public GameObject Get(int index)
    {
        //For Retrun
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            //Find Object in pool
            if (!item.activeSelf)
            {
                //select item
                select = item;

                //setactive true
                select.SetActive(true);

                //break;
                break;
            }
        }

        //If not find
        if (!select)
        {
            //instantiate
            select = Instantiate(prefabs[index], transform);

            //add pool
            pools[index].Add(select);
        }

        //return object
        return select;
    }
}
