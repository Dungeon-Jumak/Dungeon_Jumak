using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    public GameObject movingPanel;

    void Start()
    {
        GameManager.Sound.Play("[B] Map Front", Define.Sound.Bgm, true);
    }

    //Current Scene Settings
    protected override void Init()
    {
        SceneType = Define.Scene.Map;
    }

    //When convert to next scene
    public override void Clear()
    {
        Debug.Log("Map Scene changed!");
    }

    //Activate StartDunjeon panel when select going to dunjeon btn
    public void OpenMovingPanel()
    {
        movingPanel.SetActive(true);//activate the StartDunjeon panel
        StartCoroutine(LoadSceneAfterDelay(1)); 
    }

    //Click Sound
    public void ButtonClickSFX()
    {
        GameManager.Sound.Play("[S] Push Button", Define.Sound.Effect, false);
    }

    //IEnumerator function
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Scene.LoadScene(Define.Scene.Stage1);
    }

    //Convert to WaitingScene
    public void ConvertScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }
}
