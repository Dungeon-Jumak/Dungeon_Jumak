using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    public GameObject movingPanel;

    [SerializeField]
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;
        GameManager.Sound.Play("Map_Front", Define.Sound.Effect);
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

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Scene.LoadScene(Define.Scene.MainDunjeon);
    }
}
