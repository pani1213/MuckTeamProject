using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingScene : MonoBehaviour
{
    private float endingDelay = 7f;
    private bool isBossDead = false;

    public void OnBossDeath()
    {
        isBossDead = true;
        Invoke("NextScene", 5);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("EndingScene");
    }
}

