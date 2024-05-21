using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    //public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    //---¾À ÀüÈ¯ ÇÔ¼ö---//
    public void LoadScene(Define.Scene type)
    {
        //CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name  = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
}
