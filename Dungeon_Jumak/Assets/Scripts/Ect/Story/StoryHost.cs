//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StoryHost : MonoBehaviour
{
    [Header("만화 페이지")]
    [SerializeField] private GameObject[] pages;

    [Header("만화 컷")]
    [SerializeField] private Image[] cuts;

    [Header("시작 씬")]
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
        pages[0].SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(3f);

        pages[1].SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(3f);

        pages[2].SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeInCut(cutIndex));

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        startScene.ConvertScene();
    }

    IEnumerator FadeInCut(int idx)
    {
        for (float i = 0; i <= 1; i += 0.1f)
        {
            cuts[idx].color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.01f);
        }

        cutIndex++;
    }

}
