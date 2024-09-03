//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StoryHost : MonoBehaviour
{
    [Header("��ȭ ��")]
    [SerializeField] private GameObject[] cuts;

    [Header("���� ��")]
    [SerializeField] private StartScene startScene;

    private int cutIndex = 0;

    Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    public void StartStoryButton()
    {
        if (data.isFirstStart) StartCoroutine(StartStory());
        else startScene.ConvertScene();
    }

    IEnumerator StartStory()
    {
        for (int i = 0; i < cuts.Length; i++)
        {
            Debug.Log("����");

            cuts[cutIndex].SetActive(true);

            yield return new WaitForSeconds(3f);

            if (cutIndex != 3) cuts[cutIndex].SetActive(false);

            cutIndex++;
        }

        startScene.ConvertScene();
    }


}
