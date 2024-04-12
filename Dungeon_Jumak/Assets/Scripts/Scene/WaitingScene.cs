using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingScene : MonoBehaviour
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

        //---BGM 사운드 트랙 설정---//
        bgmSoundTrack = 1;

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
    }

    // Update is called once per frame
    void Update()
    {
        BGMPlayer();

    }

    public void BGMPlayer()
    {
        if (!data.isPlayBGM)
        {
            playBGM = false;

            bgmManager.CancelLoop();
            bgmManager.Stop();
        }

        if (!playBGM && data.isPlayBGM)
        {
            playBGM = true;

            bgmManager.Play(bgmSoundTrack);
            bgmManager.FadeInMusic(maxVolume);
            bgmManager.SetLoop();
        }
    }

}
