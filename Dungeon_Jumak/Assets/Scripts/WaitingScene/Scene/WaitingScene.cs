using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using TMPro;

public class WaitingScene : BaseScene
{
    public GameObject[] gameObjects;

    //TimerText ���� ������
    public GameObject timerObj;

    void Start()
    {
        GameManager.Sound.Play("BGM/[B] Waiting Scene", Define.Sound.Bgm, true);//Play The Bgm Sound in WaitingScene
    }

    void Update()
    {
        timerObj.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.timerText;
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
        StartCoroutine(ChangeSceneAfterDelay(sceneName, 1.0f)); // 1초 지연 후 씬 변경 코루틴 시작
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 지연 시간 기다림

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

    // 게임 오브젝트를 비활성화하는 메서드
    private void DisableGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}