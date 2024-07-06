//System
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

//Unity
using UnityEngine;
using UnityEngine.UI;

//TMPro
using TMPro;

public class WaitingScene : BaseScene
{
    [Header("로딩 패널")]
    public GameObject loadingPanel;

    [Header("버튼 이미지 배열")]
    public Image[] images;

    [Header("스트로크 스프라이트 배열")]
    public Sprite[] strokes;

    //Current Scene Settings
    protected override void Init()
    {
        SceneType = Define.Scene.WaitingScene;
    }

    void Start()
    {
        GameManager.Sound.Play("BGM/[B] Waiting Scene", Define.Sound.Bgm, true);//Play The Bgm Sound in WaitingScene
    }

    //Change to next scene with loading panel
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneAfterDelay(sceneName, 2f));
        loadingPanel.SetActive(true);
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Enum.TryParse(sceneName, out Define.Scene scene))
        {
            GameManager.Scene.LoadScene(scene);
        }
    }

    //Click Sound
    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("Waiting Scene changed!");
    }

    public void ActiveStroke(int index)
    {
        images[index].sprite = strokes[index];
    }
}