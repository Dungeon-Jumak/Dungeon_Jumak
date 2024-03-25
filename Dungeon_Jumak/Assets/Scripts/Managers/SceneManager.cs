using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public void cangeToJumakScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jumak");
    }
}
