using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingScene : MonoBehaviour
{
    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;
        GameManager.Sound.Play("Wait", Define.Sound.Bgm);
    }
}
