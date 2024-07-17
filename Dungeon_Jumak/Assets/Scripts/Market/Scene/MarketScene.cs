using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketScene : BaseScene
{
    public TextMeshProUGUI CoinText;
    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        CoinText.text = data.curCoin.ToString();
    }

    void Start()
    {
        GameManager.Sound.Play("BGM/[B] Shop", Define.Sound.Bgm, true);
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("WaitingScene");
    }

    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    protected override void Init()
    {
        SceneType = Define.Scene.Market;
    }

    public override void Clear()
    {
        Debug.Log("Market Scene changed!");
    }
}
