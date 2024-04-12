using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterBuChu : MonoBehaviour
{
    // public Text hpText;
    private Data data;
    void Awake()
    {
        data = DataManager.Instance.data;
        data.monsterHP = 10f;
    }

    private void Update()
    {
        if (data.monsterHP <= 0)
        {
            data.monsterHP = 0;
            StopAllCoroutines();
        }
    }


    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            CallHpFunc();
        }
    }

    void CallHpFunc()
    {
        data.playerHP -= 0.5f;
    }
}
