using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OptionPopup : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public GameObject backgroundImage;

    private void Awake()
    {
        gameOverText.gameObject.SetActive(false);
        backgroundImage.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";
        backgroundImage.SetActive(true); 
    }
}
