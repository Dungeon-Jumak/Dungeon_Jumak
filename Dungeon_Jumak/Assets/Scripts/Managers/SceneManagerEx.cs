using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class SceneManagerEx 
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    //---씬 로드하는 함수---//
    public void LoadScene(Define.Scene type)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    //---Define.cs의 enum 사용해서 씬 이름 받아오는 함수---//
    string GetSceneName(Define.Scene type)
    {
        string name  = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
}
