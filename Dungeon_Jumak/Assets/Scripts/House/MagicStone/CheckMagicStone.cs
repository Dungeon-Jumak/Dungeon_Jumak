using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CheckMagicStone : MonoBehaviour
{
    public TextMeshProUGUI magicStoneText;
    private Data data;

    private void Awake()
    {
        data = DataManager.Instance.data;
    }

    public void OnEnable()
    {
        magicStoneText.text = data.magicStone.ToString() + " / 1000";
    }
}
