using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSystem : MonoBehaviour
{
    private Data data;
    void Start()
    {
        data = DataManager.Instance.data;
    }

    public void SkipToNextDay()
    {
        data.IsMorning = true;
    }
}
