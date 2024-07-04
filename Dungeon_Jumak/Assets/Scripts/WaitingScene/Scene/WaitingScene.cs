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
    public GameObject[] gameObjects;

    [Header("로딩 패널")]
    public GameObject loadingPanel;

    [Header("버튼 이미지 배열")]
    public Image[] images;

    [Header("스트로크 스프라이트 배열")]
    public Sprite[] strokes;

    void Start()
    {
        GameManager.Sound.Play("BGM/[B] Waiting Scene", Define.Sound.Bgm, true);//Play The Bgm Sound in WaitingScene
    }

    void Update()
    {
    }

    void OnEnable()
    {
        DisableGameObjects();
    }

    //Current Scene Settings
    protected override void Init()
    {
        SceneType = Define.Scene.WaitingScene;
    }

    public void ChangeScene(string sceneName)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(ChangeSceneAfterDelay(sceneName, 2f));
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

    private void DisableGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void ActiveStroke(int index)
    {
        images[index].sprite = strokes[index];
    }
}