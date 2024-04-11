using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Text hpText;
    public float playerHPs = 3f;

    void Start()
    {
        UpdateHPText();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHPs <= 0)
        {
            hpText.text = "플레이어 사망";
            playerHPs = 0; 
        }
        else if (playerHPs > 0)
        {
            UpdateHPText();
        }

    }

    void UpdateHPText()
    {
        hpText.text = "Player HP: " + playerHPs.ToString();
    }
}
