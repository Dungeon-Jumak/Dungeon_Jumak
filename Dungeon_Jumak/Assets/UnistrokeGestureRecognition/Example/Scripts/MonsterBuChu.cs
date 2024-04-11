using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MonsterBuChu : MonoBehaviour
{
    public PlayerHP playerhp;
    public Text hpText;
    public float MonsterHPs = 3f;

    private void Update()
    {
        if (MonsterHPs <= 0)
        {
            hpText.text = "¸ó½ºÅÍ »ç¸Á";
            MonsterHPs = 0;
            StopAllCoroutines();
        }
        else if (MonsterHPs > 0)
        {
            UpdateHPText();
        }
    }

    void UpdateHPText()
    {
        hpText.text = "Monster HP: " + MonsterHPs.ToString();
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
        if (playerhp != null)
        {
            playerhp.playerHPs -= 0.5f;
        }
    }
}
