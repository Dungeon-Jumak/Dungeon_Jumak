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

    private bool playBGM = false;

    // Start is called before the first frame update
    void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();

        //---BGM ���� Ʈ�� ����---//
        bgmSoundTrack = 0;

        //---�⺻ BGM ����---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!data.isPlayBGM)
        {
            playBGM = false;

            bgmManager.CancelLoop();
            bgmManager.Stop();
        }

        if(!playBGM && data.isPlayBGM)
        {
            playBGM = true;

            bgmManager.Play(bgmSoundTrack);
            bgmManager.FadeInMusic(maxVolume);
            bgmManager.SetLoop();
        }
    }


    /// <summary>
    /// ���Ŵ����� �ٲ�� ��
    /// </summary>
    /// <param name="_sceneName"></param>
    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        SceneManager.LoadScene(_sceneName);
    }
}
