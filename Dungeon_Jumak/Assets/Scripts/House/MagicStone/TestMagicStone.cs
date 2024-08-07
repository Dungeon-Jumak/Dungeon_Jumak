using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMagicStone : MonoBehaviour
{
    private Data data;

    private void Awake()
    {
        data = DataManager.Instance.data;
    }

    public void MagicStoneCountUp()
    {
        data.magicStone++;
    }
}
