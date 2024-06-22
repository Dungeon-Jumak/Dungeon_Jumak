using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    public GameObject movingPanel;

    void Start()
    {
        GameManager.Sound.Play("BGM/[B] Map Front", Define.Sound.Bgm, false);
        StartCoroutine(PlayMusicAfterDelay(62));
    }

    IEnumerator PlayMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Sound.Play("BGM/[B] Map Back", Define.Sound.Bgm, true);
    }

    protected override void Init()
    {
        SceneType = Define.Scene.Map;
    }

    public override void Clear()
    {
        Debug.Log("Map Scene changed!");
    }

    public void OpenMovingPanel()
    {
        movingPanel.SetActive(true);
        StartCoroutine(LoadSceneAfterDelay(1));
    }

    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Scene.LoadScene(Define.Scene.Stage1);
    }

    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }
}
