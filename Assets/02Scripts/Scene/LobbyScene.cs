using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SecneNames
{
    Lobby,  // 0
    Main,   // 1
}
public class LobbyScene : MonoBehaviour
{
    public void OnClickStart()
    {
        Debug.Log("���ӽ���");
        SceneManager.LoadScene((int)SecneNames.Main);
    }
}
