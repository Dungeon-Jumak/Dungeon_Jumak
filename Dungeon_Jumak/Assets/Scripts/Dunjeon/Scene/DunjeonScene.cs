using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DunjeonScene : MonoBehaviour
{
    public GameObject resultPanel;
    public GameObject monsterPrefab;

    private GameObject secTextObj;
    private GameObject minTextObj;

    private TextMeshProUGUI secText;
    private TextMeshProUGUI minText;

    private float time = 120;

    public int numberOfMonsters = 4; // 스폰할 몬스터의 수


    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private int bgmSoundTrack;
    [SerializeField]
    private float maxVolume;

    [SerializeField]
    private Data data;

    [SerializeField]
    private AudioManager audioManager;

    private bool playBGM = false;
    // Start is called before the first frame update
    void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.AllStop();
        bgmManager.Stop();

        //---BGM 사운드 트랙 설정---//
        bgmSoundTrack = 6;

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();

        secTextObj = GameObject.Find("SecText");
        secText = secTextObj.GetComponent<TextMeshProUGUI>();
        minTextObj = GameObject.Find("MinText");
        minText = minTextObj.GetComponent<TextMeshProUGUI>();

        SpawnMonsters();
    }

    void Update()
    {
        countTime();

        if (DataManager.Instance.data.ingredient[0] == 4)
        {
            resultPanel.SetActive(true);
        }

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

    public void SoundControl()
    {
        if (data.isSound)
        {
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 1f;
                audioManager.sounds[i].Setvolume();
            }
        }
        else
        {
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 0f;
                audioManager.sounds[i].Setvolume();
            }
        }
    }

    private void countTime()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            int min = (int)time / 60;
            int sec = ((int)time - min * 60) % 60;

            secText.text = sec.ToString("00");
            minText.text = min.ToString("00");
        }
        else
        {
            resultPanel.SetActive(true);
        }
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                0 
            );
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void ChangeToWaitingScene()
    {
        GameManager.Scene.LoadScene(Define.Scene.Map);
    }

    public void AddDay()
    {
        DataManager.Instance.data.days++;
    }
}
