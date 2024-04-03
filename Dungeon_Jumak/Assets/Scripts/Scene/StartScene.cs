using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private int bgmSoundTrack;
    [SerializeField]
    private float maxVolume;

    [SerializeField]
    private Data data;

    // Start is called before the first frame update
    void Start()
    {
        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();

        bgmSoundTrack = 1;
        bgmManager.Play(bgmSoundTrack);
    }

    // Update is called once per frame
    void Update()
    {
        if (!data.isPlayBGM)
        {
            data.isPlayBGM = true;
            bgmManager.Play(bgmSoundTrack);
            bgmManager.FadeInMusic(maxVolume);
            bgmManager.SetLoop();
        }
    }

    public void ConvertScene(string _sceneName)
    {
        bgmManager.FadeOutMusic();
        SceneManager.LoadScene(_sceneName);
    }
}
