using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarketScene : BaseScene
{
    [Header("경험치 슬라이더")]
    [SerializeField] private Slider xpSlider;

    [Header("코인 텍스트")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI levelText;

    private float lastXP;

    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        coinText.text = data.curCoin.ToString() + "전";

        xpSlider.value = data.curXP / data.maxXP;
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
