using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void cangeToJumakScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jumak");
    }
}
