using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public void OnClickRetryBtn()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void OnClickMenuBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
